using Microsoft.EntityFrameworkCore;
using TODO_API.Services;
using TODO_API.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DbContext (SQL server)
var cs = builder.Configuration.GetConnectionString("TodoDb") ?? throw new InvalidOperationException("Connection string 'TodoDb' not found");

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(cs));

builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
