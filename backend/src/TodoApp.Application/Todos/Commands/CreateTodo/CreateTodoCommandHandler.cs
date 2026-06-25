using MediatR;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Domain.Todos;

namespace TodoApp.Application.Todos.Commands.CreateTodo;

public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, TodoDto>
{
    private readonly ITodoRepository _repository;
    public CreateTodoCommandHandler(ITodoRepository repository) => _repository = repository;

    public async Task<TodoDto> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var item = new TodoItem(request.Title);
        await _repository.AddAsync(item, cancellationToken);
        return TodoDto.FromEntity(item);
    }
}
