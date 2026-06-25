using MediatR;

namespace TodoApp.Application.Todos.Commands.CreateTodo;

public sealed record CreateTodoCommand(string Title) : IRequest<TodoDto>;
