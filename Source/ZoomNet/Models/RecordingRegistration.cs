using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A registration for a recording.</summary>
	public record RecordingRegistration
	{
		/// <summary>Gets or sets the meeting id.</summary>
		[JsonPropertyName("id")]
		public long MeetingId { get; init; }

		/// <summary>Gets or sets the registrant id.</summary>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; init; }

		/// <summary>Gets or sets the URL for the on-demand recording.</summary>
		[JsonPropertyName("share_url")]
		public string ShareUrl { get; init; }

		/// <summary>Gets or sets the meeting topic.</summary>
		[JsonPropertyName("topic")]
		public string Topic { get; init; }
	}
}
