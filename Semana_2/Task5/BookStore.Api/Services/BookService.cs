using Bogus;
using BookStore.Api.Models;
using System.Text;

namespace BookStore.Api.Services;

public class BookService
{
    private readonly Dictionary<string, string> _localeMapping = new()
    {
        { "en", "en_US" },
        { "de", "de" },
        { "es", "es" }
    };

    public BookResponse GenerateBooks(BookRequest request)
    {
        var locale = _localeMapping.GetValueOrDefault(request.Locale, "en_US");
        var combinedSeed = request.Seed + (request.Page * 1000);
        var faker = new Faker<Book>(locale)
            .UseSeed(combinedSeed)
            .RuleFor(b => b.Index, (f, b) => ((request.Page - 1) * request.PageSize) + f.IndexFaker + 1)
            .RuleFor(b => b.ISBN, f => GenerateISBN(f))
            .RuleFor(b => b.Title, f => GenerateTitle(f, locale))
            .RuleFor(b => b.Author, f => GenerateAuthor(f, locale))
            .RuleFor(b => b.Publisher, f => GeneratePublisher(f, locale))
            .RuleFor(b => b.CoverImageUrl, (f, b) => GenerateCoverImage(b.Title, b.Author))
            .RuleFor(b => b.Reviews, (f, b) => GenerateReviews(f, request.ReviewCount, locale, request.Likes));
        var books = faker.Generate(request.PageSize);
        return new BookResponse
        {
            Books = books,
            CurrentPage = request.Page,
            HasNextPage = true, 
            TotalPages = int.MaxValue
        };
    }

    private string GenerateISBN(Faker faker)
    {
        var prefix = "978";
        var group = faker.Random.Int(0, 9);
        var publisher = faker.Random.Int(10000, 99999);
        var title = faker.Random.Int(100, 999);
        var isbn12 = $"{prefix}{group}{publisher:D5}{title:D3}";
        var checkDigit = CalculateISBNCheckDigit(isbn12);
        var ISBN = $"{prefix}-{group}-{publisher:D5}-{title:D3}-{checkDigit}";
        return ISBN;
    }

    private int CalculateISBNCheckDigit(string isbn12)
    {
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(isbn12[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }
        return (10 - (sum % 10)) % 10;
    }

    private string GenerateTitle(Faker faker, string locale)
    {
        return locale switch
        {
            "de" => GenerateGermanTitle(faker),
            "es" => GenerateSpanishTitle(faker),
            _ => GenerateEnglishTitle(faker)
        };
    }

    private string GenerateEnglishTitle(Faker faker)
    {
        var templates = new[]
        {
            "{adjective} {noun}",
            "The {noun} of {noun}",
            "{adjective} {noun}: A {genre} Novel",
            "The {adjective} {noun}",
            "{noun} in the {place}",
            "Beyond {noun}",
            "The Last {noun}",
            "Chronicles of {noun}"
        };

        var adjectives = new[] { "Silent", "Hidden", "Lost", "Forgotten", "Ancient", "Secret", "Mysterious", "Golden", "Dark", "Bright" };
        var nouns = new[] { "Journey", "Mystery", "Adventure", "Secret", "Dream", "Shadow", "Light", "Story", "Tale", "Legend" };
        var genres = new[] { "Mystery", "Romance", "Thriller", "Fantasy", "Historical", "Science Fiction" };
        var places = new[] { "Mountains", "Forest", "City", "Desert", "Ocean", "Valley", "Castle", "Garden" };

        var template = faker.PickRandom(templates);
        
        return template
            .Replace("{adjective}", faker.PickRandom(adjectives))
            .Replace("{noun}", faker.PickRandom(nouns))
            .Replace("{genre}", faker.PickRandom(genres))
            .Replace("{place}", faker.PickRandom(places));
    }

    private string GenerateGermanTitle(Faker faker)
    {
        var templates = new[]
        {
            "Das {adjective} {noun}",
            "Die {noun} des {noun2}",
            "{adjective} {noun}: Ein {genre} Roman",
            "Der {adjective} {noun}",
            "{noun} im {place}",
            "Jenseits der {noun}",
            "Das letzte {noun}",
            "Chroniken des {noun2}"
        };

        var adjectives = new[] { "geheimnisvolle", "verborgene", "verlorene", "vergessene", "alte", "geheime", "mysteriöse", "goldene", "dunkle", "helle" };
        var nouns = new[] { "Reise", "Mysterium", "Abenteuer", "Geheimnis", "Traum", "Schatten", "Licht", "Geschichte", "Erzählung", "Legende" };
        var genres = new[] { "Krimi", "Romantik", "Thriller", "Fantasy", "Historischer", "Science-Fiction" };
        var places = new[] { "Bergen", "Wald", "Stadt", "Wüste", "Ozean", "Tal", "Schloss", "Garten" };

        var template = faker.PickRandom(templates);
        
        return template
            .Replace("{adjective}", faker.PickRandom(adjectives))
            .Replace("{noun}", faker.PickRandom(nouns))
            .Replace("{noun2}", faker.PickRandom(nouns))
            .Replace("{genre}", faker.PickRandom(genres))
            .Replace("{place}", faker.PickRandom(places));
    }

    private string GenerateSpanishTitle(Faker faker)
    {
        var templates = new[]
        {
            "{adjective} {noun}",
            "El {noun} de {noun2}",
            "{adjective} {noun}: Una novela de {genre}",
            "El {adjective} {noun}",
            "{noun} en el {place}",
            "Más allá del {noun}",
            "El último {noun}",
            "Crónicas de {noun2}"
        };

        var adjectives = new[] { "silencioso", "oculto", "perdido", "olvidado", "antiguo", "secreto", "misterioso", "dorado", "oscuro", "brillante" };
        var nouns = new[] { "viaje", "misterio", "aventura", "secreto", "sueño", "sombra", "luz", "historia", "relato", "leyenda" };
        var genres = new[] { "misterio", "romance", "thriller", "fantasía", "histórico", "ciencia ficción" };
        var places = new[] { "montañas", "bosque", "ciudad", "desierto", "océano", "valle", "castillo", "jardín" };

        var template = faker.PickRandom(templates);
        
        return template
            .Replace("{adjective}", faker.PickRandom(adjectives))
            .Replace("{noun}", faker.PickRandom(nouns))
            .Replace("{noun2}", faker.PickRandom(nouns))
            .Replace("{genre}", faker.PickRandom(genres))
            .Replace("{place}", faker.PickRandom(places));
    }

    private string GenerateAuthor(Faker faker, string locale)
    {
        return locale switch
        {
            "de" => $"{faker.Name.FirstName()} {faker.Name.LastName()}",
            "es" => $"{faker.Name.FirstName()} {faker.Name.LastName()}",
            _ => $"{faker.Name.FirstName()} {faker.Name.LastName()}"
        };
    }

    private string GeneratePublisher(Faker faker, string locale)
    {
        return locale switch
        {
            "de" => faker.PickRandom(new[] { "Penguin Random House", "Holtzbrinck", "Weltbild", "Bastei Lübbe", "Carlsen Verlag", "Fischer Verlag", "Ullstein Buchverlage", "Rowohlt Verlag" }),
            "es" => faker.PickRandom(new[] { "Penguin Random House", "Planeta", "Grupo Anaya", "Ediciones SM", "Editorial Espasa", "Alfaguara", "Ediciones Akal", "Editorial Planeta" }),
            _ => faker.PickRandom(new[] { "Penguin Random House", "HarperCollins", "Macmillan", "Simon & Schuster", "Hachette Book Group", "Scholastic", "Wiley", "Pearson Education" })
        };
    }

    private string GenerateCoverImage(string title, string author)
    {
        var seed = Math.Abs((title + author).GetHashCode()) % 1000;
        var imageUrl = $"https://picsum.photos/seed/{seed}/300/400";
        return imageUrl;
    }

    private int GenerateLikes(Faker faker, double likes)
    {
        var fixedLikes = (int)Math.Floor(likes);
        var fractionalPart = likes - fixedLikes;
        var hasExtraLike = faker.Random.Double() < fractionalPart;
        return fixedLikes + (hasExtraLike ? 1 : 0);
    }

    private List<Review> GenerateReviews(Faker faker, double reviewCount, string locale, double likesInfluence)
    {
        var reviews = new List<Review>();
        var IntReviews = (int)Math.Floor(reviewCount);
        var FracReviews = reviewCount - IntReviews;
        var hasExtraReview = faker.Random.Double() < FracReviews;
        var totalReviews = IntReviews + (hasExtraReview ? 1 : 0);
        var minRating = Math.Max(1, (int)Math.Floor(likesInfluence));
        var maxRating = 5;
        for (int i = 0; i < totalReviews; i++)
        {
            reviews.Add(new Review
            {
                Text = GenerateReviewText(faker, locale),
                ReviewerName = GenerateReviewerName(faker, locale),
                Rating = faker.Random.Int(minRating, maxRating)
            });
        }
        return reviews;
    }

    private string GenerateReviewText(Faker faker, string locale)
    {
        return locale switch
        {
            "de" => faker.PickRandom(new[]
            {
                "Ein außergewöhnliches Buch, das mich von der ersten Seite an gefesselt hat.",
                "Faszinierende Charaktere und eine packende Handlung.",
                "Sehr empfehlenswert für alle Liebhaber dieses Genres.",
                "Ein Meisterwerk, das noch lange in Erinnerung bleiben wird.",
                "Brillant geschrieben und äußerst unterhaltsam."
            }),
            "es" => faker.PickRandom(new[]
            {
                "Un libro excepcional que me cautivó desde la primera página.",
                "Personajes fascinantes y una trama envolvente.",
                "Altamente recomendado para todos los amantes de este género.",
                "Una obra maestra que será recordada por mucho tiempo.",
                "Brillantemente escrito y extremadamente entretenido."
            }),
            _ => faker.PickRandom(new[]
            {
                "An exceptional book that captivated me from the first page.",
                "Fascinating characters and a gripping plot.",
                "Highly recommended for all lovers of this genre.",
                "A masterpiece that will be remembered for a long time.",
                "Brilliantly written and extremely entertaining."
            })
        };
    }

    private string GenerateReviewerName(Faker faker, string locale)
    {
        return locale switch
        {
            "de" => $"{faker.Name.FirstName()} {faker.Name.LastName()[0]}.",
            "es" => $"{faker.Name.FirstName()} {faker.Name.LastName()[0]}.",
            _ => $"{faker.Name.FirstName()} {faker.Name.LastName()[0]}."
        };
    }
}
