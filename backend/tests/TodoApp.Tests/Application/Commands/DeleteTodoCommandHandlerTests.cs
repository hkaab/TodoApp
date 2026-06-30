using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.DeleteTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Tests.Application.Commands;

public sealed class DeleteTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new DeleteTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new DeleteTodoCommand(todoId), CancellationToken.None);
        repository.Verify(x => x.DeleteAsync(todoItem.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task Handle_Should_Throw_When_Todo_Not_Found()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new DeleteTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync((TodoItem?)null);
        Func<Task> act = async () => await handler.Handle(new DeleteTodoCommand(todoId), CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
    [Fact]
    public async Task Handle_Should_Throw_When_CancellationRequested()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new DeleteTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Test Todo");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        var cts = new CancellationTokenSource();
        cts.Cancel();
        Func<Task> act = async () => await handler.Handle(new DeleteTodoCommand(todoId), cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
