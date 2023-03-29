using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A call log item.
	/// </summary>
	public abstract class CallLog
	{
		/// <summary>
		/// Gets or sets the call Id.
		/// </summary>
		/// <value>The call id.</value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the call answer time.
		/// </summary>
		/// <value>
		/// The call's answer time, in GMT date-time format.
		/// The API only displays this response if the direction value is inbound.
		/// </value>
		[JsonPropertyName("answer_start_time")]
		public DateTime AnswerStartTime { get; set; }

		/// <summary>
		/// Gets or sets the call end time.
		/// </summary>
		/// <value>The call end time, in GMT date-time format.</value>
		[JsonPropertyName("call_end_time")]
		public DateTime CallEndTime { get; set; }

		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>Unique identifier of the phone call.</value>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>
		/// Gets or sets the call type.
		/// </summary>
		/// <value>The type of call: voip | pstn | tollfree | international | contactCenter.</value>
		[JsonPropertyName("call_type")]
		public CallLogCallType? CallType { get; set; }

		/// <summary>
		/// Gets or sets the callee country code.
		/// </summary>
		/// <value>Country calling code.</value>
		[JsonPropertyName("callee_country_code")]
		public string CalleeCountryCode { get; set; }

		/// <summary>
		/// Gets or sets the callee country iso code.
		/// </summary>
		/// <value>ISO alpha2 country code.</value>
		[JsonPropertyName("callee_country_iso_code")]
		public Country? CalleeCountryIsoCode { get; set; }

		/// <summary>
		/// Gets or sets the callee did number.
		/// </summary>
		/// <value>Callee's DID (direct inward dial) number in e164 format.</value>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; set; }

		/// <summary>
		/// Gets or sets the callee name.
		/// </summary>
		/// <value>The callee name.</value>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; set; }

		/// <summary>
		/// Gets or sets the callee number.
		/// </summary>
		/// <value>The callee number.</value>
		[JsonPropertyName("callee_number")]
		public string CalleeNumber { get; set; }

		/// <summary>
		/// Gets or sets the number type of the callee.
		/// </summary>
		/// <value>The callee number type.</value>
		[JsonPropertyName("callee_number_type")]
		public CallLogCalleeNumberType? CalleeNumberType { get; set; }

		/// <summary>
		/// Gets or sets the number source of the callee.
		/// </summary>
		/// <value>Indicates where the phone number comes from: internal | external | byop.</value>
		[JsonPropertyName("callee_number_source")]
		public CallLogNumberSource? CalleeNumberSource { get; set; }

		/// <summary>
		/// Gets or sets the caller country code.
		/// </summary>
		/// <value>Country calling code.</value>
		[JsonPropertyName("caller_country_code")]
		public string CallerCountryCode { get; set; }

		/// <summary>
		/// Gets or sets the caller country iso code.
		/// </summary>
		/// <value>ISO alpha2 country code.</value>
		[JsonPropertyName("caller_country_iso_code")]
		public Country? CallerCountryIsoCode { get; set; }

		/// <summary>
		/// Gets or sets the caller did number.
		/// </summary>
		/// <value>Caller's DID (direct inward dial) number in e164 format.</value>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; set; }

		/// <summary>
		/// Gets or sets the caller name.
		/// </summary>
		/// <value>The caller name.</value>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; set; }

		/// <summary>
		/// Gets or sets the caller number.
		/// </summary>
		/// <value>The caller number.</value>
		[JsonPropertyName("caller_number")]
		public string CallerNumber { get; set; }

		/// <summary>
		/// Gets or sets the number type of the caller.
		/// </summary>
		/// <value>The caller number type.</value>
		[JsonPropertyName("caller_number_type")]
		public CallLogCallerNumberType? CallerNumberType { get; set; }

		/// <summary>
		/// Gets or sets the number source of the caller.
		/// </summary>
		/// <value>Indicates where the phone number comes from: internal | external | byop.</value>
		[JsonPropertyName("caller_number_source")]
		public CallLogNumberSource? CallerNumberSource { get; set; }

		/// <summary>
		/// Gets or sets the billing referenceId of the caller.
		/// </summary>
		/// <value>Billing reference ID (for peering phone numbers).</value>
		[JsonPropertyName("caller_billing_reference_id")]
		public string CallerBillingReferenceId { get; set; }

		/// <summary>
		/// Gets or sets the billing charge.
		/// </summary>
		/// <value>Billing charge for the call.</value>
		[JsonPropertyName("charge")]
		public string Charge { get; set; }

		/// <summary>
		/// Gets or sets the client code.
		/// </summary>
		/// <value>Client code.</value>
		[JsonPropertyName("client_code")]
		public string ClientCode { get; set; }

		/// <summary>
		/// Gets or sets the start time of the call.
		/// </summary>
		/// <value>The Start time of the call.</value>
		[JsonPropertyName("date_time")]
		public DateTime StartedTime { get; set; }

		/// <summary>
		/// Gets or sets the call direction.
		/// </summary>
		/// <value>The call direction. Inbound or Outbound.</value>
		[JsonPropertyName("direction")]
		public CallLogDirection Direction { get; set; }

		/// <summary>
		/// Gets or sets the call duration.
		/// </summary>
		/// <value>The call duration in seconds.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the path of the call log.
		/// </summary>
		/// <value>Path of the call log.</value>
		[JsonPropertyName("path")]
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the rate.
		/// </summary>
		/// <value>Billing rate for the call.</value>
		[JsonPropertyName("rate")]
		public string Rate { get; set; }

		/// <summary>
		/// Gets or sets the recording type.
		/// </summary>
		/// <value>Recording type: 1 - On-demand recording. 2 - Automatic recording.</value>
		[JsonPropertyName("recording_type")]
		public string RecordingType { get; set; }

		/// <summary>
		/// Gets or sets the call result.
		/// </summary>
		/// <value>The Result of the call.</value>
		[JsonPropertyName("result")]
		public CallLogResult Result { get; set; }

		/// <summary>
		/// Gets or sets the site information.
		/// </summary>
		/// <value>Site information.</value>
		[JsonPropertyName("site")]
		public CallLogSite Site { get; set; }

		/// <summary>
		/// Gets or sets the user Id.
		/// </summary>
		/// <value>User ID or user email.</value>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the call hold time.
		/// </summary>
		/// <value>Hold time during a call in seconds.</value>
		[JsonPropertyName("hold_time")]
		public int HoldTime { get; set; }

		/// <summary>
		/// Gets or sets the call waiting time.
		/// </summary>
		/// <value>Waiting time for the call, in seconds.</value>
		[JsonPropertyName("waiting_time")]
		public int WaitingTime { get; set; }

		/// <summary>
		/// Gets or sets the user department.
		/// </summary>
		/// <value>Name of the user's department.</value>
		[JsonPropertyName("department")]
		public string UserDepartment { get; set; }

		/// <summary>
		/// Gets or sets the user cost center.
		/// </summary>
		/// <value>The cost center name to which a user belongs.</value>
		[JsonPropertyName("cost_center")]
		public string UserCostCenter { get; set; }
	}
}
