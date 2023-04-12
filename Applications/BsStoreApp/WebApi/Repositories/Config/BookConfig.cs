using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repositories.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book { Id = 1, Title = "Karagöz ve hacivat", Price = 100},
            new Book { Id = 2, Title = "Zengin baba yoksul baba", Price = 110},
            new Book { Id = 3, Title = "Harry Potter", Price = 150}
        );
    }
}
