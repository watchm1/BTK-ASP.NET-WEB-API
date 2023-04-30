using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts;

public interface IBookService
{
    IEnumerable<BookDto> GetAllBook(bool trackChanges);
    BookDto? GetOneBook(int id, bool trackChanges);
    BookDto CreateOneBook(BookDtoForInsertion book);
    void UpdateOneBook(int id, BookDtoForUpdate book, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
    (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);
    void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
}