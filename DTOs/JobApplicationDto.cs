using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.DTOs
{
    public record JobApplicationDto(
        int Id,
        string CompanyName,
        string Position,
        JobApplicationStatus Status,
        DateTime DateApplied,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
