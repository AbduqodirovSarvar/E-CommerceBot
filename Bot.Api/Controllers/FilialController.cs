using Bot.Application.Interfaces.UseCaseInterfaces;
using Bot.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilialController : ControllerBase
    {
        private readonly IFilialServices _filialServices;
        public FilialController(IFilialServices filialServices)
        {
            _filialServices = filialServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Filial filial, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _filialServices.CreateFilial(filial, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _filialServices.GetAll(cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _filialServices.DeleteFilial(Id, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
