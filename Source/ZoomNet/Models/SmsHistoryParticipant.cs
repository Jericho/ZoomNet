using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>The user who sent/received the SMS (as provided in API endpoints).</summary>
	public record SmsHistoryParticipant : SmsParticipantBase
	{
		/// <summary>Gets or sets owner.</summary>
		[JsonPropertyName("owner")]
		public SmsParticipantOwner Owner { get; set; }
	}
}
