using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;
    private readonly ILoggerService _logger;
    public BookManager(IRepositoryManager manager, ILoggerService logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IEnumerable<Book> GetAllBook(bool trackChanges)
    {
        return _manager.BookRepository.GetAllBooks(trackChanges);
    }

    public Book? GetOneBook(int id, bool trackChanges)
    {
        return _manager.BookRepository.GetOneBook(id, trackChanges);
    }

    public Book CreateOneBook(Book book)
    {
        _manager.BookRepository.CreateOneBook(book);
        _manager.Save();
        return book;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
        {
            var message = $"Book with id:{id} could not found.";
            _logger.LogInfo(message);
            throw new Exception(message);
        }
        entity.Title = book.Title;
        entity.Price = book.Price;
        _manager.BookRepository.Update(entity);
        _manager.Save();
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
        {
            var message = $"The Book with id: {id} could not found.";
            _logger.LogInfo(message);
            throw new Exception(message);
        }
        _manager.BookRepository.DeleteOneBook(entity);
        _manager.Save();
    }
}