using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market.Presentation.Api.Products
{
    //[ApiVersion("1.0")]
    public class ProductController : BaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCommand command) => await SendAsync(command);
        
    }
}
