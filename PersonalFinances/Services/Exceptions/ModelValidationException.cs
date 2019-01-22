using System;

namespace PersonalFinances.Services.Exceptions
{
    public class ModelValidationException : ApplicationException
    {
        public ModelValidationException (string message) 
            : base (message)
        {
        }
    }
}