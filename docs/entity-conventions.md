# Конвенция для доменных сущностей (Library.Domain)

Базовый шаблон, по которому строится большинство (~90%) сущностей в `Library.Domain/Entities`.

## Структура файла

1. Наследование от `BaseEntity` (или `BaseEntity<T>`, если нужен нестандартный тип ключа) из `Library.Domain.Entities.Basic`.
2. `#region Properties` — публичные свойства с `private set`.
3. `#region Constructors` — приватный пустой конструктор (для EF Core) + приватный конструктор с параметрами, без валидации (валидация — не забота сущности).
4. `#region Public methods` — по одному публичному сеттеру на каждое свойство (`SetX`), метод `Update(...)`, вызывающий все сеттеры, плюс доменные методы поведения (например `IncreaseStock`/`DecreaseStock`).
5. `#region Factory method` — статический `Create(...)`, вызывающий приватный конструктор. Без валидации.

## Пример — `Book`

```csharp
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
    #endregion

    #region Factory method
    public static Book Create(string title, string author, string isbn, int publicationYear, decimal price, int stockQuantity)
        => new Book(title, author, isbn, publicationYear, price, stockQuantity);
    #endregion
}
```

## Правила

- Валидация не входит в сущность (ни в конструктор, ни в `Create`) — она обрабатывается на другом уровне.
- `Id` берётся из `BaseEntity`/`BaseEntity<T>`, вручную в конструкторе не проставляется, если генерируется базой данных.
- Отклонения от шаблона (доп. поведенческие методы, другой тип ключа и т.д.) — по необходимости, это оставшиеся ~10% случаев.
