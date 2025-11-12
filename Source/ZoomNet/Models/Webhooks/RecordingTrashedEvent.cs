using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user initially deletes a cloud recording.
	/// </summary>
	public class RecordingTrashedEvent : RecordingFilesEvent
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
		/// Gets or sets the email address of the user who deleted the recording to the trash.
		/// </summary>
		[JsonPropertyName("operator")]
		public string Operator { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who deleted the recording to the trash.
		/// </summary>
		[JsonPropertyName("operator_id")]
		public string OperatorId { get; set; }
	}
}
