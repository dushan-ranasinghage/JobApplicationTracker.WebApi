using FluentAssertions;
using JobApplicationTracker.WebApi.DTOs;
using JobApplicationTracker.WebApi.Models;
using JobApplicationTracker.WebApi.Repository;
using JobApplicationTracker.WebApi.Service;
using Moq;
using Xunit;

namespace JobApplicationTracker.WebApi.Tests.Service
{
    public class JobApplicationServiceTests
    {
        private readonly Mock<IJobApplicationRepository> _mockRepository;
        private readonly JobApplicationService _service;

        public JobApplicationServiceTests()
        {
            _mockRepository = new Mock<IJobApplicationRepository>();
            _service = new JobApplicationService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllJobApplicationsAsync_ReturnsAllApplications()
        {
            var jobApplications = new List<JobApplication>
            {
                new JobApplication
                {
                    Id = 1,
                    CompanyName = "Company A",
                    Position = "Developer",
                    Status = JobApplicationStatus.Applied,
                    DateApplied = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },
                new JobApplication
                {
                    Id = 2,
                    CompanyName = "Company B",
                    Position = "Engineer",
                    Status = JobApplicationStatus.Interview,
                    DateApplied = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(jobApplications);

            var result = await _service.GetAllJobApplicationsAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.First().CompanyName.Should().Be("Company A");
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllJobApplicationsPagedAsync_ReturnsPagedResult()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var jobApplications = new List<JobApplication>
            {
                new JobApplication
                {
                    Id = 1,
                    CompanyName = "Company A",
                    Position = "Developer",
                    Status = JobApplicationStatus.Applied,
                    DateApplied = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            };
            var totalCount = 1;

            _mockRepository.Setup(r => r.GetAllPagedAsync(pageNumber, pageSize))
                .ReturnsAsync((jobApplications, totalCount));

            var result = await _service.GetAllJobApplicationsPagedAsync(pageNumber, pageSize);

            result.Should().NotBeNull();
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(totalCount);
            result.TotalPages.Should().Be(1);
            result.Data.Should().HaveCount(1);
            _mockRepository.Verify(r => r.GetAllPagedAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetAllJobApplicationsPagedAsync_ThrowsException_WhenPageNumberIsInvalid()
        {
            var pageNumber = 0;
            var pageSize = 10;

            var act = async () => await _service.GetAllJobApplicationsPagedAsync(pageNumber, pageSize);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Page number must be greater than 0. (Parameter 'pageNumber')");
            _mockRepository.Verify(r => r.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllJobApplicationsPagedAsync_ThrowsException_WhenPageSizeIsInvalid()
        {
            var pageNumber = 1;
            var pageSize = 0;

            var act = async () => await _service.GetAllJobApplicationsPagedAsync(pageNumber, pageSize);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Page size must be greater than 0. (Parameter 'pageSize')");
            _mockRepository.Verify(r => r.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetJobApplicationByIdAsync_ReturnsApplication_WhenExists()
        {
            var id = 1;
            var jobApplication = new JobApplication
            {
                Id = id,
                CompanyName = "Company A",
                Position = "Developer",
                Status = JobApplicationStatus.Applied,
                DateApplied = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(jobApplication);

            var result = await _service.GetJobApplicationByIdAsync(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.CompanyName.Should().Be("Company A");
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetJobApplicationByIdAsync_ReturnsNull_WhenNotExists()
        {
            var id = 999;
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((JobApplication?)null);

            var result = await _service.GetJobApplicationByIdAsync(id);

            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task CreateJobApplicationAsync_CreatesAndReturnsApplication()
        {
            var createDto = new CreateJobApplicationDto("Company A", "Developer", JobApplicationStatus.Applied);
            var createdApplication = new JobApplication
            {
                Id = 1,
                CompanyName = "Company A",
                Position = "Developer",
                Status = JobApplicationStatus.Applied,
                DateApplied = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.CreateAsync(It.Is<JobApplication>(ja =>
                ja.CompanyName == createDto.CompanyName &&
                ja.Position == createDto.Position &&
                ja.Status == createDto.Status)))
                .ReturnsAsync(createdApplication);

            var result = await _service.CreateJobApplicationAsync(createDto);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.CompanyName.Should().Be("Company A");
            result.Position.Should().Be("Developer");
            result.Status.Should().Be(JobApplicationStatus.Applied);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<JobApplication>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobApplicationAsync_UpdatesAndReturnsApplication_WhenExists()
        {
            var id = 1;
            var updateDto = new UpdateJobApplicationDto("Company A Updated", "Senior Developer", JobApplicationStatus.Interview);
            var existingApplication = new JobApplication
            {
                Id = id,
                CompanyName = "Company A",
                Position = "Developer",
                Status = JobApplicationStatus.Applied,
                DateApplied = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            var updatedApplication = new JobApplication
            {
                Id = id,
                CompanyName = "Company A Updated",
                Position = "Senior Developer",
                Status = JobApplicationStatus.Interview,
                DateApplied = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.ExistsAsync(id))
                .ReturnsAsync(true);
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingApplication);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<JobApplication>()))
                .ReturnsAsync(updatedApplication);

            var result = await _service.UpdateJobApplicationAsync(id, updateDto);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.CompanyName.Should().Be("Company A Updated");
            result.Position.Should().Be("Senior Developer");
            result.Status.Should().Be(JobApplicationStatus.Interview);
            _mockRepository.Verify(r => r.ExistsAsync(id), Times.Once);
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobApplicationAsync_ThrowsKeyNotFoundException_WhenNotExists()
        {
            var id = 999;
            var updateDto = new UpdateJobApplicationDto("Company A", "Developer", JobApplicationStatus.Applied);

            _mockRepository.Setup(r => r.ExistsAsync(id))
                .ReturnsAsync(false);

            var act = async () => await _service.UpdateJobApplicationAsync(id, updateDto);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Job application with ID {id} not found.");
            _mockRepository.Verify(r => r.ExistsAsync(id), Times.Once);
            _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task DeleteJobApplicationAsync_ReturnsTrue_WhenDeleted()
        {
            var id = 1;
            _mockRepository.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(true);

            var result = await _service.DeleteJobApplicationAsync(id);

            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteJobApplicationAsync_ReturnsFalse_WhenNotExists()
        {
            var id = 999;
            _mockRepository.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(false);

            var result = await _service.DeleteJobApplicationAsync(id);

            result.Should().BeFalse();
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
