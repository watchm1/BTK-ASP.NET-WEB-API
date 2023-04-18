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
        try
        {
            var entity = _manager.BookService.GetOneBook(id, false);
            if(entity == null)
                return NotFound();
            return Ok(entity);
        }
        catch (Exception ex)
        {   
            throw new Exception(ex.Message);
        }
       
    }
    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        try
        {
            if(book is null)
                return BadRequest();
            _manager.BookService.CreateOneBook(book);
            return StatusCode(201, book);   
        }
        catch (Exception ex)
        {    
            throw new Exception(ex.Message);
        }
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody]Book book)
    {
        try
        {
            if (book is null)
                return BadRequest();
            _manager.BookService.UpdateOneBook(id, book, true);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteOneBook([FromRoute]int id)
    {
       try
       {
           _manager.BookService.DeleteOneBook(id, false);
            return NoContent();
       }
       catch (Exception ex)
       { 
        throw new Exception(ex.Message);
       }
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody]JsonPatchDocument<Book> bookPatch)
    {
        try
        {
            var entity = _manager.BookService.GetOneBook(id, true);
            if (entity == null)
                return NotFound();
            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, entity, true);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}