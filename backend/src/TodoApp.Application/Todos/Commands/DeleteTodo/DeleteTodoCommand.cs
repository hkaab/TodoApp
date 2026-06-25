using MediatR;
namespace TodoApp.Application.Todos.Commands.DeleteTodo;
public sealed record DeleteTodoCommand(Guid Id) : IRequest;
