using Library.Domain.Entities.Basic;

namespace Library.Domain.Entities;

public class Book : BaseEntity
{
    #region Properties
    public string Title { get; private set; } = null!;
    public string Author { get; private set; } = null!;
    public string Isbn { get; private set; } = null!;
    public int PublicationYear { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    #endregion

    #region Constructors
    private Book()
    {
    }

    private Book(string title, string author, string isbn, int publicationYear, decimal price, int stockQuantity)
    {
        Title = title;
        Author = author;
        Isbn = isbn;
        PublicationYear = publicationYear;
        Price = price;
        StockQuantity = stockQuantity;
    }
    #endregion

    #region Public methods
    public void SetTitle(string title)
        => Title = title;

    public void SetAuthor(string author)
        => Author = author;

    public void SetIsbn(string isbn)
        => Isbn = isbn;

    public void SetPublicationYear(int publicationYear)
        => PublicationYear = publicationYear;

    public void SetPrice(decimal price)
        => Price = price;

    public void SetStockQuantity(int stockQuantity)
        => StockQuantity = stockQuantity;

    public void Update(string title, string author, string isbn, int publicationYear, decimal price, int stockQuantity)
    {
        SetTitle(title);
        SetAuthor(author);
        SetIsbn(isbn);
        SetPublicationYear(publicationYear);
        SetPrice(price);
        SetStockQuantity(stockQuantity);
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (quantity > StockQuantity)
            throw new InvalidOperationException("Not enough stock available");

        StockQuantity -= quantity;
    }
    #endregion

    #region Factory method
    public static Book Create(string title, string author, string isbn, int publicationYear, decimal price, int stockQuantity)
        => new Book(title, author, isbn, publicationYear, price, stockQuantity);
    #endregion
}
