using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call recording.
	/// </summary>
	public class PhoneCallRecording : PhoneCallRecordingBasicInfo
	{
		/// <summary>
		/// Gets or sets the unique call log ID.
		/// </summary>
		[JsonPropertyName("call_log_id")]
		public string CallLogId { get; set; }

		/// <summary>
		/// Gets or sets the callee name.
		/// </summary>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; set; }

		/// <summary>
		/// Gets or sets the callee phone number.
		/// </summary>
		[JsonPropertyName("callee_number")]
		public string CalleeNumber { get; set; }

		/// <summary>
		/// Gets or sets the callee number type.
		/// </summary>
		[JsonPropertyName("callee_number_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingCalleeNumberType? CalleeNumberType { get; set; }

		/// <summary>
		/// Gets or sets the callee account code.
		/// </summary>
		[JsonPropertyName("callee_account_code")]
		public string CalleeAccountCode { get; set; }

		/// <summary>
		/// Gets or sets the callee DID (direct inward dial) number, in E.164 format.
		/// </summary>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; set; }

		/// <summary>
		/// Gets or sets the caller name.
		/// </summary>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; set; }

		/// <summary>
		/// Gets or sets the caller phone number.
		/// </summary>
		[JsonPropertyName("caller_number")]
		public string CallerNumber { get; set; }

		/// <summary>
		/// Gets or sets the caller number type.
		/// </summary>
		[JsonPropertyName("caller_number_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingCallerNumberType? CallerNumberType { get; set; }

		/// <summary>
		/// Gets or sets the caller account code.
		/// </summary>
		[JsonPropertyName("caller_account_code")]
		public string CallerAccountCode { get; set; }

		/// <summary>
		/// Gets or sets the caller DID (direct inward dial) number, in E.164 format.
		/// </summary>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; set; }

		/// <summary>
		/// Gets or sets the call-initiating user.
		/// </summary>
		/// <remarks>
		/// The recording must belong to the initiator and call queue for it to be available.
		/// </remarks>
		[JsonPropertyName("outgoing_by")]
		public PhoneCallUser CallInitiator { get; set; }

		/// <summary>
		/// Gets or sets the call-receiving user.
		/// </summary>
		/// <remarks>
		/// The recording must belong to the receiver and call queue for it to be available.
		/// </remarks>
		[JsonPropertyName("accepted_by")]
		public PhoneCallUser CallReceiver { get; set; }

		/// <summary>
		/// Gets or sets the call recording start datetime.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime StartDateTime { get; set; }

		/// <summary>
		/// Gets or sets the call direction.
		/// </summary>
		[JsonPropertyName("direction")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public CallLogDirection Direction { get; set; }

		/// <summary>
		/// Gets or sets the download URL for the call recording.
		/// </summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the download URL for the call recording transcript.
		/// </summary>
		[JsonPropertyName("transcript_download_url")]
		public string TranscriptDownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the call duration, in seconds.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the call recording end datetime.
		/// </summary>
		[JsonPropertyName("end_time")]
		public DateTime EndDateTime { get; set; }

		/// <summary>
		/// Gets or sets the call recording owner.
		/// </summary>
		[JsonPropertyName("owner")]
		public PhoneCallRecordingOwner Owner { get; set; }

		/// <summary>
		/// Gets or sets the file URL for the call recording.
		/// </summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("file_url")]
		public string FileUrl { get; set; }

		/// <summary>
		/// Gets or sets the call recording type.
		/// </summary>
		[JsonPropertyName("recording_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingType? Type { get; set; }

		/// <summary>
		/// Gets or sets the call recording site information.
		/// </summary>
		[JsonPropertyName("site")]
		public CallLogSite Site { get; set; }

		/// <summary>
		/// Gets or sets the disclaimer status.
		/// </summary>
		[JsonPropertyName("disclaimer_status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public PhoneCallRecordingDisclaimerStatus DisclaimerStatus { get; set; }

		/// <summary>
		/// Gets or sets the call element id.
		/// </summary>
		[JsonPropertyName("call_element_id")]
		public string CallElementId { get; set; }

		/// <summary>
		/// Gets or sets the call history id.
		/// </summary>
		[JsonPropertyName("call_history_id")]
		public string CallHistoryId { get; set; }

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the channel id.
		/// </summary>
		[JsonPropertyName("channel_id")]
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets id of the SIP call.
		/// </summary>
		[JsonPropertyName("sip_id")]
		public string SipId { get; set; }
	}
}
