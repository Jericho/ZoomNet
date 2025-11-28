using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the phone voicemail.
	/// </summary>
	public abstract class PhoneVoicemailEvent : Event
	{
		/// <summary>
		/// Gets or sets account id of the callee.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }
	}
}
