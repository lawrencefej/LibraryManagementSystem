using System.Collections.Generic;

namespace LMSEntities.Helpers
{
    public class ResponseHandler
    {
        public int Id { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public string Message { get; protected set; }

        public object Result { get; set; }

        public bool IsSuccessful { get; set; }

        public ResponseHandler(object result, List<string> errors)
        {
            Errors = errors;
            Result = result;
        }

        public ResponseHandler()
        {
        }

        /// <summary>
        /// Unsuccessful Response with single error message
        /// </summary>
        /// <param name="message"></param>
        public ResponseHandler(string message)
        {
            Message = message;
            IsSuccessful = false;
        }

        /// <summary>
        /// Successful Response
        /// </summary>
        /// <param name="result"></param>
        public ResponseHandler(object result, int id)
        {
            Result = result;
            IsSuccessful = true;
            Id = id;
        }

        /// <summary>
        /// Unsuccessful Response with list of errors
        /// </summary>
        /// <param name="errors"></param>
        public ResponseHandler(List<string> errors)
        {
            Errors = errors;
        }
    }
}
