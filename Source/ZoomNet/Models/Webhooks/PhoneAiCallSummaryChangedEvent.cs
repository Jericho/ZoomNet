using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the system generates or updates an AI call summary.
	/// </summary>
	public class PhoneAiCallSummaryChangedEvent : Event
	{
		/// <summary>
		/// Gets or sets the user's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets AI call summary.
		/// </summary>
		[JsonPropertyName("object")]
		public AiCallSummary CallSummary { get; set; }
	}
}
