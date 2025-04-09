namespace WebAPI.Exceptions;

public class MaxSubscriptionsLimitException : Exception
{
    public MaxSubscriptionsLimitException(string message = "Maximum subscription limit reached. Please remove old subscriptions to create a new one.") : base(message) { }
}