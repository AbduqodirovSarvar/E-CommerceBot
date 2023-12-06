using Bot.Application.Interfaces.FileServiceInterfaces;
using Bot.Application.Services.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<TestController> _logger;
        public TestController(IFileService fileService, ILogger<TestController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            try
            {
                return Ok(await _fileService.Save(file));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
