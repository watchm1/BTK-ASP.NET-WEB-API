using Entities.Models;

namespace Repositories.EFCore.Extensions;

public static class BooksRepositoryExtensions
{
    public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
    {
        return books.Where(book => (book.Price >= minPrice) && book.Price <= maxPrice);
    }

    public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
    {
        if(string.IsNullOrWhiteSpace(searchTerm))
            return books;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return books.Where(b => b.Title.ToLower().Contains(lowerCaseTerm));
    }
}