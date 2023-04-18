using Entities.Models;

namespace Services.Contracts;

public interface IBookService
{
    IEnumerable<Book> GetAllBook(bool trackChanges);
    Book? GetOneBook(int id, bool trackChanges);
    Book CreateOneBook(Book book);
    void UpdateOneBook(int id, Book book, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
}