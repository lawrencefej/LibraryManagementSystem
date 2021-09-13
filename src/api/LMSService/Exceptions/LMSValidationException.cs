using System;

namespace LMSService.Exceptions
{
    public class LMSValidationException : Exception
    {
        public LMSValidationException()
        {
        }

        public LMSValidationException(string message)
            : base(message)
        {
        }

        public LMSValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
