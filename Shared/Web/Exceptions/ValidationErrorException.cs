namespace Shared.Web.Exceptions;

public sealed class ValidationErrorException(
    IEnumerable<(string Field, string Error)> failures)
    : Exception("Validation failed")
{
    public int StatusCode => 422;
    public IReadOnlyList<(string Field, string Error)> Errors { get; } = failures.ToList();
}