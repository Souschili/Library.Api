# Roadmap / План разработки

Статус на 2026-09-09. Ссылки: [entity-conventions.md](entity-conventions.md) — конвенция для доменных сущностей.

## Сделано

- Слои проекта: `Domain / Application / Infrastructure / Api` (Clean Architecture).
- Базовые типы сущностей:
  - `Library.Domain/Interfaces/IEntity<T>` — контракт `Id`.
  - `Library.Domain/Entities/Basic/BaseEntity<T>` / `BaseEntity` (= `BaseEntity<int>`) — реализация `Id`, с защищёнными конструкторами `BaseEntity()` (Id из БД) и `BaseEntity(T id)` (Id из приложения).
  - `Library.Domain/Interfaces/IAuditable` — `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`.
  - `Library.Domain/Interfaces/ISoftDeletable` — `IsDeleted`, `DeletedAt`, `DeletedBy`.
  - Интерфейсы вынесены в корень слоя, `Library.Domain/Interfaces` (namespace `Library.Domain.Interfaces`), отдельно от классов-реализаций в `Entities/Basic`.
- `<Nullable>disable</Nullable>` временно выставлен в `Library.Domain.csproj` (nullable warnings отключены только в Domain).
- Композитные интерфейсы (`IAuditableEntity<T>`, `ISoftDeletableEntity<T>`, `IAuditableSoftDeletableEntity<T>`) удалены как избыточные — композиция "Id + аудит/софт-делит" будет через наследование в базовых классах, а не через отдельные интерфейсы.
- Сущность `Book` (`Library.Domain/Entities/Book.cs`) — наследуется от `BaseEntity`, без аудита/софт-делита пока.
- Решение: `IAuditable`/`ISoftDeletable` остаются с публичным `set` (в отличие от `Id`, где `set` — `protected`). Причина: значения планируется проставлять снаружи — через EF Core interceptor или аналог, без доступа к внутренностям сущности. Если позже понадобится инкапсуляция через `SetX`-методы — тогда сеттер интерфейса станет либо не нужен (метод работает напрямую с backing-полем), либо `protected` в реализации.
- Сверка с реальным кодом MMS (`MMS.Domain/Entities/Base/{BaseEntity,AuditableEntity}.cs`, `MMS.Domain/Interfaces/{IEntity,ICreatable,IAuditable,ISoftDelete}.cs`, `SoftDeleteQueryExtension.cs`) — подтвердила паттерн "маркер-интерфейс + автообнаружение сущностей в interceptor/query filter", тот же принцип, что мы закладываем. Отличие: у MMS мутация полей аудита идёт через `protected`-методы (`SetCreatedBy`, `MarkAsDeleted` и т.д.), вызываемые явно из доменных методов сущности с `userId` от Application-слоя — у нас проще, через публичный `set` прямо из interceptor, без явных доменных методов на каждой сущности. Осознанный выбор в пользу меньшего boilerplate; MMS-подход более строгий, но требует `User`-сущности и явного шага в каждом хендлере.
- Замечено (не копируем): `BaseEntity<TId>` у MMS использует EF Core атрибуты `[Key]`/`[DatabaseGenerated]` прямо в Domain-слое — нарушает Clean Architecture. У нас Domain остаётся чистым от EF Core.
- Базовые классы аудита/софт-делита в `Library.Domain/Entities/Basic` — реализованы:
  - `AuditableEntity<T> : BaseEntity<T>, IAuditable` (+ non-generic `AuditableEntity`).
  - `SoftDeletableEntity<T> : BaseEntity<T>, ISoftDeletable` (+ non-generic `SoftDeletableEntity`).
  - `AuditableSoftDeletableEntity<T> : BaseEntity<T>, IAuditable, ISoftDeletable` (+ non-generic `AuditableSoftDeletableEntity`).
  - Паттерн — как у `BaseEntity`: non-generic наследует от `<int>`-версии, конструкторы `protected` (пустой + с `id`, вызывающий `base(id)`). Сборка `Library.Domain` проходит без ошибок (только уже существующие warning CS8632 по nullable-аннотациям, не регрессия).

## В плане

### Аудит и мягкое удаление

1. ~~Базовые классы в `Library.Domain/Entities/Basic`~~ — сделано (см. выше).
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

## Точка остановки (2026-09-09)

Базовые классы аудита/софт-делита написаны и собираются (см. "Сделано"). Следующий конкретный шаг — пункт 2 плана: решить, нужен ли `Book` аудит/софт-делит, и перевести его на `AuditableEntity`/`SoftDeletableEntity`/`AuditableSoftDeletableEntity` при необходимости. Ничего не блокирует — просто ещё не сделано.
