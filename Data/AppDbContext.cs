using JobApplicationTracker.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.WebApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    }
}
