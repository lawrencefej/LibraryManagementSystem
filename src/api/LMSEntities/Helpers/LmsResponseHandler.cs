using System.Collections.Generic;

namespace LMSEntities.Helpers
{
    public class LmsResponseHandler<T>
    {
        public string Error { get; set; }
        public bool Succeeded { get; set; }
        public T Item { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Unsuccessful Response with single error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public LmsResponseHandler(string errorMessage)
        {
            Error = errorMessage;
            Succeeded = false;
        }

        /// <summary>
        /// Successful Response
        /// </summary>
        /// <param name="result"></param>

        public LmsResponseHandler(T item)
        {
            Item = item;
            Succeeded = true;
        }

        public LmsResponseHandler(IList<string> errors)
        {
            Errors = errors;
            Succeeded = false;
        }

        public LmsResponseHandler()
        {
            Succeeded = true;
        }

        public static LmsResponseHandler<T> Failed(string error)
        {
            return new LmsResponseHandler<T>(error);
        }

        public static LmsResponseHandler<T> Failed(IList<string> error)
        {
            return new LmsResponseHandler<T>(error);
        }

        public static LmsResponseHandler<T> Successful()
        {
            return new LmsResponseHandler<T>();
        }

        public static LmsResponseHandler<T> Successful(T item)
        {
            return new LmsResponseHandler<T>(item);
        }

    }
}
