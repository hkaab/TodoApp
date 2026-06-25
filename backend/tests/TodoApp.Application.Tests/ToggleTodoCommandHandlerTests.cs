using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.ToggleTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Application.Tests;

public sealed class ToggleTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Toggle_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new ToggleTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem("Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new ToggleTodoCommand(todoId), CancellationToken.None);
        todoItem.IsCompleted.Should().BeTrue();
        repository.Verify(x => x.UpdateAsync(todoItem, It.IsAny<CancellationToken>()), Times.Once);
    }
}
