using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Products.Commands;
using Market.Core.Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Market.Presentation.Api.Products;

//[ApiVersion("1.0")]
public class ProductController : BaseController
{
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        return await SendAsync(command, HttpStatusCode.Created);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        return await SendAsync(command, HttpStatusCode.OK);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(DeleteProductCommand command)
    {
        return await SendAsync(command, HttpStatusCode.OK);
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery]GetProductQuery query)
    {
        return await SendAsync(query);
    }
    
    [HttpGet("getById")]
    public async Task<IActionResult> GetById([FromQuery] GetByIdProductQuery query)
    {
        return await SendAsync(query);
    }
}




