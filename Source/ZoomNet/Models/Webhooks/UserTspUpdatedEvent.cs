using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user's TSP account information is updated.
	/// </summary>
	public class UserTspUpdatedEvent : UserTspEvent
	{
		/// <summary>
		/// Gets or sets original information about user's TSP account.
		/// </summary>
		[JsonPropertyName("old_object")]
		public TspAccount OldAccount { get; set; }
	}
}
