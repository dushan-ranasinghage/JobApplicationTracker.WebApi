using JobApplicationTracker.WebApi.Models;
using JobApplicationTracker.WebApi.Repository;

namespace JobApplicationTracker.WebApi.Service
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repository;

        public JobApplicationService(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobApplication>> GetAllJobApplicationsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<JobApplication?> GetJobApplicationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication)
        {
            return await _repository.CreateAsync(jobApplication);
        }

        public async Task<JobApplication> UpdateJobApplicationAsync(int id, JobApplication jobApplication)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Job application with ID {id} not found.");
            }

            jobApplication.Id = id;
            return await _repository.UpdateAsync(jobApplication);
        }

        public async Task<bool> DeleteJobApplicationAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
