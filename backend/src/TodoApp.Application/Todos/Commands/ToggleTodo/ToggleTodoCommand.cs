using MediatR;
namespace TodoApp.Application.Todos.Commands.ToggleTodo;
public sealed record ToggleTodoCommand(Guid Id) : IRequest<TodoDto>;
