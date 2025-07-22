using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace human_resource_management.Repository 
{
    public class PerformanceReviewRepository : IPerformanceReviewRepository
    {
        private readonly HumanResourceManagementContext _context;

        public PerformanceReviewRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PerformanceReview>> GetAllAsync()
        {
            return await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Include(r => r.Project)
                .ToListAsync();
        }

        public async Task<PerformanceReview?> GetByIdAsync(int id)
        {
            return await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }
        public async Task<IEnumerable<PerformanceReview>> GetByProjectIdAsync(int projectId)
        {
            return await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Include(r => r.Project)
                .Where(r => r.ProjectId == projectId)
                .ToListAsync();
        }
        public async Task AddAsync(PerformanceReview review)
        {
            _context.PerformanceReviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PerformanceReview review)
        {
            _context.PerformanceReviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PerformanceReview review)
        {
            _context.PerformanceReviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PerformanceReviews.AnyAsync(r => r.ReviewId == id);
        }
    }
}
