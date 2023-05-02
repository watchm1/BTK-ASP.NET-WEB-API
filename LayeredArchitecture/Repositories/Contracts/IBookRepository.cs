using Entities.Models;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<IEnumerable<Book>> GetAllBookAsync(bool trackChanges);
    Task<Book> GetOneBookAsync(int id, bool trackChanges);
    void CreateOneBook(Book book);
    void UpdateOneBook(Book book);
    void DeleteOneBook(Book book);
}