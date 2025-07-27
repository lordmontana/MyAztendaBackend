namespace Shared.Web.Exceptions;

public sealed class DomainRuleException(string message)
    : Exception(message)
{
    public int StatusCode => 409;
}