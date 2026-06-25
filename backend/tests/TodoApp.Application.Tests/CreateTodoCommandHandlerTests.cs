using FluentAssertions;
using Moq;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Application.Tests;

public sealed class CreateTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Todo()
    {
        var repository = new Mock<ITodoRepository>();
        var handler = new CreateTodoCommandHandler(repository.Object);

        var result = await handler.Handle(new CreateTodoCommand("Write tests"), CancellationToken.None);

        result.Title.Should().Be("Write tests");
        repository.Verify(x => x.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
