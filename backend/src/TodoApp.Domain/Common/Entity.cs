using System.Diagnostics.CodeAnalysis;

namespace TodoApp.Domain.Common;

[ExcludeFromCodeCoverage]
public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset CreatedAtUtc { get; protected set; }
    public DateTimeOffset? UpdatedAtUtc { get; protected set; }
    public Guid UserId { get; protected set; }
}
