using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        if (entities is null)
            return NotFound();
        return Ok(entities);
    }
    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
         return Ok(_manager.BookService.GetOneBook(id, false));
    }
    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        if(book is null) 
            return BadRequest();
        _manager.BookService.CreateOneBook(book);
        return StatusCode(201, book);   
       
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody]BookDtoForUpdate bookDto)
    {
        if (bookDto is null)
            return BadRequest();
        _manager.BookService.UpdateOneBook(id, bookDto, true);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteOneBook([FromRoute]int id)
    {
        _manager.BookService.DeleteOneBook(id, false);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody]JsonPatchDocument<Book> bookPatch)
    {
        var entity = _manager.BookService.GetOneBook(id, true);
        
        bookPatch.ApplyTo(entity);
        _manager.BookService.UpdateOneBook(id, new BookDtoForUpdate(entity.Id, entity.Title, entity.Price), true);
        return NoContent();
    }

}