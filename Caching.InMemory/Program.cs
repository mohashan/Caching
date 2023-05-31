using Caching.InMemory.Cache;
using Caching.InMemory.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // dotnet ef migrations add InitialCreate --output-dir Migrations
    // Commit Migration to have db on local sql server, plus some sample data
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    // dotnet ef migrations add InitialCreate --output-dir Migrations
    // Commit Migration to have db on local sql server, plus some sample data
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
