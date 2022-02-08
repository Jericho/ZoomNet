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

		/// <summary>Gets or sets the total size of the recording files and audio files.</summary>
		/// <value>The total size.</value>
		[JsonProperty(PropertyName = "total_size", NullValueHandling = NullValueHandling.Ignore)]
		public long TotalSize { get; set; }

		/// <summary>Gets or sets the total number of recording files and audio files.</summary>
		/// <value>The number of files.</value>
		[JsonProperty(PropertyName = "recording_count", NullValueHandling = NullValueHandling.Ignore)]
		public long FilesCount { get; set; }

		/// <summary>Gets or sets the recording files.</summary>
		[JsonProperty(PropertyName = "recording_files", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingFile[] RecordingFiles { get; set; }

		/// <summary>Gets or sets the URL which can be used to share the recording.</summary>
		/// <value>The play URL.</value>
		[JsonProperty(PropertyName = "share_url", NullValueHandling = NullValueHandling.Ignore)]
		public string ShareUrl { get; set; }

		/// <summary>Gets or sets the password of the sharing recording file.</summary>
		/// <value>Thepassword.</value>
		[JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
		public string Password { get; set; }

		/// <summary>Gets or sets the audio files for each participant.</summary>
		/// <remarks>This is available only when the "Record a seperate audio file of each participant" setting is enabled.</remarks>
		[JsonProperty(PropertyName = "participant_audio_files", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingFile[] ParticipantAudioFiles { get; set; }
	}
}
