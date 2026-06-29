using TodoApp.Domain.Todos;

namespace TodoApp.Application.Todos;

public sealed record TodoDto(Guid UserId,Guid Id, string Title, bool IsCompleted, DateTimeOffset CreatedAtUtc, DateTimeOffset? UpdatedAtUtc)
{
    public static TodoDto FromEntity(TodoItem item) => new(item.UserId,item.Id, item.Title, item.IsCompleted, item.CreatedAtUtc, item.UpdatedAtUtc);
}
