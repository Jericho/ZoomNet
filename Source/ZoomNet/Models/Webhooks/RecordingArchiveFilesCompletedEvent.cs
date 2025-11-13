using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time the archived files of a meeting or webinar become available to download.
	/// </summary>
	public class RecordingArchiveFilesCompletedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets authentication token to use to retrieve the recording.
		/// </summary>
		/// <remarks>
		/// Use the generated token value with the download_url value to download
		/// the cloud recording via an OAuth app.
		/// The download token only lasts for 24 hours after its creation,
		/// and you can only download the file within 24 hours of receiving the
		/// event notification. Pass download_token as a bearer token in
		/// the authorization header of your HTTP request.
		/// </remarks>
		public string DownloadToken { get; set; }

		/// <summary>
		/// Gets or sets information about the meeting or webinar recording.
		/// </summary>
		[JsonPropertyName("object")]
		public RecordingArchive Recording { get; set; }
	}
}
