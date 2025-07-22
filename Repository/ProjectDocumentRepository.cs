using human_resource_management.Mapper;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace human_resource_management.Repository
{
    public class ProjectDocumentRepository :IProjectDocumentRepository
    {
        private readonly HumanResourceManagementContext _context;
        public ProjectDocumentRepository(HumanResourceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectDocument>> GetAllAsync() =>
            await _context.ProjectDocuments.ToListAsync();

        public async Task<ProjectDocument?> GetByIdAsync(int id) =>
            await _context.ProjectDocuments.FindAsync(id);
        public async Task<List<ProjectDocument>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProjectDocuments
                                 .Where(d => d.ProjectId == projectId)
                                 .ToListAsync();
        }

        public async Task AddAsync(ProjectDocument document)
        {
            _context.ProjectDocuments.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectDocument document)
        {
            _context.ProjectDocuments.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectDocument document)
        {
            _context.ProjectDocuments.Remove(document);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.ProjectDocuments.AnyAsync(d => d.DocumentId == id);
    }
}
