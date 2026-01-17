using Microsoft.AspNetCore.Mvc;
using JobApplicationTracker.WebApi.DTOs;
using JobApplicationTracker.WebApi.Service;

namespace JobApplicationTracker.WebApi.Controllers
{
    [Route("api/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _service;

        public JobApplicationsController(IJobApplicationService service)
        {
            _service = service;
        }

        // GET: api/job-applications
        [HttpGet]
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

        // GET: api/job-applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplication(int id)
        {
            var jobApplication = await _service.GetJobApplicationByIdAsync(id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return jobApplication;
        }

        // PUT: api/job-applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobApplication(int id, UpdateJobApplicationDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateJobApplicationAsync(id, updateDto);
            return NoContent();
        }

        // POST: api/job-applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobApplicationDto>> PostJobApplication(CreateJobApplicationDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateJobApplicationAsync(createDto);
            return CreatedAtAction("GetJobApplication", new { id = created.Id }, created);
        }

        // DELETE: api/job-applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            var deleted = await _service.DeleteJobApplicationAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/job-applications/test-exception
        // This endpoint is for testing the global exception handler
        [HttpGet("test-exception")]
        public IActionResult TestException()
        {
            throw new Exception("This is a test exception to verify global exception handling");
        }
    }
}
