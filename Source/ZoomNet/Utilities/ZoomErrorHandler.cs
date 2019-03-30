using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Error handler for requests dispatched to the Zoom API.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class ZoomErrorHandler : IHttpFilter
	{
		#region PUBLIC METHODS

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request) { }

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException)
		{
			if (response.Message.IsSuccessStatusCode) return;

			var errorMessage = GetErrorMessage(response.Message).Result;
			throw new Exception(errorMessage);
		}

		private static async Task<string> GetErrorMessage(HttpResponseMessage message)
		{
			// Default error message
			var errorMessage = $"{(int)message.StatusCode}: {message.ReasonPhrase}";

			if (message.Content != null)
			{
				/*
					In case of an error, the Zoom API returns a JSON string that looks like this:
					{
						"code": 300,
						"message": "This meeting has not registration required: 544993922"
					}
				*/

				var responseContent = await message.Content.ReadAsStringAsync(null).ConfigureAwait(false);

				if (!string.IsNullOrEmpty(responseContent))
				{
					try
					{
						var jObject = JObject.Parse(responseContent);
						var codeProperty = jObject["code"];
						var messageProperty = jObject["message"];

						if (messageProperty != null)
						{
							errorMessage = messageProperty.Value<string>();
						}
						else if (codeProperty != null)
						{
							errorMessage = $"Error code: {codeProperty.Value<string>()}";
						}
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
					}
					catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
					{
						// Intentionally ignore parsing errors
					}
				}
			}

			return errorMessage;
		}

		#endregion
	}
}
