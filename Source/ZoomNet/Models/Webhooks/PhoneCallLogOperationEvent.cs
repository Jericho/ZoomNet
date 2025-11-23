using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the phone call log/element/history operation (deleted/completed).
	/// </summary>
	public abstract class PhoneCallLogOperationEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the call log.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets user id of the call log owner.
		/// </summary>
		public string UserId { get; set; }
	}
}
