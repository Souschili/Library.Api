# Library.Domain

Domain-слой в Clean Architecture. Ядро приложения — доменные сущности и их поведение, без зависимостей от других слоёв и от внешних технологий (EF Core, ASP.NET и т.д.).

## Что здесь

- Доменные сущности (`Book` и т.д.) — данные + поведение (методы вроде `IncreaseStock`/`DecreaseStock`), без валидации (валидация — забота Application-слоя).
- Базовые типы и контракты для сущностей: `Id`, аудит (`CreatedAt`/`UpdatedAt`/...), мягкое удаление (`IsDeleted`/`DeletedAt`/...).

Чего здесь **нет**: EF Core, репозитории, DI, HTTP, обработчики команд/запросов — это Infrastructure/Application/Api.

## Структура

```
Library.Domain/
└── Entities/
    ├── Book.cs                          — доменная сущность "Книга"
    └── Basic/
        ├── BaseEntity.cs                — BaseEntity<T> / BaseEntity (= BaseEntity<int>), реализация Id
        └── Interfaces/
            ├── IEntity.cs                — контракт Id<T>
            ├── IAuditable.cs             — контракт аудита (CreatedAt/UpdatedAt/CreatedBy/UpdatedBy)
            └── ISoftDeletable.cs         — контракт мягкого удаления (IsDeleted/DeletedAt/DeletedBy)
```

Интерфейсы (`Interfaces/`) отделены от классов-реализаций (`Basic/`), чтобы не смешивать контракты и готовую реализацию в одной папке.

## Конвенции

Правила построения сущностей — в [`docs/entity-conventions.md`](../docs/entity-conventions.md). План по базовым классам аудита/софт-делита и остальным сущностям домена — в [`docs/roadmap.md`](../docs/roadmap.md).
