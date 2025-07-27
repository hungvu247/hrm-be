using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Repository
{
    public class RequestRepository : IRequestFormRepository
    {
        private readonly HumanResourceManagementContext _context;

        public RequestRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestForm>> GetAllAsync(int? employeeId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null) return new List<RequestForm>();

            var query = _context.RequestForms
                .Include(r => r.Employee)
                .Include(r => r.RequestType)
                .Include(r => r.ReviewedByNavigation)
                .AsQueryable();

            switch (employee.RoleId)
            {
                case 6: //manager
                    // Role 6: Xem tất cả
                    return await query.ToListAsync();

                case 0: // employee
                    // Role 0: Chỉ xem các request của chính mình
                    return await query.Where(r => r.EmployeeId == employeeId).ToListAsync();
                case 1: //admin
                    // Role 0: Chỉ xem các request của chính mình
                    return await query.Where(r => r.EmployeeId == employeeId).ToListAsync();

                case 2: //hr
                    // Role 2: Xem của mình + các request loại 1 hoặc 4
                    return await query.Where(r =>
                        r.EmployeeId == employeeId ||
                        r.RequestTypeId == 1 ||
                        r.RequestTypeId == 4
                    ).ToListAsync();

                case 3://lead
                    // Role 3: Xem của mình + các nhân viên mình lead
                    var teamEmployeeIds = await _context.Employees
                        .Where(e => e.LeadEmployeeId == employeeId)
                        .Select(e => e.EmployeeId)
                        .ToListAsync();

                    return await query
                        .Where(r => r.EmployeeId == employeeId || teamEmployeeIds.Contains(r.EmployeeId))
                        .ToListAsync();

                default:
                    // Không có quyền
                    return new List<RequestForm>();
            }
        }

        public async Task AddAsync(RequestForm requestForm)
        {
            _context.RequestForms.Add(requestForm);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateStatusAsync(int id, string status, int reviewedById)
        {
            var request = await _context.RequestForms.FindAsync(id);
            if (request == null) return false;

            request.Status = status;
            request.ReviewedAt = DateTime.Now;
            request.ReviewedBy = reviewedById;

            await _context.SaveChangesAsync();
            return true;
        }



        //public async Task<RequestForm?> GetByIdAsync(int id)
        //{
        //    return await _context.RequestForms
        //        .Include(r => r.Employee)
        //        .Include(r => r.RequestType)
        //        .Include(r => r.ReviewedByNavigation)
        //        .FirstOrDefaultAsync(r => r.RequestId == id);
        //}

        //public async Task AddAsync(RequestForm form)
        //{
        //    await _context.RequestForms.AddAsync(form);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(RequestForm form)
        //{
        //    _context.RequestForms.Update(form);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(RequestForm form)
        //{
        //    _context.RequestForms.Remove(form);
        //    await _context.SaveChangesAsync();
        //}

        //Task<IEnumerable<RequestForm>> IRequestFormRepository.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
