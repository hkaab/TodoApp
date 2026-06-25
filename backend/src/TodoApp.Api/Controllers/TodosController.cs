using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.DeleteTodo;
using TodoApp.Application.Todos.Commands.ToggleTodo;
using TodoApp.Application.Todos.Commands.UpdateTodo;
using TodoApp.Application.Todos.Queries.GetTodoById;
using TodoApp.Application.Todos.Queries.GetTodos;

namespace TodoApp.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/todos")]
public sealed class TodosController : ControllerBase
{
    private readonly ISender _sender;
    public TodosController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetTodosQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetTodoByIdQuery(id), cancellationToken));

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = todo.Id, version = "1.0" }, todo);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoRequest request, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new UpdateTodoCommand(id, request.Title), cancellationToken));

    [HttpPatch("{id:guid}/toggle")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Toggle(Guid id, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new ToggleTodoCommand(id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTodoCommand(id), cancellationToken);
        return NoContent();
    }
}

public sealed record UpdateTodoRequest(string Title);
