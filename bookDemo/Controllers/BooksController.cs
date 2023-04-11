using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace bookDemo.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var books = ApplicationContext.Books.ToList();
        return Ok(books);
    }
    [HttpGet("{id:int}")]
    public IActionResult GetBookById(int id)
    {
        var book = ApplicationContext.Books.Find(item => item.Id == id);
        if(book != null)
            return Ok(book);
        else
            return NotFound();

    }
    [HttpPost]
    public IActionResult AddNewBook([FromBody]Book book)
    {
        try
        {
            if(book == null)
                return BadRequest();
                Console.WriteLine("Book already exists");
            ApplicationContext.Books.Add(book);
            return StatusCode(201, book);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name="id")]int id,[FromBody]Book book)
    {
        var entity = ApplicationContext.Books.Find(b=> b.Id.Equals(id));
        if(entity == null)
            return NotFound();
        if(id != book.Id)
            return BadRequest("Invalid argument");
        
        ApplicationContext.Books.Remove(entity);
        book.Id = book.Id;
        ApplicationContext.Books.Add(book);
        return Ok(book);
    }
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        ApplicationContext.Books.Clear();
        return Ok("Deleted all Data");
    }
    [HttpDelete("{id:int}")]
    public IActionResult DeleteById([FromRoute(Name="id")]int id)
    {
        var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
        if(entity == null)
            return NotFound(new {
                status =  "404",
                message ="The requested entity does not exist",
                requirement = ""
            });
        
        ApplicationContext.Books.Remove(entity);
        return NoContent();
    }
    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name="id")]int id, [FromBody] JsonPatchDocument<Book> bookPatch)
    {
        // check entity
        var entity = ApplicationContext.Books.Find(b=> b.Id.Equals(id));
        if(entity == null)
            return NotFound();
        bookPatch.ApplyTo(entity);
        return NoContent();
    }
}
