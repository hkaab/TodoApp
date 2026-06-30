using FluentAssertions;
using TodoApp.Application.Todos.Commands.CreateTodo;
using Xunit;

namespace TodoApp.Tests.Application.Commands;

public sealed class CreateTodoCommandValidatorTests
{
    [Fact]
    public void Validator_Should_Fail_When_Title_Is_Empty()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), ""));
        result.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Validator_Should_Pass_When_Title_Is_Not_Empty()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), "Valid Title"));
        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validator_Should_Fail_When_Title_Is_Too_Long()
    {
        var validator = new CreateTodoCommandValidator();
        var longTitle = new string('a', 256); // Assuming max length is 255
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), longTitle));
        result.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Validator_Should_Pass_When_Title_Is_At_Max_Length()
    {
        var validator = new CreateTodoCommandValidator();
        var maxLengthTitle = new string('a', 200); // Assuming max length is 200
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), maxLengthTitle));
        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validator_Should_Fail_When_UserId_Is_Empty()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.Empty, "Valid Title"));
        result.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Validator_Should_Pass_When_UserId_Is_Not_Empty()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), "Valid Title"));
        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validator_Should_Fail_When_Both_Title_And_UserId_Are_Invalid()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.Empty, ""));
        result.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Validator_Should_Pass_When_Both_Title_And_UserId_Are_Valid()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), "Valid Title"));
        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validator_Should_Fail_When_Title_Is_Whitespace()
    {
        var validator = new CreateTodoCommandValidator();
        var result = validator.Validate(new CreateTodoCommand(Guid.NewGuid(), "   "));
        result.IsValid.Should().BeFalse();
    }

}

