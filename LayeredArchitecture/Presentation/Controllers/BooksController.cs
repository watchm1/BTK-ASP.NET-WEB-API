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
    public IActionResult GetBooks()
    {
        var entities = _manager.BookService.GetAllBook(false);
        return Ok(entities);
    }
    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
         return Ok(_manager.BookService.GetOneBook(id, false));
    }
    [HttpPost]
    public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        _manager.BookService.CreateOneBook(bookDto);
        return StatusCode(201, bookDto);
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody]BookDtoForUpdate bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        _manager.BookService.UpdateOneBook(id, bookDto, false);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteOneBook([FromRoute]int id)
    {
        _manager.BookService.DeleteOneBook(id, false);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody]JsonPatchDocument<BookDtoForUpdate>? bookDtoPatch)
    {
        if (bookDtoPatch is null)
            return BadRequest();
        var result = _manager.BookService.GetOneBookForPatch(id, false);
        var entity = _manager.BookService.GetOneBook(id, true);

       
        bookDtoPatch.ApplyTo(result.bookDtoForUpdate, ModelState);
        TryValidateModel(result.bookDtoForUpdate);
        if(!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        _manager.BookService.SaveChangesForPatch(result.bookDtoForUpdate, result.book);
        return NoContent();
    }

}