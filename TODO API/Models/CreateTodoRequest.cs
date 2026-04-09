namespace TODO_API.Models;

public class CreateTodoRequest
{
    public string? Title { get; set; }
    public DateTime? DueDateTime { get; set; }
}
