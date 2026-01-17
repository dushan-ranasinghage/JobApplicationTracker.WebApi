using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.DTOs
{
    /// <summary>
    /// Data transfer object representing a job application
    /// </summary>
    /// <param name="Id">Unique identifier of the job application</param>
    /// <param name="CompanyName">Name of the company</param>
    /// <param name="Position">Job position title</param>
    /// <param name="Status">Current status of the application</param>
    /// <param name="DateApplied">Date and time when the application was submitted (UTC)</param>
    /// <param name="CreatedAt">Date and time when the record was created (UTC)</param>
    /// <param name="UpdatedAt">Date and time when the record was last updated (UTC). Null if never updated.</param>
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
