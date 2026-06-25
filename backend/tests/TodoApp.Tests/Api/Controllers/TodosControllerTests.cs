using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
        var todos = new List<TodoDto>
        {
            new(Guid.NewGuid(), "Task 1", false, DateTime.UtcNow,DateTime.UtcNow),
            new(Guid.NewGuid(), "Task 2", true, DateTime.UtcNow,DateTime.UtcNow)
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetTodosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);

        // Act
        var result = await _controller.GetAll(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(okResult.Value);

        Assert.Equal(2, value.Count());
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var command = new CreateTodoCommand("New task");
        var todoDto = new TodoDto(
            Guid.NewGuid(),
            command.Title,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoDto);

        // Act
        var result = await _controller.Create(command, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(nameof(TodosController.GetById), createdResult.ActionName);
        Assert.Equal(todoDto, createdResult.Value);
    }
    [Fact]
    public async Task ToggleTodo_ShouldReturnOk_WithUpdatedTodo()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var command = new ToggleTodoCommand(todoId);
        var updatedTodoDto = new TodoDto(
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
        // Act
        var result = await _controller.GetAll(CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(okResult.Value);
        Assert.Empty(value);
    }



}