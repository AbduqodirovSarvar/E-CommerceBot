using Bot.Application.Interfaces.HandleInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, [FromServices] IUpdateHandler handler, CancellationToken cancellationToken)
        {
            try
            {
                await handler.HandleUpdateAsync(update, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                await handler.HandleErrorAsync(ex, cancellationToken);
                return BadRequest(ex.Message);
            }
        }
    }
}
