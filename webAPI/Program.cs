using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using webAPI.dal.Entities;
using webAPI.dal.Repositories;
using webAPI.dal.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

/////////////////////////////////
// Add services to the container.
/////////////////////////////////

builder.Services.AddControllers();

//CONFIG FILES
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

//SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Product API",
        Description = "An ASP.NET Core Web API for managing Product-Category items",
    });
});

//DATABASE
builder.Services.AddScoped((s) => new SqlConnection(builder.Configuration.GetConnectionString("MSSQLConnection")));
builder.Services.AddScoped<IDbTransaction>(s =>
{
    SqlConnection conn = s.GetRequiredService<SqlConnection>();
    conn.Open();
    return conn.BeginTransaction();
});

//DEPENDENCY INJECTION
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

///////////////////////////////////////
// Configure the HTTP request pipeline.
///////////////////////////////////////

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();