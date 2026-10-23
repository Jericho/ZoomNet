using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the video management settings for a meeting.</summary>
	public class MeetingVideoManagementSettings
	{
		/// <summary>Gets or sets a value indicating whether the video management is enabled.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the channels for the video management.</summary>
		[JsonPropertyName("channels")]
		public MeetingVideoManagementChannel[] Channels { get; set; }
	}
}
