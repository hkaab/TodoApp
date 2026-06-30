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
            var todoItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo");
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
            var todoItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo");
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
            var todoItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo");
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
            var todoItem1 = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo 1");
            var todoItem2 = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo 2");
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
            var nonExistentItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Non-existent Todo");
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
            var todoItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo");
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
            var todoItem = new TodoApp.Domain.Todos.TodoItem(Guid.NewGuid(), "Test Todo");
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
        [Fact]
        public async Task GetByUserAsync_ShouldReturnItemsForSpecificUser()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var userId = Guid.NewGuid();
            var todoItem1 = new TodoApp.Domain.Todos.TodoItem(userId, "Test Todo 1");
            var todoItem2 = new TodoApp.Domain.Todos.TodoItem(userId, "Test Todo 2");
            await repository.AddAsync(todoItem1, CancellationToken.None);
            await repository.AddAsync(todoItem2, CancellationToken.None);
            // Act
            var userItems = await repository.GetByUserAsync(userId, CancellationToken.None);
            // Assert
            Assert.Equal(2, userItems.Count);
            Assert.Contains(userItems, item => item.Id == todoItem1.Id);
            Assert.Contains(userItems, item => item.Id == todoItem2.Id);
        }
        [Fact]
        public async Task GetByUserAsync_ShouldReturnEmptyCollectionForUserWithNoItems()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var userId = Guid.NewGuid();
            // Act
            var userItems = await repository.GetByUserAsync(userId, CancellationToken.None);
            // Assert
            Assert.Empty(userItems);
        }
        [Fact]
        public async Task GetByUserAsync_ShouldThrowExceptionWhenCancellationRequested()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await repository.GetByUserAsync(Guid.NewGuid(), cts.Token);
            });
        }
        [Fact]
        public async Task GetByUserAsync_ShouldReturnItemsForMultipleUsers()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var todoItem1 = new TodoApp.Domain.Todos.TodoItem(userId1, "Test Todo 1");
            var todoItem2 = new TodoApp.Domain.Todos.TodoItem(userId2, "Test Todo 2");
            await repository.AddAsync(todoItem1, CancellationToken.None);
            await repository.AddAsync(todoItem2, CancellationToken.None);
            // Act
            var user1Items = await repository.GetByUserAsync(userId1, CancellationToken.None);
            var user2Items = await repository.GetByUserAsync(userId2, CancellationToken.None);
            // Assert
            Assert.Single(user1Items);
            Assert.Equal(todoItem1.Id, user1Items.First().Id);
            Assert.Single(user2Items);
            Assert.Equal(todoItem2.Id, user2Items.First().Id);
        }
        [Fact]
        public async Task GetByUserAsync_ShouldReturnEmptyCollectionForNonExistentUser()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var userId = Guid.NewGuid();
            // Act
            var userItems = await repository.GetByUserAsync(userId, CancellationToken.None);
            // Assert
            Assert.Empty(userItems);
        }
        [Fact]
        public async Task GetByUserAsync_ShouldReturnItemsInOrderOfAddition()
        {
            // Arrange
            var repository = new TodoApp.Infrastructure.Persistence.InMemoryTodoRepository();
            var userId = Guid.NewGuid();
            var todoItem1 = new TodoApp.Domain.Todos.TodoItem(userId, "Test Todo 1");
            var todoItem2 = new TodoApp.Domain.Todos.TodoItem(userId, "Test Todo 2");
            await repository.AddAsync(todoItem1, CancellationToken.None);
            await repository.AddAsync(todoItem2, CancellationToken.None);
            // Act
            var userItems = await repository.GetByUserAsync(userId, CancellationToken.None);
            // Assert
            Assert.Equal(2, userItems.Count);
            Assert.Equal(todoItem1.Id, userItems.First().Id);
            Assert.Equal(todoItem2.Id, userItems.Last().Id);
        }
    }
}
