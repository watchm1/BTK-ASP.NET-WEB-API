using Entities.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    public async Task<IActionResult> GetBooks()
    {
        var entities = await _manager.BookService.GetAllBookAsync(false);
        return Ok(entities);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
    {
         return Ok(await _manager.BookService.GetOneBookAsync(id, false));
    }
    [HttpPost]
    public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        await _manager.BookService.CreateOneBookAsync(bookDto);
        return StatusCode(201, bookDto);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody]BookDtoForUpdate bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOneBook([FromRoute]int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, false);
        return NoContent();
    }

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