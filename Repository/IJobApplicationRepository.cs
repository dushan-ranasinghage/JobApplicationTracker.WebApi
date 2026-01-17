using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.Repository
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();
        Task<(IEnumerable<JobApplication> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<JobApplication?> GetByIdAsync(int id);
        Task<JobApplication> CreateAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateAsync(JobApplication jobApplication);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
