using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A call log item.</summary>
	public abstract record CallLog : CallLogBasicInfo
	{
		/// <summary>Gets or sets the call answer time.</summary>
		[JsonPropertyName("answer_start_time")]
		public DateTime? AnswerStartTime { get; init; }

		/// <summary>Gets or sets the call end time.</summary>
		[JsonPropertyName("call_end_time")]
		public DateTime? CallEndTime { get; init; }

		/// <summary>Gets or sets the call type.</summary>
		[JsonPropertyName("call_type")]
		public CallLogCallType? CallType { get; init; }

		/// <summary>Gets or sets the callee country code.</summary>
		[JsonPropertyName("callee_country_code")]
		public string CalleeCountryCode { get; init; }

		/// <summary>Gets or sets the callee country ISO code.</summary>
		[JsonPropertyName("callee_country_iso_code")]
		public Country? CalleeCountryIsoCode { get; init; }

		/// <summary>Gets or sets the callee DID (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; init; }

		/// <summary>Gets or sets the callee location.</summary>
		[JsonPropertyName("callee_location")]
		public string CalleeLocation { get; init; }

		/// <summary>Gets or sets the callee name.</summary>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; init; }

		/// <summary>Gets or sets the callee number.</summary>
		[JsonPropertyName("callee_number")]
		public string CalleeNumber { get; init; }

		/// <summary>Gets or sets the number source of the callee.</summary>
		[JsonPropertyName("callee_number_source")]
		public CallLogNumberSource? CalleeNumberSource { get; init; }

		/// <summary>Gets or sets the number type of the callee.</summary>
		[JsonPropertyName("callee_number_type")]
		public CallLogCalleeNumberType? CalleeNumberType { get; init; }

		/// <summary>Gets or sets the callee user id.</summary>
		[JsonPropertyName("callee_user_id")]
		public string CalleeUserId { get; init; }

		/// <summary>Gets or sets the billing reference i of the caller (for peering phone numbers).</summary>
		[JsonPropertyName("caller_billing_reference_id")]
		public string CallerBillingReferenceId { get; init; }

		/// <summary>Gets or sets the caller country code.</summary>
		[JsonPropertyName("caller_country_code")]
		public string CallerCountryCode { get; init; }

		/// <summary>Gets or sets the caller country ISO code.</summary>
		[JsonPropertyName("caller_country_iso_code")]
		public Country? CallerCountryIsoCode { get; init; }

		/// <summary>Gets or sets the caller DID  (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; init; }

		/// <summary>Gets or sets the caller location.</summary>
		[JsonPropertyName("caller_location")]
		public string CallerLocation { get; init; }

		/// <summary>Gets or sets the caller name.</summary>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; init; }

		/// <summary>Gets or sets the caller number.</summary>
		[JsonPropertyName("caller_number")]
		public string CallerNumber { get; init; }

		/// <summary>Gets or sets the number source of the caller.</summary>
		[JsonPropertyName("caller_number_source")]
		public CallLogNumberSource? CallerNumberSource { get; init; }

		/// <summary>Gets or sets the number type of the caller.</summary>
		[JsonPropertyName("caller_number_type")]
		public CallLogCallerNumberType? CallerNumberType { get; init; }

		/// <summary>Gets or sets the caller user id.</summary>
		[JsonPropertyName("caller_user_id")]
		public string CallerUserId { get; init; }

		/// <summary>Gets or sets the billing charge for the call.</summary>
		[JsonPropertyName("charge")]
		public string Charge { get; init; }

		/// <summary>Gets or sets the client code.</summary>
		[JsonPropertyName("client_code")]
		public string ClientCode { get; init; }

		/// <summary>Gets or sets the user's cost center name.</summary>
		[JsonPropertyName("cost_center")]
		public string UserCostCenter { get; init; }

		/// <summary>Gets or sets the start time of the call.</summary>
		[JsonPropertyName("date_time")]
		public DateTime StartedTime { get; init; }

		/// <summary>Gets or sets the user's department name.</summary>
		[JsonPropertyName("department")]
		public string UserDepartment { get; init; }

		/// <summary>Gets or sets the call direction (inbound/outbound).</summary>
		[JsonPropertyName("direction")]
		public CallLogDirection? Direction { get; init; }

		/// <summary>Gets or sets the call duration, in seconds.</summary>
		[JsonPropertyName("duration")]
		public int? Duration { get; init; }

		/// <summary>Gets or sets a value indicating whether the call log includes recording.</summary>
		[JsonPropertyName("has_recording")]
		public bool? HasRecording { get; init; }

		/// <summary>Gets or sets the call hold time, in seconds.</summary>
		[JsonPropertyName("hold_time")]
		public int? HoldTime { get; init; }

		/// <summary>Gets or sets the path of the call log.</summary>
		[JsonPropertyName("path")]
		public string Path { get; init; }

		/// <summary>Gets or sets the billing rate for the call.</summary>
		[JsonPropertyName("rate")]
		public string Rate { get; init; }

		/// <summary>Gets or sets the unique identifier of the call recording.</summary>
		[JsonPropertyName("recording_id")]
		public string RecordingId { get; init; }

		/// <summary>Gets or sets the recording type.</summary>
		[JsonPropertyName("recording_type")]
		public PhoneCallRecordingType? RecordingType { get; init; }

		/// <summary>Gets or sets the call result.</summary>
		[JsonPropertyName("result")]
		public CallLogResult Result { get; init; }

		/// <summary>Gets or sets the site information.</summary>
		[JsonPropertyName("site")]
		public CallLogSite Site { get; init; }

		/// <summary>Gets or sets the user id (or email).</summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; init; }

		/// <summary>Gets or sets the call waiting time, in seconds.</summary>
		[JsonPropertyName("waiting_time")]
		public int? WaitingTime { get; init; }
	}
}
