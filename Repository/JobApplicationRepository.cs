using JobApplicationTracker.WebApi.Data;
using JobApplicationTracker.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.WebApi.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await _context.JobApplications.ToListAsync();
        }

        public async Task<(IEnumerable<JobApplication> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.JobApplications.CountAsync();
            var items = await _context.JobApplications
                .OrderByDescending(ja => ja.DateApplied)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return (items, totalCount);
        }

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _context.JobApplications.FindAsync(id);
        }

        public async Task<JobApplication> CreateAsync(JobApplication jobApplication)
        {
            jobApplication.CreatedAt = DateTime.UtcNow;
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task<JobApplication> UpdateAsync(JobApplication jobApplication)
        {
            jobApplication.UpdatedAt = DateTime.UtcNow;
            _context.Entry(jobApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return false;
            }

            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.JobApplications.AnyAsync(e => e.Id == id);
        }
    }
}
