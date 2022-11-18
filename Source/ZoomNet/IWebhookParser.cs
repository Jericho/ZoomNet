using ZoomNet.Models.Webhooks;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the webhook parser.
	/// </summary>
	public interface IWebhookParser
	{
		/// <summary>
		/// Verifies the webhook signature.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's webhook.</param>
		/// <param name="secretToken">Your secret token. You can obtain this value in the 'Add Feature' configuration section of you Marketplace Zoom app.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>A boolean value indicating wether the signature was validated or not.</returns>
		bool VerifySignature(string requestBody, string secretToken, string signature, string timestamp);

		/// <summary>
		/// Parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's webhook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		Event ParseEventWebhook(string requestBody);
	}
}
