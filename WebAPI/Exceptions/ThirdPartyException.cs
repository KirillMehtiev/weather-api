namespace WebAPI.Exceptions;

public class ThirdPartyException : Exception
{
    public ThirdPartyException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}