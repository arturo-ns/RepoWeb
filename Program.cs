using prueba.bounded_context.Application.Internal.CommandServices;
using prueba.bounded_context.Application.Internal.QueryServices;
using prueba.bounded_context.Domain.Repositories;
using prueba.bounded_context.Domain.Services;
using prueba.bounded_context.Infrastructure.Persistence.EFC.Repositories;
using prueba.Shared.Domain.Repositories;
using prueba.Shared.Infrastructure.Interfaces.ASP.Configuration;
using prueba.Shared.Infrastructure.Persistence.EFC.Configuration;
using prueba.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Controllers + kebab case
builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// --- Swagger limpio ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ----------------------

// --- Database Context ---
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        if (cs is null)
            throw new Exception("Database connection string is not set.");

        options.UseMySQL(cs)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var csTemplate = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(csTemplate))
            throw new Exception("Database connection string template is not set.");

        var cs = Environment.ExpandEnvironmentVariables(csTemplate);

        options.UseMySQL(cs)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });
}

// --- Dependency Injection ---
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Bills bounded context
builder.Services.AddScoped<bounded_context.Domain.Repositories.IBillRepository, bounded_context.Infrastructure.Persistence.EFC.Repositories.BillRepository>();
builder.Services.AddScoped<bounded_context.Domain.Services.IBillCommandService, bounded_context.Application.Internal.CommandServices.BillCommandService>();
builder.Services.AddScoped<bounded_context.Domain.Services.IBillQueryService, bounded_context.Application.Internal.QueryServices.BillQueryService>();

var app = builder.Build();

// Ensure database exists
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.EnsureCreated();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
