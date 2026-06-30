using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.UpdateTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Tests.Application.Commands;

public sealed class UpdateTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new UpdateTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Old Title");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new UpdateTodoCommand(todoId, "New Title"), CancellationToken.None);
        todoItem.Title.Should().Be("New Title");
        repository.Verify(x => x.UpdateAsync(todoItem, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task Handle_Should_Throw_When_Todo_Not_Found()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new UpdateTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync((TodoItem?)null);
        Func<Task> act = async () => await handler.Handle(new UpdateTodoCommand(todoId, "New Title"), CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
    [Fact]
    public async Task Handle_Should_Throw_When_CancellationRequested()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new UpdateTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Old Title");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        var cts = new CancellationTokenSource();
        cts.Cancel();
        Func<Task> act = async () => await handler.Handle(new UpdateTodoCommand(todoId, "New Title"), cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
    [Fact]
    public async Task Handle_Should_Throw_When_Title_Is_Empty()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new UpdateTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem(todoId, "Old Title");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        Func<Task> act = async () => await handler.Handle(new UpdateTodoCommand(todoId, ""), CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
