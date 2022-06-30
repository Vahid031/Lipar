using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Categories.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Market.Presentation.Api.Categories;

public class CategoryController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        return await SendAsync(command, HttpStatusCode.Created);
    }
    
    //[HttpPut("update")]
    //public async Task<IActionResult> Update(UpdateCategoryCommand command)
    //{
        //    return await SendAsync(command, HttpStatusCode.OK);
    //}
    
    //[HttpDelete("delete")]
    //public async Task<IActionResult> Delete(DeleteCategoryCommand command)
    //{
        //    return await SendAsync(command, HttpStatusCode.OK);
    //}
    
    //[HttpGet("get")]
    //public async Task<IActionResult> Get([FromQuery] GetCategoryQuery query)
    //{
        //    return await SendAsync(query);
    //}
}


