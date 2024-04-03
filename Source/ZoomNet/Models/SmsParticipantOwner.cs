using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS history owner.
	/// </summary>
	public class SmsParticipantOwner
	{
		/// <summary>
		/// Gets or sets the owner ID.
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
