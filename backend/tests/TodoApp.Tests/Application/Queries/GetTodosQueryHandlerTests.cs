using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Abstractions.Repositories;
using TodoApp.Application.Todos.Queries.GetTodos;
using TodoApp.Domain.Todos;
using Xunit;

namespace TodoApp.Tests.Application.Queries
{
    public class GetTodosQueryHandlerTests
    {
        [Fact]
        public async Task GetTodosQueryHandler_Should_Return_Todos_In_Descending_Order()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new("Todo 1"),
                new("Todo 2")
            };
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(todos);
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery();
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Todo 2", result.First().Title);
        }
        [Fact]
        public async Task GetTodosQueryHandler_Should_Return_Empty_List_When_No_Todos()
        {
            // Arrange
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TodoItem>());
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery();
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetTodosQueryHandler_Should_Call_Repository_Once()
        {
            // Arrange
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TodoItem>());
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery();
            // Act
            await handler.Handle(query, CancellationToken.None);
            // Assert
            repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task GetTodosQueryHandler_Should_Throw_Exception_When_Repository_Fails()
        {
            // Arrange
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Repository failure"));
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery();
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
