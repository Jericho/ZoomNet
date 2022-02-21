using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Sharing and recording details of participants from live or past meetings.
	/// </summary>
	public class ParticipantSharingDetails
	{
		/// <summary>
		/// Gets or sets the Universally unique identifier of the participant.
		/// </summary>
		/// <value>
		/// The Universally unique identifier of the participant.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the participant ID.
		/// </summary>
		/// <value>
		/// The participant ID.
		/// </value>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant display name.
		/// </summary>
		/// <value>
		/// The participant display name.
		/// </value>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the sharing details.
		/// </summary>
		/// <value>
		/// An array of sharing and recording details.
		/// </value>
		[JsonPropertyName("details")]
		public SharingAndRecordingDetail[] SharingAndRecordingDetails { get; set; }
	}
}
