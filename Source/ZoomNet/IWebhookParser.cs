using ZoomNet.Models.Webhooks;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the webhook parser.
	/// </summary>
	public interface IWebhookParser
	{
		/// <summary>
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's WebHook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		Event ParseEventWebhook(string requestBody);
	}
}
