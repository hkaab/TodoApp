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

namespace TodoApp.Application.Tests.Queries
{
    public class GetTodosQueryHandlerTests
    {
        [Fact] 
        public async Task GetTodosQueryHandler_Should_Return_Todos_In_Descending_Order()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new TodoItem ("Todo 1"),
                new TodoItem ("Todo 2")
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
    }
}
