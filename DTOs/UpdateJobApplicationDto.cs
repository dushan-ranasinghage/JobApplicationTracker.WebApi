using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.DTOs
{
    public record UpdateJobApplicationDto(
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        string CompanyName,

        [Required(ErrorMessage = "Position is required")]
        [MaxLength(200, ErrorMessage = "Position cannot exceed 200 characters")]
        string Position,

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(JobApplicationStatus), ErrorMessage = "Invalid status value")]
        JobApplicationStatus Status,

        [Required(ErrorMessage = "Date applied is required")]
        DateTime DateApplied
    );
}
