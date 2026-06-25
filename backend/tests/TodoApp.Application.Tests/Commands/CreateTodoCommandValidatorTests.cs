using FluentAssertions;
using TodoApp.Application.Todos.Commands.CreateTodo;
using Xunit;

namespace TodoApp.Application.Tests.Commands;

public sealed class CreateTodoCommandValidatorTests
{
    [Fact]
    public void Validator_Should_Fail_When_Title_Is_Empty()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(""));
        result.IsValid.Should().BeFalse();
    }
}
