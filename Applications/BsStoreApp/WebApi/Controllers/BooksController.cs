using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly RepositoryCT _context;
    public BooksController(RepositoryCT context)
    {
        this._context = context;   
    }
    [HttpGet]
    public IActionResult GetBooks()
    {
        var entities = _context.Books.ToList();
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
            var entity = _context.Books.Where(b=> b.Id.Equals(id)).SingleOrDefault();
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
            _context.Books.Add(book);
            _context.SaveChanges();
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
            var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
            if(entity is null)
                return NotFound();

            if(id != book.Id)
                return BadRequest();
            entity.Title = book.Title;
            entity.Price = book.Price;
            _context.SaveChanges();
            return Ok(book);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    [HttpDelete]
    public ActionResult DeleteAll()
    {
        try
        {
            _context.Books.RemoveRange(_context.Books);
            return Ok(new {
                statusCode = "200",
                message = "Deleted All Books."
            });
        }
        catch (Exception ex)
        {   
            throw new Exception(ex.Message);
        }
    }
    [HttpDelete("{id:int}")]
    public ActionResult DeleteAll([FromRoute]int id)
    {
       try
       {
             var entity = _context.Books.Where(b=> b.Id.Equals(id)).SingleOrDefault();
            if(entity is null)
                return NotFound(new {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                });
            _context.Books.Remove(entity);
            _context.SaveChanges();
            return NoContent();
       }
       catch (Exception ex)
       { 
        throw new Exception(ex.Message);
       }
    }

}
