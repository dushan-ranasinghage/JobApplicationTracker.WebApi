using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.WebApi.Models;

namespace JobApplicationTracker.WebApi.DTOs
{
    public class CreateJobApplicationDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [MaxLength(200, ErrorMessage = "Position cannot exceed 200 characters")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(JobApplicationStatus), ErrorMessage = "Invalid status value")]
        public JobApplicationStatus Status { get; set; }

        [Required(ErrorMessage = "Date applied is required")]
        public DateTime DateApplied { get; set; }
    }
}
