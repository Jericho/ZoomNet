using System;
using System.Net;
using System.Net.Http;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Exception that includes both the formatted message and the status code.
	/// </summary>
	public class ZoomException : Exception
	{
		/// <summary>
		/// Gets the status code of the non-successful call.
		/// </summary>
		public HttpStatusCode StatusCode => ResponseMessage.StatusCode;

		/// <summary>
		/// Gets the HTTP response message which caused the exception.
		/// </summary>
		public HttpResponseMessage ResponseMessage { get; }

		/// <summary>
		/// Gets the human readable representation of the request/response.
		/// </summary>
		public string DiagnosticLog { get; }

		/// <summary>
		/// Gets the Zoom error code.
		/// </summary>
		/// <remarks>
		/// Sometimes the Zoom API documentation contains additional details about a given error code.
		/// </remarks>
		public int? ErrorCode { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="responseMessage">The response message of the non-successful call.</param>
		/// <param name="diagnosticLog">The human readable representation of the request/response.</param>
		/// <param name="errorCode">The Zoom error code.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public ZoomException(string message, HttpResponseMessage responseMessage, string diagnosticLog, int? errorCode = null, Exception innerException = null)
			: base(message, innerException)
		{
			ResponseMessage = responseMessage;
			DiagnosticLog = diagnosticLog;
			ErrorCode = errorCode;
		}
	}
}
