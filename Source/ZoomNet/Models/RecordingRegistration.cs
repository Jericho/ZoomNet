using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// A registration for a recording.
	/// </summary>
	public class RecordingRegistration
	{
		/// <summary>Gets or sets the registrant id.</summary>
		/// <value>The registrant id.</value>
		[JsonProperty("registrant_id", NullValueHandling = NullValueHandling.Ignore)]
		public string RegistrantId { get; set; }

		/// <summary>Gets or sets the meeting id.</summary>
		/// <value>The meeting id.</value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long MeetingId { get; set; }

		/// <summary>Gets or sets the meeting topic.</summary>
		/// <value>The topic.</value>
		[JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
		public string Topic { get; set; }

		/// <summary>Gets or sets the URL for the on-demand recording.</summary>
		/// <value>The user id.</value>
		[JsonProperty("share_url", NullValueHandling = NullValueHandling.Ignore)]
		public string ShareUrl { get; set; }
	}
}
