using System;
using System.Collections.Generic;

namespace LMSService.Exceptions
{
    public class NoValuesFoundException : Exception
    {
        public NoValuesFoundException()
        {
        }

        public NoValuesFoundException(string message)
            : base(message)
        {
        }

        public NoValuesFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NoValuesFoundException(List<string> message)
        {

        }
    }
}
