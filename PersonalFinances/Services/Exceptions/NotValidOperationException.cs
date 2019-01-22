namespace PersonalFinances.Services.Exceptions
{
    public class NotValidOperationException : ModelValidationException
    {
        public NotValidOperationException (string message )
            : base(message)
        {

        }
    }
}