using Microsoft.EntityFrameworkCore;
using TODO_API.Services;
using TODO_API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DbContext with provider toggle and Azure SQL retry logic
var cs = builder.Configuration.GetConnectionString("TodoDb") ?? throw new InvalidOperationException("Connection string 'TodoDb' not found");
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    if (dbProvider == "Sqlite")
    {
        options.UseSqlite(cs);
    }
    else
    {
        options.UseSqlServer(cs, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    }
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<TodoDbContext>();

builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Apply pending migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "TODO API");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
