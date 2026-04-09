using TODO_API.Models;
using TODO_API.Data;

namespace TODO_API.Services;

public class TodoService : ITodoService
{
    public readonly TodoDbContext _db;

    public TodoService(TodoDbContext db)
    {
        _db = db;
    }

    public IReadOnlyList<TodoItem> GetAll() => _db.TodoItems.OrderBy(t => t.Id).ToList();

    public TodoItem? GetById(int id) => _db.TodoItems.FirstOrDefault(t => t.Id == id);

    public TodoItem Create(string title, DateTime? dueDate)
    {
        var item = new TodoItem
        {
            Title = title,
            IsCompleted = false,
            DueDateTime = dueDate ?? DateTime.Now.AddDays(7)
        };
        _db.TodoItems.Add(item);
        _db.SaveChanges();
        return item;
    }

    public bool Delete(int id)
    {
        var item = _db.TodoItems.Find(id);
        if (item is null) return false;

        _db.TodoItems.Remove(item);
        _db.SaveChanges();
        return true;
    }

    public TodoItem? Update(int id, string? title, bool? isCompleted, DateTime? dueDate)
    {
        var item = _db.TodoItems.Find(id);
        if (item is null) return null;

        if (title is not null) item.Title = title;
        if (isCompleted.HasValue) item.IsCompleted = isCompleted.Value; 
        if (dueDate is not null) item.DueDateTime = dueDate.Value; 

        _db.SaveChanges();
        return item;
    }
}
