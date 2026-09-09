# Roadmap / План разработки

Статус на 2026-09-09. Ссылки: [entity-conventions.md](entity-conventions.md) — конвенция для доменных сущностей.

## Сделано

- Слои проекта: `Domain / Application / Infrastructure / Api` (Clean Architecture).
- Базовые типы сущностей (`Library.Domain/Entities/Basic`):
  - `Interfaces/IEntity<T>` — контракт `Id`.
  - `BaseEntity<T>` / `BaseEntity` (= `BaseEntity<int>`) — реализация `Id`.
  - `Interfaces/IAuditable` — `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`.
  - `Interfaces/ISoftDeletable` — `IsDeleted`, `DeletedAt`, `DeletedBy`.
  - Интерфейсы вынесены в подпапку `Basic/Interfaces` (namespace `Library.Domain.Entities.Basic.Interfaces`), классы остаются в `Basic`.
- Композитные интерфейсы (`IAuditableEntity<T>`, `ISoftDeletableEntity<T>`, `IAuditableSoftDeletableEntity<T>`) удалены как избыточные — композиция "Id + аудит/софт-делит" будет через наследование в базовых классах, а не через отдельные интерфейсы.
- Сущность `Book` (`Library.Domain/Entities/Book.cs`) — наследуется от `BaseEntity`, без аудита/софт-делита пока.

## В плане

### Аудит и мягкое удаление

1. Базовые классы в `Library.Domain/Entities/Basic`:
   - `AuditableEntity<T> : BaseEntity<T>, IAuditable`
   - `SoftDeletableEntity<T> : BaseEntity<T>, ISoftDeletable`
   - `AuditableSoftDeletableEntity<T> : BaseEntity<T>, IAuditable, ISoftDeletable`
   - Каждый — с non-generic вариантом (`AuditableEntity : AuditableEntity<int>` и т.д.), по аналогии с `BaseEntity`.
2. `Book` — решить, нужен ли ему аудит/софт-делит, и перевести на соответствующий базовый класс.
3. Инфраструктура (`Library.Infrastructure`, когда появится EF Core):
   - `SaveChangesInterceptor`, который проставляет `CreatedAt/CreatedBy` при добавлении и `UpdatedAt/UpdatedBy` при изменении сущностей, реализующих `IAuditable`.
   - `SaveChangesInterceptor` или переопределение `Remove`, конвертирующее удаление сущностей `ISoftDeletable` в soft-delete (`IsDeleted = true`, `DeletedAt`, `DeletedBy`) вместо физического удаления.
   - Global Query Filter в `OnModelCreating` — автообнаружение по `ISoftDeletable` для всех сущностей, скрывающее `IsDeleted == true` из обычных запросов.
   - `CreatedBy`/`UpdatedBy`/`DeletedBy` — источник значения (текущий пользователь) определить, когда появится аутентификация/`User`.

### Сущности домена

- `Book` — реализован (см. выше).
- Остальные сущности (выдача книг, читатели/клиенты, продажи) — состав и модель ещё не определены, требуют отдельного обсуждения.

## Открытые вопросы

- Нужен ли `CreatedBy`/`UpdatedBy`/`DeletedBy` как `string?` (текущий вариант) или как ссылка на будущую сущность `User` (`Guid?`/`int?` + FK)?
- Формат хранения времени — `DateTime` (текущий) vs `DateTimeOffset`/явный UTC-суффикс в имени свойств.
