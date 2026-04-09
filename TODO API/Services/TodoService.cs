using TODO_API.Models;

namespace TODO_API.Services;

public class TodoService : ITodoService
{
    private readonly List<TodoItem> _todos = [];
    private int _nextId = 1;

    public IReadOnlyList<TodoItem> GetAll()
    {
        return _todos.AsReadOnly();
    }

    public TodoItem? GetById(int id)
    {
        return _todos.Find(t => t.Id == id);
    }

    public TodoItem Create(string title, DateTime? dueDate)
    {
        // Default due date to 7 days from now if not provided
        var due = dueDate ?? DateTime.Now.AddDays(7);

        var item = new TodoItem
        {
            Id = _nextId++,
            Title = title,
            IsCompleted = false,
            DueDateTime = due
        };

        _todos.Add(item);
        return item;
    }

    public bool Delete(int id)
    {
        var item = _todos.Find(t => t.Id == id);
        if (item is null)
        {
            return false;
        }

        _todos.Remove(item);
        return true;
    }

    public TodoItem? Update(int id, string? title, bool? isCompleted, DateTime? dueDate)
    {
        var item = _todos.Find(t => t.Id == id);
        if (item is null)
        {
            return null;
        }

        if (title is not null)
        {
            item.Title = title;
        }

        if (isCompleted.HasValue)
        {
            item.IsCompleted = isCompleted.Value;
        }

        if (dueDate is not null)
        {
            item.DueDateTime = dueDate.Value;
        }

        return item;
    }
}
