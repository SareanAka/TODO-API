using System.ComponentModel.DataAnnotations;

namespace TODO_API.Models;

public class CreateTodoRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDateTime { get; set; }
}
