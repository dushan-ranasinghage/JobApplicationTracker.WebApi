using JobApplicationTracker.WebApi.DTOs;
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

        public async Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationsAsync()
        {
            var jobApplications = await _repository.GetAllAsync();
            return jobApplications.Select(MapToDto);
        }

        public async Task<JobApplicationDto?> GetJobApplicationByIdAsync(int id)
        {
            var jobApplication = await _repository.GetByIdAsync(id);
            return jobApplication == null ? null : MapToDto(jobApplication);
        }

        public async Task<JobApplicationDto> CreateJobApplicationAsync(CreateJobApplicationDto createDto)
        {
            var jobApplication = new JobApplication
            {
                CompanyName = createDto.CompanyName,
                Position = createDto.Position,
                Status = createDto.Status,
                DateApplied = createDto.DateApplied
            };

            var created = await _repository.CreateAsync(jobApplication);
            return MapToDto(created);
        }

        public async Task<JobApplicationDto> UpdateJobApplicationAsync(int id, UpdateJobApplicationDto updateDto)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Job application with ID {id} not found.");
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Job application with ID {id} not found.");
            }

            existing.CompanyName = updateDto.CompanyName;
            existing.Position = updateDto.Position;
            existing.Status = updateDto.Status;
            existing.DateApplied = updateDto.DateApplied;

            var updated = await _repository.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteJobApplicationAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static JobApplicationDto MapToDto(JobApplication jobApplication)
        {
            return new JobApplicationDto(
                Id: jobApplication.Id,
                CompanyName: jobApplication.CompanyName,
                Position: jobApplication.Position,
                Status: jobApplication.Status,
                DateApplied: jobApplication.DateApplied,
                CreatedAt: jobApplication.CreatedAt,
                UpdatedAt: jobApplication.UpdatedAt
            );
        }
    }
}
