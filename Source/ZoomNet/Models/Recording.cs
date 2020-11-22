using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// A recording.
	/// </summary>
	public class Recording
	{
		/// <summary>Gets or sets the unique id.</summary>
		/// <value>The unique id.</value>
		[JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>Gets or sets the recording id.</summary>
		/// <value>The id.</value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>Gets or sets the ID of the user account.</summary>
		/// <value>The account id.</value>
		[JsonProperty("account_id", NullValueHandling = NullValueHandling.Ignore)]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the ID of the user who is set as the host of the meeting.</summary>
		/// <value>The user id.</value>
		[JsonProperty("host_id", NullValueHandling = NullValueHandling.Ignore)]
		public string HostId { get; set; }

		/// <summary>Gets or sets the topic of the meeting.</summary>
		/// <value>The topic.</value>
		[JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
		public string Topic { get; set; }

		/// <summary>Gets or sets the date and time when the meeting started.</summary>
		/// <value>The start time.</value>
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the meeting duration.</summary>
		/// <value> The duration.</value>
		[JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
		public long Duration { get; set; }

		/// <summary>Gets or sets the total size of the recording.</summary>
		/// <value>The total size.</value>
		[JsonProperty(PropertyName = "total_size", NullValueHandling = NullValueHandling.Ignore)]
		public long TotalSize { get; set; }

		/// <summary>Gets or sets the number of files.</summary>
		/// <value>The number of files.</value>
		[JsonProperty(PropertyName = "recording_count", NullValueHandling = NullValueHandling.Ignore)]
		public long FilesCount { get; set; }

		/// <summary>Gets or sets the recording files.</summary>
		[JsonProperty(PropertyName = "recording_files", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingFile[] RecordingFiles { get; set; }
	}
}
