using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Todos.Queries.GetTodoById;
using Xunit;

namespace TodoApp.Application.Tests.Queries
{
   public class GetTodoByIdQueryTests
    {
        [Fact]
        public void GetTodoByIdQuery_Should_Have_Correct_Id()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var query = new GetTodoByIdQuery(expectedId);
            // Act
            var actualId = query.Id;
            // Assert
            Assert.Equal(expectedId, actualId);
        }
        [Fact]
        public void GetTodoByIdQuery_Should_Throw_Exception_For_Empty_Id()
        {
            // Arrange
            var emptyId = Guid.Empty;
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new GetTodoByIdQuery(emptyId));
        }
    }
}
