using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Base information about SMS owner common for API endpoints and webhook events.
	/// </summary>
	public abstract class SmsOwnerBase
	{
		/// <summary>
		/// Gets or sets the owner user id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the owner type.
		/// </summary>
		[JsonPropertyName("type")]
		public SmsParticipantOwnerType Type { get; set; }
	}
}
