using MediatR;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Common.Exceptions;
namespace TodoApp.Application.Todos.Commands.DeleteTodo;
public sealed class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
{
    private readonly ITodoRepository _repository;
    public DeleteTodoCommandHandler(ITodoRepository repository) => _repository = repository;
    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Todo '{request.Id}' was not found.");
        await _repository.DeleteAsync(item.Id, cancellationToken);
    }
}
