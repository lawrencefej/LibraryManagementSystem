using System;
using System.Collections.Generic;
using System.Text;

namespace LMSService.Exceptions
{
    public class TestValidationException : Exception
    {
        public TestValidationException()
        {
        }

        public TestValidationException(string message)
            : base(message)
        {
        }

        public TestValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TestValidationException(List<string> message)
        {

        }
    }
}
