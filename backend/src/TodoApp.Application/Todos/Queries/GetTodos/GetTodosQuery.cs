using MediatR;
namespace TodoApp.Application.Todos.Queries.GetTodos;
public sealed record GetTodosQuery(Guid UserId) : IRequest<IReadOnlyCollection<TodoDto>>;
