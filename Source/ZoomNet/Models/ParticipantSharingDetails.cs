using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Sharing and recording details of participants from live or past meetings.</summary>
	public record ParticipantSharingDetails
	{
		/// <summary>Gets or sets the sharing details.</summary>
		[JsonPropertyName("details")]
		public SharingAndRecordingDetail[] SharingAndRecordingDetails { get; init; }

		/// <summary>Gets or sets the Universally unique identifier of the participant.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the participant ID.</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; init; }

		/// <summary>Gets or sets the participant display name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; init; }
	}
}
