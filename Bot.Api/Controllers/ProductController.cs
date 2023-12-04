using Bot.Application.Interfaces.UseCaseInterfaces;
using Bot.Application.Services.UseCases;
using Bot.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto product, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _productServices.CreateProduct(product, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("Type")]
        public async Task<IActionResult> CreateType([FromBody] ProductType productType, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _productServices.CreateProductType(productType, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
