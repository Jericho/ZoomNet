using System;
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
		/// <param name="requestBody">The content submitted by Zoom's webhook.</param>
		/// <returns>An <see cref="Event" />.</returns>
		Event ParseEventWebhook(string requestBody);

		/// <summary>
		/// Verifies the signature and parses the event webhook.
		/// </summary>
		/// <param name="requestBody">The content submitted by Zoom's webhook.</param>
		/// <param name="secretToken">Your secret token. You can obtain this value in the 'Add Feature' configuration section of you Marketplace Zoom app.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>An <see cref="Event" />.</returns>
		/// <exception cref="InvalidOperationException">The signature is invalid.</exception>
		Event VerifyAndParseEventWebhook(string requestBody, string secretToken, string signature, string timestamp);
	}
}
