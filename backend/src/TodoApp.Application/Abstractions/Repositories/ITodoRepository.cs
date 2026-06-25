using TodoApp.Domain.Todos;

namespace TodoApp.Application.Abstractions.Repositories;

public interface ITodoRepository
{
    Task<IReadOnlyCollection<TodoItem>> GetAllAsync(CancellationToken cancellationToken);
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(TodoItem item, CancellationToken cancellationToken);
    Task UpdateAsync(TodoItem item, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
