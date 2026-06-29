using System.Collections.Concurrent;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Domain.Todos;

namespace TodoApp.Infrastructure.Persistence;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _items = new();

    public Task<IReadOnlyCollection<TodoItem>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var todos = _items
            .Where(x => x.Value.UserId == userId)
            .Select(x => x.Value)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<TodoItem>>(todos);
    }
    public Task<IReadOnlyCollection<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyCollection<TodoItem>>(_items.Values.ToList());
    }

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _items.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _items[item.Id] = item;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_items.ContainsKey(item.Id))
        {
            throw new KeyNotFoundException();
        }
        _items[item.Id] = item;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _items.TryRemove(id, out _);
        return Task.CompletedTask;
    }


}
