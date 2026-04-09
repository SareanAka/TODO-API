namespace TODO_API.Models;

public class UpdateTodoRequest
{
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueDateTime { get; set; }
}
