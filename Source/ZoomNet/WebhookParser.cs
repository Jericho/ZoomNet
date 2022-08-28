using System;
using System.Security.Cryptography;
using System.Text;
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
			var webHookEvent = JsonSerializer.Deserialize<Event>(requestBody, ZoomNetJsonFormatter.DeserializerOptions);
			return webHookEvent;
		}

		/// <inheritdoc/>
		public Event VerifyAndParseEventWebhook(string requestBody, string secretToken, string signature, string timestamp)
		{
			// Construct the message
			var message = $"v0:{timestamp}:{requestBody}";

			// Hash the message
			var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
			var hashAsHex = ToHex(hashAsBytes);

			// Create the signature
			var calculatedSignature = $"v0={hashAsHex}";

			// Compare the signatures
			if (calculatedSignature != signature) throw new InvalidOperationException("The signature is invalid.");

			// Parse the webhook event
			return ParseEventWebhook(requestBody);
		}

		private static string ToHex(byte[] bytes)
		{
			var result = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i++)
				result.Append(bytes[i].ToString("x2"));
			return result.ToString();
		}
	}
}
