using Microsoft.EntityFrameworkCore;
using TODO_API.Models;
using TODO_API.Data;

namespace TODO_API.Services;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _db;

    public TodoService(TodoDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync() =>
        await _db.TodoItems.OrderBy(t => t.Id).ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(int id) =>
        await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

    public async Task<TodoItem> CreateAsync(string title, DateTime? dueDate)
    {
        var item = new TodoItem
        {
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            DueDateTime = dueDate ?? DateTime.UtcNow.AddDays(7)
        };
        _db.TodoItems.Add(item);
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _db.TodoItems.FindAsync(id);
        if (item is null) return false;

        _db.TodoItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<TodoItem?> UpdateAsync(int id, string? title, bool? isCompleted, DateTime? dueDate)
    {
        var item = await _db.TodoItems.FindAsync(id);
        if (item is null) return null;

        if (title is not null) item.Title = title;
        if (isCompleted.HasValue) item.IsCompleted = isCompleted.Value;
        if (dueDate is not null) item.DueDateTime = dueDate.Value;

        await _db.SaveChangesAsync();
        return item;
    }
}
