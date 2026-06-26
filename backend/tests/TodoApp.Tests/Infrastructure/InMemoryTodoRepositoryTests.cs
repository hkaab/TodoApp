using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TodoApp.Tests.Infrastructure
{
    public sealed class InMemoryTodoRepositoryTests
    {
        [Fact]
        public async Task AddAndGetTodoItem_ShouldReturnAddedItem()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            // Act
            await repository.AddAsync(todoItem, CancellationToken.None);
            var retrievedItem = await repository.GetByIdAsync(todoItem.Id, CancellationToken.None);
            // Assert
            Assert.NotNull(retrievedItem);
            Assert.Equal(todoItem.Id, retrievedItem!.Id);
            Assert.Equal(todoItem.Title, retrievedItem.Title);
            Assert.Equal(todoItem.IsCompleted, retrievedItem.IsCompleted);
        }
        [Fact]
        public async Task UpdateTodoItem_ShouldReturnUpdatedItem()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            await repository.AddAsync(todoItem, CancellationToken.None);
            // Act
            todoItem.Toggle();
            await repository.UpdateAsync(todoItem, CancellationToken.None);
            var retrievedItem = await repository.GetByIdAsync(todoItem.Id, CancellationToken.None);
            // Assert
            Assert.NotNull(retrievedItem);
            Assert.Equal(todoItem.Id, retrievedItem!.Id);
            Assert.Equal(todoItem.Title, retrievedItem.Title);
            Assert.Equal(todoItem.IsCompleted, retrievedItem.IsCompleted);
        }
        [Fact]
        public async Task DeleteTodoItem_ShouldReturnNull()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            await repository.AddAsync(todoItem, CancellationToken.None);
            // Act
            await repository.DeleteAsync(todoItem.Id, CancellationToken.None);
            var retrievedItem = await repository.GetByIdAsync(todoItem.Id, CancellationToken.None);
            // Assert
            Assert.Null(retrievedItem);
        }
        [Fact]
        public async Task GetAllTodoItems_ShouldReturnAllItems()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem1 = new TodoApp.Domain.Todos.TodoItem("Test Todo 1");
            var todoItem2 = new TodoApp.Domain.Todos.TodoItem("Test Todo 2");
            await repository.AddAsync(todoItem1, CancellationToken.None);
            await repository.AddAsync(todoItem2, CancellationToken.None);
            // Act
            var allItems = await repository.GetAllAsync(CancellationToken.None);
            // Assert
            Assert.Equal(2, allItems.Count);
            Assert.Contains(allItems, item => item.Id == todoItem1.Id);
            Assert.Contains(allItems, item => item.Id == todoItem2.Id);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistentItem()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var nonExistentId = Guid.NewGuid();
            // Act
            var retrievedItem = await repository.GetByIdAsync(nonExistentId, CancellationToken.None);
            // Assert
            Assert.Null(retrievedItem);
        }
        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionForNonExistentItem()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var nonExistentItem = new TodoApp.Domain.Todos.TodoItem("Non-existent Todo");
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await repository.UpdateAsync(nonExistentItem, CancellationToken.None);
            });
        }
        [Fact]
        public async Task DeleteAsync_ShouldNotThrowExceptionForNonExistentItem()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var nonExistentId = Guid.NewGuid();
            // Act & Assert
            await repository.DeleteAsync(nonExistentId, CancellationToken.None);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollectionWhenNoItems()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            // Act
            var allItems = await repository.GetAllAsync(CancellationToken.None);
            // Assert
            Assert.Empty(allItems);
        }
        [Fact]
        public async Task AddAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.AddAsync(todoItem, cts.Token);
            });
        }
        [Fact]
        public async Task GetByIdAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.GetByIdAsync(Guid.NewGuid(), cts.Token);
            });
        }
        [Fact]
        public async Task UpdateAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var todoItem = new TodoApp.Domain.Todos.TodoItem("Test Todo");
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.UpdateAsync(todoItem, cts.Token);
            });
        }
        [Fact]
        public async Task DeleteAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.DeleteAsync(Guid.NewGuid(), cts.Token);
            });
        }
        [Fact]
        public async Task GetAllAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.GetAllAsync(cts.Token);
            });
        }
    }
}
