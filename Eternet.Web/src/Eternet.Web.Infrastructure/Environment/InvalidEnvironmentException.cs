namespace Eternet.Web.Infrastructure.Environment;

[Serializable]
internal class InvalidEnvironmentException : Exception
{
    public InvalidEnvironmentException()
    {
    }

    public InvalidEnvironmentException(string? message) : base(message)
    {
    }

    public InvalidEnvironmentException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
