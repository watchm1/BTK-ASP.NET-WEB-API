using System.Reflection;
using System.Text;
using Entities.Models;
using System.Linq.Dynamic.Core;

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

    public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return books.OrderBy(b => b.Id);
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<bool>(orderByQueryString);
        if (orderQuery is null)
            return books.OrderBy(b => b.Id);
        return books.OrderBy(orderQuery);
        return books;
    }
}