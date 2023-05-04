using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters, bool trackChanges);
    Task<Book> GetOneBookAsync(int id, bool trackChanges);
    void CreateOneBook(Book book);
    void UpdateOneBook(Book book);
    void DeleteOneBook(Book book);
}