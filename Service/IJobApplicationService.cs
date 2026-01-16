using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.Service
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplication>> GetAllJobApplicationsAsync();
        Task<JobApplication?> GetJobApplicationByIdAsync(int id);
        Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateJobApplicationAsync(int id, JobApplication jobApplication);
        Task<bool> DeleteJobApplicationAsync(int id);
    }
}
