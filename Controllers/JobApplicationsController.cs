using Microsoft.AspNetCore.Mvc;
using JobApplicationTracker.WebApi.DTOs;
using JobApplicationTracker.WebApi.Service;

namespace JobApplicationTracker.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing job applications
    /// </summary>
    [Route("api/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _service;

        public JobApplicationsController(IJobApplicationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all job applications
        /// </summary>
        /// <param name="pageNumber">Page number for pagination (optional). If provided, pageSize must also be provided.</param>
        /// <param name="pageSize">Number of items per page (optional). If provided, pageNumber must also be provided.</param>
        /// <returns>List of job applications. Returns paginated results if both pageNumber and pageSize are provided, otherwise returns all applications.</returns>
        /// <response code="200">Returns the list of job applications</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResponseDto<JobApplicationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetJobApplications([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            // If pagination parameters are provided, return paginated results
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var pagedResult = await _service.GetAllJobApplicationsPagedAsync(pageNumber.Value, pageSize.Value);
                return Ok(pagedResult);
            }

            // Otherwise, return all results
            var jobApplications = await _service.GetAllJobApplicationsAsync();
            return Ok(jobApplications);
        }

        /// <summary>
        /// Get a job application by ID
        /// </summary>
        /// <param name="id">The unique identifier of the job application</param>
        /// <returns>The job application with the specified ID</returns>
        /// <response code="200">Returns the requested job application</response>
        /// <response code="404">If the job application is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplication(int id)
        {
            var jobApplication = await _service.GetJobApplicationByIdAsync(id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return jobApplication;
        }

        /// <summary>
        /// Update an existing job application
        /// </summary>
        /// <param name="id">The unique identifier of the job application to update</param>
        /// <param name="updateDto">The updated job application data. Note: DateApplied cannot be updated as it is set automatically on creation.</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">Job application updated successfully</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="404">If the job application is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutJobApplication(int id, UpdateJobApplicationDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateJobApplicationAsync(id, updateDto);
            return NoContent();
        }

        /// <summary>
        /// Create a new job application
        /// </summary>
        /// <param name="createDto">The job application data to create. DateApplied is automatically set to the current UTC time.</param>
        /// <returns>The newly created job application</returns>
        /// <response code="201">Returns the newly created job application</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobApplicationDto>> PostJobApplication(CreateJobApplicationDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateJobApplicationAsync(createDto);
            return CreatedAtAction("GetJobApplication", new { id = created.Id }, created);
        }

        /// <summary>
        /// Delete a job application
        /// </summary>
        /// <param name="id">The unique identifier of the job application to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">Job application deleted successfully</response>
        /// <response code="404">If the job application is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            var deleted = await _service.DeleteJobApplicationAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
