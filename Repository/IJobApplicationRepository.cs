using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.Repository
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();
        Task<JobApplication?> GetByIdAsync(int id);
        Task<JobApplication> CreateAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateAsync(JobApplication jobApplication);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
