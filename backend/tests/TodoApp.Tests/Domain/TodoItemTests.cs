using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TodoApp.Tests.Domain
{
    public sealed class TodoItemTests
    {
        [Fact]
        public void Constructor_Should_Initialize_TodoItem()
        {
            // Arrange
            var title = "Test Todo";
            // Act
            var todoItem = new TodoApp.Domain.Todos.TodoItem(title);
            // Assert
            Assert.Equal(title, todoItem.Title);
            Assert.False(todoItem.IsCompleted);
            Assert.NotEqual(Guid.Empty, todoItem.Id);
            Assert.True(todoItem.CreatedAtUtc <= DateTimeOffset.UtcNow);
        }
        [Fact]
        public void Rename_Should_Update_Title_And_UpdatedAtUtc()
        {
            // Arrange
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Old Title");
            var newTitle = "New Title";
            // Act
            todoItem.Rename(newTitle);
            // Assert
            Assert.Equal(newTitle, todoItem.Title);
            Assert.NotNull(todoItem.UpdatedAtUtc);
            Assert.True(todoItem.UpdatedAtUtc <= DateTimeOffset.UtcNow);
        }
        [Fact]
        public void Toggle_Should_Change_IsCompleted_And_Update_UpdatedAtUtc()
        {
            // Arrange
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            var initialIsCompleted = todoItem.IsCompleted;
            // Act
            todoItem.Toggle();
            // Assert
            Assert.NotEqual(initialIsCompleted, todoItem.IsCompleted);
            Assert.NotNull(todoItem.UpdatedAtUtc);
            Assert.True(todoItem.UpdatedAtUtc <= DateTimeOffset.UtcNow);
        } 
        [Fact]
        public void Constructor_Should_Throw_Exception_For_Empty_Title()
        {
            // Arrange
            var emptyTitle = "";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new TodoApp.Domain.Todos.TodoItem(emptyTitle));
            Assert.Equal("Todo title is required. (Parameter 'title')", exception.Message);
        }
        [Fact]
        public void Rename_Should_Throw_Exception_For_Empty_Title()
        {
            // Arrange
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Old Title");
            var emptyTitle = "";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => todoItem.Rename(emptyTitle));
            Assert.Equal("Todo title is required. (Parameter 'title')", exception.Message);
        }
    }
}
