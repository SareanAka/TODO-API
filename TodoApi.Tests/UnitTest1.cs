using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TODO_API.Data;
using TODO_API.Services;

namespace TodoApi.Tests;

public class TodoServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly TodoDbContext _db;
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new TodoDbContext(options);
        _db.Database.EnsureCreated();

        _service = new TodoService(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoTodosExist()
    {
        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task Create_AddsTodoAndReturnsIt()
    {
        var due = new DateTime(new DateOnly(2025, 03, 12), new TimeOnly(12, 00, 00));
        var item = await _service.CreateAsync("Meeting", due);

        Assert.Equal(1, item.Id);
        Assert.Equal("Meeting", item.Title);
        Assert.False(item.IsCompleted);
        Assert.Equal(due, item.DueDateTime);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCreatedTodos()
    {
        await _service.CreateAsync("Task 1", null);
        await _service.CreateAsync("Task 2", null);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetById_ReturnsTodo_WhenExists()
    {
        var created = await _service.CreateAsync("Test", null);

        var result = await _service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotExists()
    {
        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ReturnsTrue_WhenTodoExists()
    {
        var created = await _service.CreateAsync("To delete", null);

        var result = await _service.DeleteAsync(created.Id);

        Assert.True(result);
        Assert.Empty(await _service.GetAllAsync());
    }

    [Fact]
    public async Task Delete_ReturnsFalse_WhenTodoNotExists()
    {
        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task Update_ReturnsUpdatedTodo_WhenExists()
    {
        var created = await _service.CreateAsync("Original", null);

        var result = await _service.UpdateAsync(created.Id, "Updated", true, null);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.True(result.IsCompleted);
    }

    [Fact]
    public async Task Update_ReturnsNull_WhenNotExists()
    {
        var result = await _service.UpdateAsync(999, "Nope", null, null);

        Assert.Null(result);
    }

    [Fact]
    public async Task Update_OnlyUpdatesProvidedFields()
    {
        var created = await _service.CreateAsync("Original", null);

        var result = await _service.UpdateAsync(created.Id, null, true, null);

        Assert.NotNull(result);
        Assert.Equal("Original", result.Title);
        Assert.True(result.IsCompleted);
    }

    [Fact]
    public async Task Create_AssignsIncrementingIds()
    {
        var first = await _service.CreateAsync("First", null);
        var second = await _service.CreateAsync("Second", null);

        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }
}
