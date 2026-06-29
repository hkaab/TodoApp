using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.Api.Contracts;
using TodoApp.Api.Controllers;
using TodoApp.Application.Todos;
using TodoApp.Application.Todos.Commands.CreateTodo;
using TodoApp.Application.Todos.Commands.ToggleTodo;
using TodoApp.Application.Todos.Queries.GetTodos;
using Xunit;

namespace TodoApp.Api.Tests.Controllers;

public class TodosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TodosController _controller;

    public TodosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TodosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetTodos_ShouldReturnOk_WithTodos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var todos = new List<TodoDto>
        {
            new(userId, Guid.NewGuid(), "Task 1", false, DateTime.UtcNow, DateTime.UtcNow),
            new(userId, Guid.NewGuid(), "Task 2", true, DateTime.UtcNow, DateTime.UtcNow)
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetTodosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);
        // Act
        var result = await _controller.GetTodos(userId,CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(okResult.Value);

        Assert.Equal(2, value.Count());
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTodoCommand(userId, "New task");
        var todoDto = new TodoDto(
            userId,
            Guid.NewGuid(),
            command.Title,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateTodoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoDto);

        var request = new CreateTodoRequest(command.Title);

        // Act
        var result = await _controller.Create(userId, request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(nameof(TodosController.GetById), createdResult.ActionName);
        Assert.Equal(todoDto, createdResult.Value);
    }
    [Fact]
    public async Task ToggleTodo_ShouldReturnOk_WithUpdatedTodo()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var todoId = Guid.NewGuid();
        var command = new ToggleTodoCommand(todoId);
        var updatedTodoDto = new TodoDto(
            userId,
            todoId,
            "Task 1",
            true, // Assuming the task was toggled to completed
            DateTime.UtcNow,
            DateTime.UtcNow
        );
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedTodoDto);
        // Act
        var result = await _controller.Toggle(todoId, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<TodoDto>(okResult.Value);
        Assert.Equal(updatedTodoDto, value);
    }
    [Fact]
    public async Task GetTodos_ShouldReturnOk_WithEmptyList()
    {
        // Arrange
        var todos = new List<TodoDto>();
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetTodosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);

        var userId = Guid.NewGuid(); // Add this line

        // Act
        var result = await _controller.GetTodos(userId, CancellationToken.None); // Pass userId

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(okResult.Value);
        Assert.Empty(value);

    }
    [Fact]
    public async Task CreateTodo_ShouldSendCommand_ToMediator()
    {
        var todoId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new CreateTodoCommand(userId, "Test todo");
        var createdTodoDto = new TodoDto(
            userId,
            todoId,
            "Task 1",
            true, // Assuming the task was toggled to completed
            DateTime.UtcNow,
            DateTime.UtcNow
        );
        _mediatorMock
            .Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTodoDto);

        var request = new CreateTodoRequest(command.Title);

        // Fix: Call the correct overload with all required parameters
        var result = await _controller.Create(userId, request, CancellationToken.None);

        Assert.IsType<CreatedAtActionResult>(result);

        _mediatorMock.Verify(
            x => x.Send(It.Is<CreateTodoCommand>(c => c.UserId == userId && c.Title == command.Title), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}