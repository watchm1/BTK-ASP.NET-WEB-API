using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;

    public BookManager(IRepositoryManager manager)
    {
        _manager = manager;
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
        if (book is null)
            throw new ArgumentNullException(nameof(book));
        _manager.BookRepository.CreateOneBook(book);
        _manager.Save();
        return book;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
            throw new Exception($"Book with id:{id} could not found.");
        if (book is null)
            throw new ArgumentNullException(nameof(book));
        entity.Title = book.Title;
        entity.Price = book.Price;
        _manager.BookRepository.Update(entity);
        _manager.Save();
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
            throw new Exception($"Book with id:{id} could not found");
        _manager.BookRepository.DeleteOneBook(entity);
        _manager.Save();
    }
}