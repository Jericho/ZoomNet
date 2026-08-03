using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Error handler for requests dispatched to the Zoom API.
	/// </summary>
	/// <seealso cref="IHttpFilter" />
	internal class ZoomErrorHandler : IHttpFilter
	{
		private const string DEFAULT_HTTP_200_EXCEPTION_MESSAGE = "The Zoom API returned a status code that indicates that your request was unseccessful, without providing an explanation. Typically this means that you either lack the necessary permissions or that a paid account is required and you have a free account.";

		/// <summary>
		/// Gets a reference to the diagnostic store.
		/// </summary>
		public IDiagnosticStore DiagnosticStore { get; }

		/// <summary>
		/// Gets a value indicating whether HTTP 200 returned by the Zoom API should be treated as a failure.
		/// </summary>
		/// <remarks>
		/// As incredible as it sounds, Zoom sometimes uses HTTP 200 to indicate that a particular request has failed.
		/// One such example is when you attempt to update a webinar but your account does not have a Webinar subscription plan. See the <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/webinars/webinarupdate#responses">'Responses' section in the documentation about updating a webinar</a>.
		/// Another example is when you attempt to create a role but you do not have the permission to do so. See the <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/roles/createrole#responses">'Responses' section in the documentation about creating a role</a>.
		/// </remarks>
		public bool TreatHttp200AsException { get; }

		/// <summary>
		/// Gets a custom error message used when the Zoom API returns a HTTP 200 status code that is treated as a failure.
		/// </summary>
		/// <remarks>
		/// A generic message is used if you do not provide your own custom message.
		/// </remarks>
		public string CustomHttp200ExceptionMessage { get; }

		public ZoomErrorHandler(IDiagnosticStore diagnosticStore)
			: this(diagnosticStore, false, null) { }

		public ZoomErrorHandler(IDiagnosticStore diagnosticStore, bool treatHttp200AsException, string customHttp200ExceptionMessage)
		{
			DiagnosticStore = diagnosticStore ?? throw new ArgumentNullException(nameof(diagnosticStore));
			TreatHttp200AsException = treatHttp200AsException;
			CustomHttp200ExceptionMessage = customHttp200ExceptionMessage;
		}

		#region PUBLIC METHODS

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request) { }

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException)
		{
			var (isError, errorMessage, errorCode) = response.Message.GetErrorMessageAsync().GetAwaiter().GetResult();
			if (!isError) return;

			var diagnosticLog = "Diagnostic log unavailable";
			var diagnosticId = response.Message.Headers.GetValue(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			if (string.IsNullOrEmpty(diagnosticId)) diagnosticId = response.Message.RequestMessage.Headers.GetValue(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			if (!string.IsNullOrEmpty(diagnosticId) && DiagnosticStore.TryGetValue(diagnosticId, out var diagnosticInfo))
			{
				diagnosticLog = diagnosticInfo.GetFormattedLog();
			}

			if (TreatHttp200AsException && response.Status == HttpStatusCode.OK)
			{
				// We favor the custom message provided by developer or the message in the response from Zoom.
				// If both of these messages are null, we fallback on a generic message.
				errorMessage = CustomHttp200ExceptionMessage ?? errorMessage ?? DEFAULT_HTTP_200_EXCEPTION_MESSAGE;

				throw new ZoomException(errorMessage, response.Message, diagnosticLog);
			}
			else
			{
				throw new ZoomException(errorMessage, response.Message, diagnosticLog, errorCode);
			}
		}

		#endregion
	}
}
