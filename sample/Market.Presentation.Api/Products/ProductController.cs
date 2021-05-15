using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market.Presentation.Api.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            return await Execute(command);
        }
    }
}
