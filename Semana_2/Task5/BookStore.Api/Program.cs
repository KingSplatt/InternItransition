using BookStore.Api.Models;
using BookStore.Api.Services;
using CsvHelper;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<BookService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();

app.MapGet("/api/books", (
    BookService bookService,
    string locale = "en",
    int seed = 12345,
    int page = 1,
    int pageSize = 20,
    double likes = 0,
    double reviewCount = 0) =>
{
    var request = new BookRequest
    {
        Locale = locale,
        Seed = seed,
        Page = page,
        PageSize = pageSize,
        Likes = likes,
        ReviewCount = reviewCount
    };
    
    return bookService.GenerateBooks(request);
})
.WithName("GetBooks")
.WithOpenApi();

app.MapGet("/api/books/export", (
    BookService bookService,
    string locale = "en",
    int seed = 12345,
    int pages = 1,
    double likes = 0,
    double reviewCount = 0) =>
{
    var allBooks = new List<Book>();
    
    for (int page = 1; page <= pages; page++)
    {
        var request = new BookRequest
        {
            Locale = locale,
            Seed = seed,
            Page = page,
            PageSize = 20,
            Likes = likes,
            ReviewCount = reviewCount
        };
        
        var response = bookService.GenerateBooks(request);
        allBooks.AddRange(response.Books);
    }
    
    var csv = GenerateCSV(allBooks);
    return Results.File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "books.csv");
})
.WithName("ExportBooks")
.WithOpenApi();

app.Run();

static string GenerateCSV(List<Book> books)
{
    var csvData = books.Select(book => new
    {
        Index = book.Index,
        ISBN = book.ISBN,
        Title = book.Title,
        Author = book.Author,
        Publisher = book.Publisher,
        ReviewCount = book.Reviews.Count
    }).ToList();
    
    using var writer = new StringWriter();
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    csv.WriteRecords(csvData);
    
    return writer.ToString();
}