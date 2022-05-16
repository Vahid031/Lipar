using Lipar.Core.Contract.Utilities;
using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Products.Commands;
using Market.Core.Application.Products.Queries;
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
        private readonly IJson json;

        public ProductController(ILogger<ProductController> logger, IJson json)
        {
            this.logger = logger;
            this.json = json;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            return await SendAsync(command, HttpStatusCode.Created);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery]GetProductQuery query)
        {
            return await SendAsync(query);
        }
    }

   
}
