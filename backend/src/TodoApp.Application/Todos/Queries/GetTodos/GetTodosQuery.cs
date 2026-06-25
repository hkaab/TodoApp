using MediatR;
namespace TodoApp.Application.Todos.Queries.GetTodos;
public sealed record GetTodosQuery : IRequest<IReadOnlyCollection<TodoDto>>;
