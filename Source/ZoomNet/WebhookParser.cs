using System.Text.Json;
using ZoomNet.Json;
using ZoomNet.Models.Webhooks;

namespace ZoomNet
{
	/// <summary>
	/// Allows parsing of information posted from Zoom.
	/// </summary>
	public class WebhookParser : IWebhookParser
	{
		/// <summary>
		/// The name of the HTTP header where Zoom stores the verification token.
		/// </summary>
		public const string AUTHORIZATION_HEADER_NAME = "authorization";

		/// <summary>
		/// The name of the HTTP header where Zoom stores the encrypted authorization signature.
		/// </summary>
		public const string SIGNATURE_HEADER_NAME = "x-zm-signature";

		/// <summary>
		/// The name of the HTTP header where Zoom stores the time the request was sent, in epoch format.
		/// </summary>
		public const string TIMESTAMP_HEADER_NAME = "x-zm-request-timestamp";

		/// <summary>
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's WebHook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		public Event ParseEventWebhook(string requestBody)
		{
			var webHookEvent = JsonSerializer.Deserialize<Event>(requestBody, JsonFormatter.DeserializerOptions);
			return webHookEvent;
		}
	}
}
