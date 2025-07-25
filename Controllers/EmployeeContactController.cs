using human_resource_management.Dto;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HR")]
    public class EmployeeContactController : ControllerBase
    {
        private readonly HumanResourceManagementContext _context;

        public EmployeeContactController(HumanResourceManagementContext context)
        {
            _context = context;
        }

        [HttpPost("get-all-employee-contact")]
        [Produces("application/json")]
       
        public async Task<IActionResult> GetAllEmployeeContacts([FromBody] ContactFilterDto filter)
        {
            var query = _context.EmployeeContacts
                .Include(c => c.Employee)
                .AsQueryable();
     if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(c =>
                    c.ContactType.Contains(filter.Keyword) ||
                    c.ContactValue.Contains(filter.Keyword));
            }

 
            if (filter.EmployeeId.HasValue)
            {
                query = query.Where(c => c.EmployeeId == filter.EmployeeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(c => c.ContactType == filter.Type);
            }

      
            var totalRecords = await query.CountAsync();

       
            var contacts = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var contactDtos = contacts.Select(EmployeeContactMapper.ToDto).ToList();

            return Ok(new
            {
                data = contactDtos,
                totalRecords,
                totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize)
            });
        }


        [HttpPost("add-contact")]
        [Produces("application/json")]
        public async Task<IActionResult> AddContact([FromBody] CreateEmployeeContactDto dto)
        {
            if (dto.IsPrimary)
            {
                var checkIsDefault = _context.EmployeeContacts
                .FirstOrDefault(e => e.EmployeeId == dto.EmployeeId);
                if (checkIsDefault != null)
                {
                    checkIsDefault.IsPrimary = false;
                }
            }

            var contact = EmployeeContactMapper.ToEntity(dto);

            _context.EmployeeContacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok(EmployeeContactMapper.ToDto(contact));
        }


        [HttpDelete("delete-contact/{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> DeleteContactById(int id)
        {
            var contact = _context.EmployeeContacts
                .FirstOrDefault(e => e.ContactId == id);
            if (contact != null)
            {
                _context.EmployeeContacts.Remove(contact);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound("Không tồn tại contact với id : " + id);
        }

        [HttpGet("get-by-id/{id}")]
        [Produces("application/json")]

        public async Task<IActionResult> GetById(int id)
        {
            var contact = _context.EmployeeContacts
                .FirstOrDefault(e => e.ContactId == id);
            if (contact != null)
            {
                return Ok(EmployeeContactMapper.ToDto(contact));
            }
            return NotFound("Không tồn tại contact với id : " + id);
        }



        [HttpDelete("delete-contact/{employeeId:int}/{type:int}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteContactByEmployeeIdAndType(int employeeId, int type)
        {
            // Kiểm tra enum hợp lệ
            if (!Enum.IsDefined(typeof(ContactTypeEnum), type))
            {
                return BadRequest("Loại contact không hợp lệ.");
            }

            string contactType = ((ContactTypeEnum)type).ToString();

            var contact = await _context.EmployeeContacts
                .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.ContactType == contactType);

            if (contact == null)
            {
                return NotFound($"Không tìm thấy liên hệ của nhân viên {employeeId} với loại {contactType}.");
            }

            _context.EmployeeContacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok($"Đã xóa liên hệ '{contactType}' của nhân viên ID {employeeId}.");
        }


        [HttpPut("update-contact/{contactId:int}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateContact(int contactId, [FromBody] UpdateEmployeeContactDto dto)
        {
            var contact = await _context.EmployeeContacts.FindAsync(contactId);

            if (contact == null)
                return NotFound($"Không tìm thấy liên hệ với ContactId = {contactId}");

        
            if (dto.IsPrimary == true)
            {
                var oldPrimary = await _context.EmployeeContacts
                    .FirstOrDefaultAsync(c => c.EmployeeId == contact.EmployeeId && c.IsPrimary == true && c.ContactId != contactId);

                if (oldPrimary != null)
                {
                    oldPrimary.IsPrimary = false;
                }

                contact.IsPrimary = true;
            }
            else if (dto.IsPrimary.HasValue)
            {
                contact.IsPrimary = dto.IsPrimary.Value;
            }

            if (dto.ContactValue != null)
                contact.ContactValue = dto.ContactValue;

            if (dto.ContactType.HasValue)
                contact.ContactType = dto.ContactType.Value.ToString();

            await _context.SaveChangesAsync();

            return Ok(EmployeeContactMapper.ToDto(contact));
        }


    }
}