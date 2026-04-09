using TODO_API.Models;

namespace TODO_API.Services;

public interface ITodoService
{
    IReadOnlyList<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem Create(string title, DateTime? dueDate);
    bool Delete(int id);
    TodoItem? Update(int id, string? title, bool? isCompleted, DateTime? dueDate);
}
