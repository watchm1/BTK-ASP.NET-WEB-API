using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebApi.Controllers;
[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IRepositoryManager _manager;

    public BooksController(IRepositoryManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var entities = _manager.BookRepository.GetAllBooks(false);
        if(entities != null)
            return Ok(entities);
        else
            return NotFound();
    }
    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        try
        {
            var entity = _manager.BookRepository.GetOneBook(id, false); 
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
            _manager.BookRepository.CreateOneBook(book);
            _manager.Save();
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
            var entity = _manager.BookRepository.GetOneBook(id, true);
            if(entity is null)
                return NotFound();

            if(id != book.Id)
                return BadRequest();
            entity.Title = book.Title;
            entity.Price = book.Price;
            _manager.Save(); 
            return Ok(book);
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
           var entity = _manager.BookRepository.GetOneBook(id, false); 
            if(entity is null)
                return NotFound(new {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                });
            _manager.BookRepository.DeleteOneBook(entity);
            _manager.Save();
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
            var entity = _manager.BookRepository.GetOneBook(id, true);
            if (entity == null)
                return NotFound();
            bookPatch.ApplyTo(entity);
            _manager.BookRepository.UpdateOneBook(entity);
            _manager.Save();
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}