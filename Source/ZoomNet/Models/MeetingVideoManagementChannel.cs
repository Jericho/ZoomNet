using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the waiting room settings for a meeting.</summary>
	public record MeetingVideoManagementChannel
	{
		/// <summary>Gets or sets the id of the video management channel.</summary>
		[JsonPropertyName("channel_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the video management channel.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }
	}
}
