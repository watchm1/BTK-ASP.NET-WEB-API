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

    public async Task<IEnumerable<BookDto>> GetAllBookAsync(bool trackChanges)
    {
        var books = await _manager.BookRepository.GetAllBookAsync(trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> GetOneBookAsync(int id, bool trackChanges)
    {
        var book = await _manager.BookRepository.GetOneBookAsync(id, trackChanges); 
        if (book == null)
            throw new BookNotFound(id);
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book)
    {
        _manager.BookRepository.CreateOneBook(_mapper.Map<Book>(book));
        await _manager.SaveAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = await _manager.BookRepository.GetOneBookAsync(id, trackChanges);
        if (entity is null)
            throw new BookNotFound(id);
        // entity.Title = book.Title;
        // entity.Price = book.Price;
        entity = _mapper.Map<Book>(bookDto);
        _manager.BookRepository.Update(entity);
        await _manager.SaveAsync();
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var entity =await _manager.BookRepository.GetOneBookAsync(id, trackChanges);
        if (entity is null)
            throw new BookNotFound(id);
        _manager.BookRepository.DeleteOneBook(entity);
        await _manager.SaveAsync();
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await _manager.BookRepository.GetOneBookAsync(id, trackChanges);
        if (book is null)
            throw new BookNotFound(id);
        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }
}