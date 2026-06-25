using TodoApp.Domain.Common;

namespace TodoApp.Domain.Todos;

public sealed class TodoItem : Entity
{
    private TodoItem() { }

    public TodoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = ValidateTitle(title);
        IsCompleted = false;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public string Title { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }

    public void Rename(string title)
    {
        Title = ValidateTitle(title);
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Toggle()
    {
        IsCompleted = !IsCompleted;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private static string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Todo title is required.", nameof(title));
        return title.Trim();
    }
}
