namespace Repositories.Contracts;

public interface IRepositoryManager
{
    IBookRepository BookRepository { get; set; }
    void Save();
}