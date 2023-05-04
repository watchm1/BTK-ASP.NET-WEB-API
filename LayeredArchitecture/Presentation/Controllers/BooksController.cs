using System.Text.Json;
using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;


[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _manager;

    public BooksController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery]BookParameters bookParameters)
    {
        var pagedResult = await _manager.BookService.GetAllBookAsync(bookParameters, false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.Item1);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
    {
         return Ok(await _manager.BookService.GetOneBookAsync(id, false));
    }
    [ServiceFilter(typeof(ValidationFilterAttribute),Order = 1)]
    [ServiceFilter(typeof(LogFilterAttribute), Order = 2)]
    [HttpPost]
    public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion bookDto)
    {
        await _manager.BookService.CreateOneBookAsync(bookDto);
        return StatusCode(201, bookDto);
    }
    [ServiceFilter(typeof(ValidationFilterAttribute),Order = 1)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody]BookDtoForUpdate bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
        return NoContent();
    }
    [ServiceFilter(typeof(ValidationFilterAttribute),Order = 1)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOneBook([FromRoute]int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, false);
        return NoContent();
    }
    [ServiceFilter(typeof(ValidationFilterAttribute),Order = 1)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody]JsonPatchDocument<BookDtoForUpdate>? bookDtoPatch)
    {
        if (bookDtoPatch is null)
            return BadRequest();
        var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);
        var entity = await _manager.BookService.GetOneBookAsync(id, true);

       
        bookDtoPatch.ApplyTo(result.bookDtoForUpdate, ModelState);
        TryValidateModel(result.bookDtoForUpdate);
        if(!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);
        return NoContent();
    }

}