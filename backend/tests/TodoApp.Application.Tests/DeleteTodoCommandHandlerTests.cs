using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.DeleteTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Application.Tests;

public sealed class DeleteTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new DeleteTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem("Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new DeleteTodoCommand(todoId), CancellationToken.None);
        repository.Verify(x => x.DeleteAsync(todoItem.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
