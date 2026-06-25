using MediatR;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Common.Exceptions;
namespace TodoApp.Application.Todos.Commands.UpdateTodo;
public sealed class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoDto>
{
    private readonly ITodoRepository _repository;
    public UpdateTodoCommandHandler(ITodoRepository repository) => _repository = repository;
    public async Task<TodoDto> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException($"Todo '{request.Id}' was not found.");
        item.Rename(request.Title);
        await _repository.UpdateAsync(item, cancellationToken);
        return TodoDto.FromEntity(item);
    }
}
