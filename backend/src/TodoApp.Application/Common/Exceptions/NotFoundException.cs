using System.Diagnostics.CodeAnalysis;

namespace TodoApp.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
