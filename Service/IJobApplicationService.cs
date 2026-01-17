using JobApplicationTracker.WebApi.DTOs;

namespace JobApplicationTracker.WebApi.Service
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationsAsync();
        Task<PagedResponseDto<JobApplicationDto>> GetAllJobApplicationsPagedAsync(int pageNumber, int pageSize);
        Task<JobApplicationDto?> GetJobApplicationByIdAsync(int id);
        Task<JobApplicationDto> CreateJobApplicationAsync(CreateJobApplicationDto createDto);
        Task<JobApplicationDto> UpdateJobApplicationAsync(int id, UpdateJobApplicationDto updateDto);
        Task<bool> DeleteJobApplicationAsync(int id);
    }
}
