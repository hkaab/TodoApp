using MediatR;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Common.Exceptions;
namespace TodoApp.Application.Todos.Queries.GetTodoById;
public sealed class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoDto>
{
    private readonly ITodoRepository _repository;
    public GetTodoByIdQueryHandler(ITodoRepository repository) => _repository = repository;
    public async Task<TodoDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException($"Todo '{request.Id}' was not found.");
        return TodoDto.FromEntity(item);
    }
}
