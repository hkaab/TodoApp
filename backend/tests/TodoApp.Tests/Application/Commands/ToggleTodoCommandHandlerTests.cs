using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.ToggleTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Tests.Application.Commands;

public sealed class ToggleTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Toggle_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new ToggleTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new ToggleTodoCommand(todoId), CancellationToken.None);
        todoItem.IsCompleted.Should().BeTrue();
        repository.Verify(x => x.UpdateAsync(todoItem, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task Handle_Should_Throw_When_Todo_Not_Found()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new ToggleTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync((TodoItem?)null);
        Func<Task> act = async () => await handler.Handle(new ToggleTodoCommand(todoId), CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
    [Fact]
    public async Task Handle_Should_Throw_When_CancellationRequested()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new ToggleTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        var cts = new CancellationTokenSource();
        cts.Cancel();
        Func<Task> act = async () => await handler.Handle(new ToggleTodoCommand(todoId), cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
