using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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

    public async Task<(IEnumerable<BookDto>, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters,bool trackChanges)
    {
        var booksWithMetaData = await _manager.BookRepository.GetAllBookAsync(bookParameters, trackChanges);
        var booksDtos = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
        return (booksDtos, booksWithMetaData.MetaData);
    }

    public async Task<BookDto> GetOneBookAsync(int id, bool trackChanges)
    {
        var book = await CheckExists(id, trackChanges);
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
        var entity = await CheckExists(id, trackChanges);
        // entity.Title = book.Title;
        // entity.Price = book.Price;
        entity = _mapper.Map<Book>(bookDto);
        _manager.BookRepository.Update(entity);
        await _manager.SaveAsync();
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var entity = await CheckExists(id, trackChanges);
        _manager.BookRepository.DeleteOneBook(entity);
        await _manager.SaveAsync();
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await CheckExists(id, trackChanges);
        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }

    private async Task<Book> CheckExists(int id, bool trackChanges)
    {
        var entity =await _manager.BookRepository.GetOneBookAsync(id, trackChanges);
        if (entity is null)
            throw new BookNotFound(id);
        return entity;
    }
}