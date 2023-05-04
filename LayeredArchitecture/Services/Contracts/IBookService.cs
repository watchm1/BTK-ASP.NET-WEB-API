using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts;

public interface IBookService
{
    Task<(IEnumerable<BookDto>, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
    Task<BookDto> GetOneBookAsync(int id, bool trackChanges);
    Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);
    Task UpdateOneBookAsync(int id, BookDtoForUpdate book, bool trackChanges);
    Task DeleteOneBookAsync(int id, bool trackChanges);
    Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
    Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
}