namespace DemoSocial.Domain.Exceptions;

public class UserProfileNotValidException : NotValidException
{
    internal UserProfileNotValidException() {}
    internal UserProfileNotValidException(string message) : base(message) {}
    internal UserProfileNotValidException(string message, Exception innerValue) : base(message, innerValue) {}
}