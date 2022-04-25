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
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's WebHook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		public Event ParseEventWebhook(string requestBody)
		{
			var webHookEvent = JsonSerializer.Deserialize<Event>(requestBody, ZoomNetJsonFormatter.DeserializerOptions);
			return webHookEvent;
		}
	}
}
