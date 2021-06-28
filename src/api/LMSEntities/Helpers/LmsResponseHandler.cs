using System.Collections.Generic;

namespace LMSEntities.Helpers
{
    public class LmsResponseHandler<T>
    {
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
        public T Item { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Unsuccessful Response with single error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public LmsResponseHandler(string errorMessage)
        {
            Error = errorMessage;
            IsSuccessful = false;
        }

        /// <summary>
        /// Successful Response
        /// </summary>
        /// <param name="result"></param>

        public LmsResponseHandler(T item)
        {
            Item = item;
            IsSuccessful = true;
        }

        public LmsResponseHandler(IList<string> errors)
        {
            Errors = errors;
            IsSuccessful = false;
        }

        public static LmsResponseHandler<T> Failed(string error)
        {
            return new LmsResponseHandler<T>(error);
        }

        public static LmsResponseHandler<T> Failed(IList<string> error)
        {
            return new LmsResponseHandler<T>(error);
        }

        public static LmsResponseHandler<T> Successful(T item)
        {
            return new LmsResponseHandler<T>(item);
        }

    }
}
