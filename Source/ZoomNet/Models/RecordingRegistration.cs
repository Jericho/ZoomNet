using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A registration for a recording.
	/// </summary>
	public class RecordingRegistration
	{
		/// <summary>Gets or sets the registrant id.</summary>
		/// <value>The registrant id.</value>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }

		/// <summary>Gets or sets the meeting id.</summary>
		/// <value>The meeting id.</value>
		[JsonPropertyName("id")]
		public long MeetingId { get; set; }

		/// <summary>Gets or sets the meeting topic.</summary>
		/// <value>The topic.</value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>Gets or sets the URL for the on-demand recording.</summary>
		/// <value>The user id.</value>
		[JsonPropertyName("share_url")]
		public string ShareUrl { get; set; }
	}
}
