using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an account user's outbound text violates a SMS etiquette block policy defined in the Zoom phone web admin portal.
	/// </summary>
	public class PhoneSmsEtiquetteBlockEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets information about SMS etiquette block policy that was violated.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsEtiquettePolicy Policy { get; set; }
	}
}
