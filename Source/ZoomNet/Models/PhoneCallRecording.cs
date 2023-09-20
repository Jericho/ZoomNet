using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call recording.
	/// </summary>
	public class PhoneCallRecording
	{
		/// <summary>Gets or sets the call ID.</summary>
		/// <value>The phone call's unique ID.</value>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>Gets or sets the call log ID.</summary>
		/// <value>The phone call log's unique ID.</value>
		[JsonPropertyName("call_log_id")]
		public string CallLogId { get; set; }

		/// <summary>Gets or sets the callee name.</summary>
		/// <value>The callee's contact name.</value>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; set; }

		/// <summary>Gets or sets the callee number.</summary>
		/// <value>The callee's phone number.</value>
		[JsonPropertyName("callee_number")]
		public string CalleeNumber { get; set; }

		/// <summary>Gets or sets the callee number type.</summary>
		/// <value>The callee's number type.</value>
		[JsonPropertyName("callee_number_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingCalleeNumberType? CalleeNumberType { get; set; }

		/// <summary>Gets or sets the caller name.</summary>
		/// <value>The caller's contact name.</value>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; set; }

		/// <summary>Gets or sets the caller number.</summary>
		/// <value>The caller;s phone number.</value>
		[JsonPropertyName("caller_number")]
		public string CallerNumber { get; set; }

		/// <summary>Gets or sets the caller number type.</summary>
		/// <value>The caller's number type.</value>
		[JsonPropertyName("caller_number_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingCallerNumberType? CallerNumberType { get; set; }

		/// <summary>Gets or sets the call-initiating user.</summary>
		/// <value>The call-initiating user.</value>
		/// <remarks>The recording must belong to the initiator and call queue for it to be available.</remarks>
		[JsonPropertyName("outgoing_by")]
		public PhoneCallUser CallInitiator { get; set; }

		/// <summary>Gets or sets the call-receiving user.</summary>
		/// <value>The call-initiating user.</value>
		/// <remarks>The recording must belong to the receiver and call queue for it to be available.</remarks>
		[JsonPropertyName("accepted_by")]
		public PhoneCallUser CallReceiver { get; set; }

		/// <summary>Gets or sets the call recording start datetime.</summary>
		/// <value>The date and time at which the recording was received.</value>
		[JsonPropertyName("date_time")]
		public DateTime StartDateTime { get; set; }

		/// <summary>Gets or sets the call direction.</summary>
		/// <value>The call's direction (inbound / outbound).</value>
		[JsonPropertyName("direction")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public CallLogDirection Direction { get; set; }

		/// <summary>Gets or sets the download URL for the call recording.</summary>
		/// <value>The URL from which to download the recording
		/// (with no OAuth access token).</value>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the call duration.</summary>
		/// <value>The call recording's duration, in seconds.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>Gets or sets the call recording end datetime.</summary>
		/// <value>The recording's end time.</value>
		[JsonPropertyName("end_time")]
		public DateTime EndDateTime { get; set; }

		/// <summary>Gets or sets the call recording ID.</summary>
		/// <value>The recording's ID.</value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the call recording owner.</summary>
		/// <value>The owner of the recording.</value>
		[JsonPropertyName("owner")]
		public PhoneCallRecordingOwner Owner { get; set; }

		/// <summary>Gets or sets the file URL for the call recording.</summary>
		/// <value>The URL from which to download the recording
		/// (with OAuth access token included as query parameter).</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("file_url")]
		public string FileUrl { get; set; }

		/// <summary>Gets or sets the call recording type.</summary>
		/// <value>Type of the call recording.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("recording_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingType? Type { get; set; }

		/// <summary>Gets or sets the call recording site information.</summary>
		/// <value>Site information.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("site")]
		public CallLogSite Site { get; set; }

		/// <summary>Gets or sets the disclaimer status.</summary>
		/// <value>The status of disclaimer for recording.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("disclaimer_status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingDisclaimerStatus DisclaimerStatus { get; set; }
	}
}
