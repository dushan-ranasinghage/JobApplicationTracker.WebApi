using FluentAssertions;
using JobApplicationTracker.WebApi.Controllers;
using JobApplicationTracker.WebApi.DTOs;
using JobApplicationTracker.WebApi.Models;
using JobApplicationTracker.WebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JobApplicationTracker.WebApi.Tests.Controllers
{
    public class JobApplicationsControllerTests
    {
        private readonly Mock<IJobApplicationService> _mockService;
        private readonly JobApplicationsController _controller;

        public JobApplicationsControllerTests()
        {
            _mockService = new Mock<IJobApplicationService>();
            _controller = new JobApplicationsController(_mockService.Object);
        }

        [Fact]
        public async Task GetJobApplications_ReturnsAllApplications_WhenNoPaginationProvided()
        {
            var expectedApplications = new List<JobApplicationDto>
            {
                new JobApplicationDto(1, "Company A", "Developer", JobApplicationStatus.Applied, DateTime.UtcNow, DateTime.UtcNow, null),
                new JobApplicationDto(2, "Company B", "Engineer", JobApplicationStatus.Interview, DateTime.UtcNow, DateTime.UtcNow, null)
            };

            _mockService.Setup(s => s.GetAllJobApplicationsAsync())
                .ReturnsAsync(expectedApplications);

            var result = await _controller.GetJobApplications(null, null);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var applications = okResult.Value.Should().BeAssignableTo<IEnumerable<JobApplicationDto>>().Subject;
            applications.Should().HaveCount(2);
            _mockService.Verify(s => s.GetAllJobApplicationsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetJobApplications_ReturnsPagedResult_WhenPaginationProvided()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var pagedResult = new PagedResponseDto<JobApplicationDto>(
                Data: new List<JobApplicationDto>
                {
                    new JobApplicationDto(1, "Company A", "Developer", JobApplicationStatus.Applied, DateTime.UtcNow, DateTime.UtcNow, null)
                },
                PageNumber: pageNumber,
                PageSize: pageSize,
                TotalCount: 1,
                TotalPages: 1
            );

            _mockService.Setup(s => s.GetAllJobApplicationsPagedAsync(pageNumber, pageSize))
                .ReturnsAsync(pagedResult);

            var result = await _controller.GetJobApplications(pageNumber, pageSize);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<PagedResponseDto<JobApplicationDto>>().Subject;
            response.PageNumber.Should().Be(pageNumber);
            response.PageSize.Should().Be(pageSize);
            response.TotalCount.Should().Be(1);
            _mockService.Verify(s => s.GetAllJobApplicationsPagedAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetJobApplication_ReturnsApplication_WhenExists()
        {
            var id = 1;
            var expectedApplication = new JobApplicationDto(
                id, "Company A", "Developer", JobApplicationStatus.Applied, DateTime.UtcNow, DateTime.UtcNow, null);

            _mockService.Setup(s => s.GetJobApplicationByIdAsync(id))
                .ReturnsAsync(expectedApplication);

            var result = await _controller.GetJobApplication(id);

            var application = result.Value.Should().BeOfType<JobApplicationDto>().Subject;
            application.Id.Should().Be(id);
            application.CompanyName.Should().Be("Company A");
            _mockService.Verify(s => s.GetJobApplicationByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetJobApplication_ReturnsNotFound_WhenNotExists()
        {
            var id = 999;
            _mockService.Setup(s => s.GetJobApplicationByIdAsync(id))
                .ReturnsAsync((JobApplicationDto?)null);

            var result = await _controller.GetJobApplication(id);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Value.Should().BeNull();
            _mockService.Verify(s => s.GetJobApplicationByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task PostJobApplication_ReturnsCreated_WhenValid()
        {
            var createDto = new CreateJobApplicationDto("Company A", "Developer", JobApplicationStatus.Applied);
            var createdApplication = new JobApplicationDto(
                1, "Company A", "Developer", JobApplicationStatus.Applied, DateTime.UtcNow, DateTime.UtcNow, null);

            _mockService.Setup(s => s.CreateJobApplicationAsync(createDto))
                .ReturnsAsync(createdApplication);

            var result = await _controller.PostJobApplication(createDto);

            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var application = createdResult.Value.Should().BeOfType<JobApplicationDto>().Subject;
            application.Id.Should().Be(1);
            application.CompanyName.Should().Be("Company A");
            _mockService.Verify(s => s.CreateJobApplicationAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task PostJobApplication_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var createDto = new CreateJobApplicationDto("", "Developer", JobApplicationStatus.Applied);
            _controller.ModelState.AddModelError("CompanyName", "Company name is required");

            var result = await _controller.PostJobApplication(createDto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _mockService.Verify(s => s.CreateJobApplicationAsync(It.IsAny<CreateJobApplicationDto>()), Times.Never);
        }

        [Fact]
        public async Task PutJobApplication_ReturnsNoContent_WhenValid()
        {
            var id = 1;
            var updateDto = new UpdateJobApplicationDto("Company A Updated", "Senior Developer", JobApplicationStatus.Interview);

            _mockService.Setup(s => s.UpdateJobApplicationAsync(id, updateDto))
                .ReturnsAsync(new JobApplicationDto(id, "Company A Updated", "Senior Developer", JobApplicationStatus.Interview, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow));

            var result = await _controller.PutJobApplication(id, updateDto);

            result.Should().BeOfType<NoContentResult>();
            _mockService.Verify(s => s.UpdateJobApplicationAsync(id, updateDto), Times.Once);
        }

        [Fact]
        public async Task PutJobApplication_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var id = 1;
            var updateDto = new UpdateJobApplicationDto("", "Developer", JobApplicationStatus.Applied);
            _controller.ModelState.AddModelError("CompanyName", "Company name is required");

            var result = await _controller.PutJobApplication(id, updateDto);

            result.Should().BeOfType<BadRequestObjectResult>();
            _mockService.Verify(s => s.UpdateJobApplicationAsync(It.IsAny<int>(), It.IsAny<UpdateJobApplicationDto>()), Times.Never);
        }

        [Fact]
        public async Task DeleteJobApplication_ReturnsNoContent_WhenDeleted()
        {
            var id = 1;
            _mockService.Setup(s => s.DeleteJobApplicationAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteJobApplication(id);

            result.Should().BeOfType<NoContentResult>();
            _mockService.Verify(s => s.DeleteJobApplicationAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteJobApplication_ReturnsNotFound_WhenNotExists()
        {
            var id = 999;
            _mockService.Setup(s => s.DeleteJobApplicationAsync(id))
                .ReturnsAsync(false);

            var result = await _controller.DeleteJobApplication(id);

            result.Should().BeOfType<NotFoundResult>();
            _mockService.Verify(s => s.DeleteJobApplicationAsync(id), Times.Once);
        }
    }
}
