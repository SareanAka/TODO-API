using System.ComponentModel.DataAnnotations;

namespace TODO_API.Models;

public class UpdateTodoRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueDateTime { get; set; }
}
