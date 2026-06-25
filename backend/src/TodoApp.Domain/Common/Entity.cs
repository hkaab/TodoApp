namespace TodoApp.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset CreatedAtUtc { get; protected set; }
    public DateTimeOffset? UpdatedAtUtc { get; protected set; }
}
