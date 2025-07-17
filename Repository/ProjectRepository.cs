using System;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;
namespace human_resource_management.Repository
{
    public class ProjectRepository : Mapper.IProjectRepository
    {
        private readonly HumanResourceManagementContext _context;

        public ProjectRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync() =>
            await _context.Projects.ToListAsync();

        public async Task<Project?> GetByIdAsync(int id) =>
            await _context.Projects.FindAsync(id);

        public async Task<Project> AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
