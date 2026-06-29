using MediatR;

namespace TodoApp.Application.Todos.Commands.CreateTodo;

public sealed record CreateTodoCommand(Guid UserId, string Title) : IRequest<TodoDto>;
