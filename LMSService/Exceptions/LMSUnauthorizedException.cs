using System;
using System.Collections.Generic;
using System.Text;

namespace LMSService.Exceptions
{
    public class LMSUnauthorizedException : Exception
    {
        public LMSUnauthorizedException()
        {
        }

        public LMSUnauthorizedException(string message)
            : base(message)
        {
        }

        public LMSUnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public LMSUnauthorizedException(List<string> message)
        {

        }
    }
}
