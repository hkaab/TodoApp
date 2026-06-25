using MediatR;
namespace TodoApp.Application.Todos.Commands.UpdateTodo;
public sealed record UpdateTodoCommand(Guid Id, string Title) : IRequest<TodoDto>;
