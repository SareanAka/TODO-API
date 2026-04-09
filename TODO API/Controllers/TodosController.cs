using Microsoft.AspNetCore.Mvc;
using TODO_API.Models;
using TODO_API.Services;

namespace TODO_API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ITodoService todoService, ILogger<TodosController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<TodoItem>> GetAll()
    {
        _logger.LogInformation("Retrieving all todo items");
        var todos = _todoService.GetAll();
        _logger.LogInformation("Retrieved {Count} todo items", todos.Count);
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> GetById(int id)
    {
        _logger.LogInformation("Retrieving todo item with id {Id}", id);
        var item = _todoService.GetById(id);

        if (item is null)
        {
            _logger.LogWarning("Todo item with id {Id} not found", id);
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public ActionResult<TodoItem> Create([FromBody] CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            _logger.LogWarning("Create todo failed: title is required");
            return BadRequest(new { error = "Title is required." });
        }

        _logger.LogInformation("Creating todo item with title '{Title}'", request.Title);
        var item = _todoService.Create(request.Title, request.DueDateTime);
        _logger.LogInformation("Created todo item with id {Id}", item.Id);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting todo item with id {Id}", id);
        var deleted = _todoService.Delete(id);

        if (!deleted)
        {
            _logger.LogWarning("Delete failed: todo item with id {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Deleted todo item with id {Id}", id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult<TodoItem> Update(int id, [FromBody] UpdateTodoRequest request)
    {
        if (request.Title is null && request.IsCompleted is null)
        {
            _logger.LogWarning("Update todo failed: no fields provided for id {Id}", id);
            return BadRequest(new { error = "At least one field (title or isCompleted) must be provided." });
        }

        _logger.LogInformation("Updating todo item with id {Id}", id);
        var item = _todoService.Update(id, request.Title, request.IsCompleted, request.DueDateTime);

        if (item is null)
        {
            _logger.LogWarning("Update failed: todo item with id {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Updated todo item with id {Id}", id);
        return Ok(item);
    }
}
