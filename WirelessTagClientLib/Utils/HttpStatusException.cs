using System;
using System.Net;

namespace WirelessTagClientLib
{
    /// <summary>
    /// HttpStatusException
    /// </summary>
    public class HttpStatusException : Exception
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="HttpStatusException"/> class.
        /// </summary>
        /// <param name="status">Http status code.</param>
        public HttpStatusException(HttpStatusCode status)
        {
            Status = status;
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="HttpStatusException"/> class.
        /// </summary>
        /// <param name="status">Http status code.</param>
        /// <param name="message"></param>
        public HttpStatusException(HttpStatusCode status, string message)
            : base(message)
        {
            Status = status;
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="HttpStatusException"/> class.
        /// </summary>
        /// <param name="status">Http status code.</param>
        /// <param name="innerException"></param>
        public HttpStatusException(HttpStatusCode status, Exception innerException)
            : base(status.ToString(), innerException)
        {
            Status = status;
        }

        /// <summary>
        /// Get the Http status code.
        /// </summary>
        public HttpStatusCode Status { get; private set; }
    }
}
