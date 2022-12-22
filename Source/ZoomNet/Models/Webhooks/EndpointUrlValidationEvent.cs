using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when Zoom wants to validate the event notification endpoint URL you have configured in your account.
	/// </summary>
	public class EndpointUrlValidationEvent : Event
	{
		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		[JsonPropertyName("plainToken")]
		public string PlainToken { get; set; }

		/// <summary>
		/// Generates the payload that should be returned to Zoom when your are asked to validate your webhook endpoint URL.
		/// </summary>
		/// <param name="secretToken">Your webhoop app's secret token.</param>
		/// <returns>The payload to be returned to Zoom.</returns>
		public string GenerateUrlValidationResponse(string secretToken)
		{
			// Generate the encrypted token according to Zoom's instructions: https://marketplace.zoom.us/docs/api-reference/webhook-reference/#verify-webhook-events
			var crypto = new HMACSHA256(Encoding.UTF8.GetBytes(secretToken));
			var encryptedToken = crypto.ComputeHash(Encoding.UTF8.GetBytes(this.PlainToken)).ToHexString();

			var data = new JsonObject
			{
				{ "plainToken", this.PlainToken },
				{ "encryptedToken", encryptedToken },
			};

			var jsonString = JsonSerializer.Serialize(data, typeof(JsonObject), JsonFormatter.SerializationContext);
			return jsonString;
		}
	}
}
