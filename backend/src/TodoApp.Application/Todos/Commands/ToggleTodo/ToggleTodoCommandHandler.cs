using MediatR;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Common.Exceptions;
namespace TodoApp.Application.Todos.Commands.ToggleTodo;
public sealed class ToggleTodoCommandHandler : IRequestHandler<ToggleTodoCommand, TodoDto>
{
    private readonly ITodoRepository _repository;
    public ToggleTodoCommandHandler(ITodoRepository repository) => _repository = repository;
    public async Task<TodoDto> Handle(ToggleTodoCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Todo '{request.Id}' was not found.");
        item.Toggle();
        await _repository.UpdateAsync(item, cancellationToken);
        return TodoDto.FromEntity(item);
    }
}
