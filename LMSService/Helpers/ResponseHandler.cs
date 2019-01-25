using System;
using System.Collections.Generic;
using System.Text;

namespace LMSService.Helpers
{
    public class ResponseHandler
    {
        public int Id { get; set; }

        public List<string> Errors { get; set; }

        public object Result { get; set; }

        public bool Valid { get; set; }

        public ResponseHandler(object result, List<string> errors)
        {
            Errors = errors;
            Result = result;
        }

        public ResponseHandler()
        {
        }
    }
}
