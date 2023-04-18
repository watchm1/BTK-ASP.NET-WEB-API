using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WebApi.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        // configuration builder
        // dbcontextoptions builder
        var congiguration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();

        var builder =
            new DbContextOptionsBuilder<RepositoryContext>().UseMySql(
                congiguration.GetConnectionString("SqlConnection") ?? string.Empty,
                new MySqlServerVersion(new Version(8,0,32)),
                prj => prj.MigrationsAssembly("WebApi"));
        return new RepositoryContext(builder.Options);
    }
}