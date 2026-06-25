using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.UpdateTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Application.Tests;

public sealed class UpdateTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new UpdateTodoCommandHandler(repository.Object);
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem("Old Title");
        repository.Setup(x => x.GetByIdAsync(todoId, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        await handler.Handle(new UpdateTodoCommand(todoId, "New Title"), CancellationToken.None);
        todoItem.Title.Should().Be("New Title");
        repository.Verify(x => x.UpdateAsync(todoItem, It.IsAny<CancellationToken>()), Times.Once);
    }   
}
