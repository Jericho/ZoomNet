using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an account user's outbound text violates a SMS etiquette warn policy defined in Zoom phone web admin portal.
	/// </summary>
	public class PhoneSmsEtiquetteWarnEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets information about SMS etiquette warn policy that was violated.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsEtiquettePolicy Policy { get; set; }
	}
}
