using Moq;
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
                new TodoItem(Guid.NewGuid(), "Todo 1"),
                new TodoItem(Guid.NewGuid(), "Todo 2")
            };
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<Guid>(),It.IsAny<CancellationToken>())).ReturnsAsync(todos);
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery(Guid.NewGuid());
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
            repositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<Guid>(),It.IsAny<CancellationToken>())).ReturnsAsync(new List<TodoItem>());
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery(Guid.NewGuid());
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
            repositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<TodoItem>());
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery(Guid.NewGuid());
            // Act
            await handler.Handle(query, CancellationToken.None);
            // Assert
            repositoryMock.Verify(r => r.GetByUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task GetTodosQueryHandler_Should_Throw_When_CancellationRequested()
        {
            // Arrange
            var repositoryMock = new Mock<ITodoRepository>();
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery(Guid.NewGuid());
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => handler.Handle(query, cancellationTokenSource.Token));
        }
        [Fact]
        public async Task GetTodosQueryHandler_Should_Return_Todos_For_Specific_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var todos = new List<TodoItem>
            {
                new TodoItem(userId, "Todo 1"),
                new TodoItem(userId, "Todo 2")
            };
            var repositoryMock = new Mock<ITodoRepository>();
            repositoryMock.Setup(r => r.GetByUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(todos);
            var handler = new GetTodosQueryHandler(repositoryMock.Object);
            var query = new GetTodosQuery(userId);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, todo => Assert.Equal(userId, todo.UserId));
        }
    }
}
