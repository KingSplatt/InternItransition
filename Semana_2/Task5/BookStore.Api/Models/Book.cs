using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Models;

public class Book
{
    public int Index { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public List<Review> Reviews { get; set; } = new();
    public string CoverImageUrl { get; set; } = string.Empty;
    public int Likes { get; set; }
}

public class Review
{
    public string Text { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; }
}

public class BookRequest
{
    public string Locale { get; set; } = "en";
    public int Seed { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public double Likes { get; set; }
    public double ReviewCount { get; set; }
}

public class BookResponse
{
    public List<Book> Books { get; set; } = new();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNextPage { get; set; }
}
