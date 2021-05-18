using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Market.Presentation.Api.Products
{
    //[ApiVersion("1.0")]
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> logger;

        public ProductController(ILogger<ProductController> logger)
        {
            this.logger = logger;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            logger.LogWarning("salam");

            return await SendAsync(command, HttpStatusCode.Created);
        }

    }
}
