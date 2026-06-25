using MediatR;
namespace TodoApp.Application.Todos.Queries.GetTodoById;
public sealed record GetTodoByIdQuery(Guid Id) : IRequest<TodoDto>;
