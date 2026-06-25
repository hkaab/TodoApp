using MediatR;
using TodoApp.Application.Abstractions.Repositories;
namespace TodoApp.Application.Todos.Queries.GetTodos;
public sealed class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IReadOnlyCollection<TodoDto>>
{
    private readonly ITodoRepository _repository;
    public GetTodosQueryHandler(ITodoRepository repository) => _repository = repository;
    public async Task<IReadOnlyCollection<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return items.OrderByDescending(x => x.CreatedAtUtc).Select(TodoDto.FromEntity).ToList();
    }
}
