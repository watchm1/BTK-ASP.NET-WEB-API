using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;
    public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<BookDto> GetAllBook(bool trackChanges)
    {
        var books = _manager.BookRepository.GetAllBooks(trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public BookDto? GetOneBook(int id, bool trackChanges)
    {
        var book = _manager.BookRepository.GetOneBook(id, trackChanges); 
        if (book == null)
            throw new BookNotFound(id);
        return _mapper.Map<BookDto>(book);
    }

    public BookDto CreateOneBook(BookDtoForInsertion book)
    {
        _manager.BookRepository.CreateOneBook(_mapper.Map<Book>(book));
        _manager.Save();
        return _mapper.Map<BookDto>(book);
    }

    public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
            throw new BookNotFound(id);
        // entity.Title = book.Title;
        // entity.Price = book.Price;
        entity = _mapper.Map<Book>(bookDto);
        _manager.BookRepository.Update(entity);
        _manager.Save();
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (entity is null)
            throw new BookNotFound(id);
        _manager.BookRepository.DeleteOneBook(entity);
        _manager.Save();
    }

    public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
    {
        var book = _manager.BookRepository.GetOneBook(id, trackChanges);
        if (book is null)
            throw new BookNotFound(id);
        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
        return (bookDtoForUpdate, book);
    }

    public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        _manager.Save();
    }
}