using Findash;
using Findash.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Validation stuff
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

// Database stuff
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connStr);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Dependecy Injection Pipeline já foi construída então podemos chamar coisas
// Criámos um novo scope porque não podemos associar isto ao root scope
// Um scope em ASP.NET Core representa o ciclo de vida de um Http Request
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.MigrateAndSeed(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();