using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Base information about voicemail common for API endpoints and webhook events.</summary>
	public abstract record VoicemailBase : VoicemailBasicInfo
	{
		/// <summary>Gets or sets call element id.</summary>
		[JsonPropertyName("call_element_id")]
		public string CallElementId { get; init; }

		/// <summary>Gets or sets call history id.</summary>
		[JsonPropertyName("call_history_id")]
		public string CallHistoryId { get; init; }

		/// <summary>Gets or sets unique call id.</summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; init; }

		/// <summary>Gets or sets unique call log id.</summary>
		[JsonPropertyName("call_log_id")]
		public string CallLogId { get; init; }

		/// <summary>Gets or sets callee's account code.</summary>
		[JsonPropertyName("callee_account_code")]
		public string CalleeAccountCode { get; init; }

		/// <summary>Gets or sets callee's name.</summary>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; init; }

		/// <summary>Gets or sets callee's phone number.</summary>
		[JsonPropertyName("callee_number")]
		public string CalleeNumber { get; init; }

		/// <summary>Gets or sets callee's number type.</summary>
		[JsonPropertyName("callee_number_type")]
		public PhoneCallNumberType CalleeNumberType { get; init; }

		/// <summary>Gets or sets caller's account code.</summary>
		[JsonPropertyName("caller_account_code")]
		public string CallerAccountCode { get; init; }

		/// <summary>Gets or sets caller's name.</summary>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; init; }

		/// <summary>Gets or sets caller's number.</summary>
		[JsonPropertyName("caller_number")]
		public string CallerNumber { get; init; }

		/// <summary>Gets or sets caller's number type.</summary>
		[JsonPropertyName("caller_number_type")]
		public PhoneCallNumberType CallerNumberType { get; init; }

		/// <summary>Gets or sets date and time when voicemail was created.</summary>
		[JsonPropertyName("date_time")]
		public DateTime CreatedOn { get; init; }

		/// <summary>Gets or sets url which can be used to download the voicemail.</summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; init; }

		/// <summary>Gets or sets voicemail duration, in seconds.</summary>
		[JsonPropertyName("duration")]
		public int? Duration { get; init; }

		/// <summary>Gets or sets the voicemail transcript.</summary>
		[JsonPropertyName("transcription")]
		public VoicemailTranscript Transcription { get; init; }
	}
}
