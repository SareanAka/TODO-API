using TODO_API.Services;

namespace TodoApi.Tests;

public class TodoServiceTests
{
    private readonly TodoService _service = new();

    [Fact]
    public void GetAll_ReturnsEmptyList_WhenNoTodosExist()
    {
        var result = _service.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void Create_AddsTodoAndReturnsIt()
    {
        var item = _service.Create("Meeting", new DateTime(new DateOnly(2025, 03, 12), new TimeOnly(12, 00, 00)));

        Assert.Equal(1, item.Id);
        Assert.Equal("Meeting", item.Title);
        Assert.False(item.IsCompleted);
        Assert.Equal(new DateTime(new DateOnly(2025, 03, 12), new TimeOnly(12, 00, 00)), item.DueDateTime);
    }

    [Fact]
    public void GetAll_ReturnsAllCreatedTodos()
    {
        _service.Create("Task 1", null);
        _service.Create("Task 2", null);

        var result = _service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetById_ReturnsTodo_WhenExists()
    {
        var created = _service.Create("Test", null);

        var result = _service.GetById(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotExists()
    {
        var result = _service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void Delete_ReturnsTrue_WhenTodoExists()
    {
        var created = _service.Create("To delete", null);

        var result = _service.Delete(created.Id);

        Assert.True(result);
        Assert.Empty(_service.GetAll());
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenTodoNotExists()
    {
        var result = _service.Delete(999);

        Assert.False(result);
    }

    [Fact]
    public void Update_ReturnsUpdatedTodo_WhenExists()
    {
        var created = _service.Create("Original", null);

        var result = _service.Update(created.Id, "Updated", true, null);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.True(result.IsCompleted);
    }

    [Fact]
    public void Update_ReturnsNull_WhenNotExists()
    {
        var result = _service.Update(999, "Nope", null, null);

        Assert.Null(result);
    }

    [Fact]
    public void Update_OnlyUpdatesProvidedFields()
    {
        var created = _service.Create("Original", null);

        var result = _service.Update(created.Id, null, true, null);

        Assert.NotNull(result);
        Assert.Equal("Original", result.Title);
        Assert.True(result.IsCompleted);
    }

    [Fact]
    public void Create_AssignsIncrementingIds()
    {
        var first = _service.Create("First", null);
        var second = _service.Create("Second", null);

        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
    }
}
