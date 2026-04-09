using TODO_API.Models;

namespace TODO_API.Services;

public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task<TodoItem> CreateAsync(string title, DateTime? dueDate);
    Task<bool> DeleteAsync(int id);
    Task<TodoItem?> UpdateAsync(int id, string? title, bool? isCompleted, DateTime? dueDate);
}
