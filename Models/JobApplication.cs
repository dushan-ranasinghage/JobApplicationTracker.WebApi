using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.WebApi.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Position { get; set; } = string.Empty;

        [Required]
        public JobApplicationStatus Status { get; set; }

        [Required]
        public DateTime DateApplied { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    public enum JobApplicationStatus
    {
        Applied = 1,
        Interview = 2,
        Offer = 3,
        Rejected = 4,
        Accepted = 5
    }

}
