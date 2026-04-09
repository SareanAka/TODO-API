using TODO_API.Models;

namespace TODO_API.Services;

public class TodoService : ITodoService
{
    private static readonly List<TodoItem> _todos = [];
    private static int _nextId = 1;
    private static readonly object _lock = new();

    public IReadOnlyList<TodoItem> GetAll()
    {
        lock (_lock)
        {
            return _todos.AsReadOnly();
        }
    }

    public TodoItem? GetById(int id)
    {
        lock (_lock)
        {
            return _todos.Find(t => t.Id == id);
        }
    }

    public TodoItem Create(string title, DateTime? dueDate)
    {
        // Default due date to 7 days from now if not provided
        var due = dueDate ?? DateTime.Now.AddDays(7);

        lock (_lock)
        {
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
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var item = _todos.Find(t => t.Id == id);
            if (item is null)
            {
                return false;
            }

            _todos.Remove(item);
            return true;
        }
    }

    public TodoItem? Update(int id, string? title, bool? isCompleted, DateTime? dueDate)
    {
        lock (_lock)
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
}
