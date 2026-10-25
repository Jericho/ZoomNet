using System;
using System.Text.Json.Serialization;
using ZoomNet.Utilities;

namespace ZoomNet.Models
{
	/// <summary>Call element record.</summary>
	public record CallElement
	{
		/// <summary>Gets or sets the call answer time.</summary>
		[JsonPropertyName("answer_time")]
		public DateTime? AnswerTime { get; init; }

		/// <summary>Gets or sets the id of the call element.</summary>
		[JsonPropertyName("call_element_id")]
		public string CallElementId { get; init; }

		/// <summary>Gets or sets the call history id.</summary>
		[JsonPropertyName("call_history_uuid")]
		public string CallHistoryUuid { get; init; }

		/// <summary>Gets or sets the unique id of the call.</summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; init; }

		/// <summary>Gets or sets the call path id.</summary>
		[JsonPropertyName("call_path_id")]
		public string CallPathId { get; init; }

		/// <summary>Gets or sets the call type.</summary>
		[JsonPropertyName("call_type")]
		public CallElementCallType? CallType { get; init; }

		/// <summary>Gets or sets the callee account code.</summary>
		[JsonPropertyName("callee_account_code")]
		public string CalleeAccountCode { get; init; }

		/// <summary>Gets or sets the callee cost center name.</summary>
		[JsonPropertyName("callee_cost_center")]
		public string CalleeCostCenter { get; init; }

		/// <summary>Gets or sets the callee country code.</summary>
		[JsonPropertyName("callee_country_code")]
		public string CalleeCountryCode { get; init; }

		/// <summary>Gets or sets the callee country ISO code.</summary>
		[JsonPropertyName("callee_country_iso_code")]
		public Country? CalleeCountryIsoCode { get; init; }

		/// <summary>Gets or sets the callee department name.</summary>
		[JsonPropertyName("callee_department")]
		public string CalleeDepartment { get; init; }

		/// <summary>Gets or sets the callee private IP address.</summary>
		[JsonPropertyName("callee_device_private_ip")]
		public string CalleeDevicePrivateIp { get; init; }

		/// <summary>Gets or sets the callee public IP address.</summary>
		[JsonPropertyName("callee_device_public_ip")]
		public string CalleeDevicePublicIp { get; init; }

		/// <summary>Gets or sets the callee device type.</summary>
		[JsonPropertyName("callee_device_type")]
		public string CalleeDeviceType { get; init; }

		/// <summary>Gets or sets the callee DID (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; init; }

		/// <summary>Gets or sets the callee email.</summary>
		[JsonPropertyName("callee_email")]
		public string CalleeEmail { get; init; }

		/// <summary>Gets or sets the callee employee id.</summary>
		[JsonPropertyName("callee_employee_id")]
		public string CalleeEmployeeId { get; init; }

		/// <summary>Gets or sets the callee extension id.</summary>
		[JsonPropertyName("callee_ext_id")]
		public string CalleeExtensionId { get; init; }

		/// <summary>Gets or sets the callee extension number.</summary>
		[JsonPropertyName("callee_ext_number")]
		public string CalleeExtensionNumber { get; init; }

		/// <summary>Gets or sets the callee extension type.</summary>
		[JsonPropertyName("callee_ext_type")]
		public CallElementExtensionType? CalleeExtensionType { get; init; }

		/// <summary>Gets or sets the callee name.</summary>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; init; }

		/// <summary>Gets or sets the number type of the callee.</summary>
		[JsonPropertyName("callee_number_type")]
		public CallElementNumberType? CalleeNumberType { get; init; }

		/// <summary>Gets or sets the callee site id.</summary>
		[JsonPropertyName("callee_site_id")]
		public string CalleeSiteId { get; init; }

		/// <summary>Gets or sets the caller account code.</summary>
		[JsonPropertyName("caller_account_code")]
		public string CallerAccountCode { get; init; }

		/// <summary>Gets or sets the caller cost center.</summary>
		[JsonPropertyName("caller_cost_center")]
		public string CallerCostCenter { get; init; }

		/// <summary>Gets or sets the caller country code.</summary>
		[JsonPropertyName("caller_country_code")]
		public string CallerCountryCode { get; init; }

		/// <summary>Gets or sets the caller country ISO code.</summary>
		[JsonPropertyName("caller_country_iso_code")]
		public Country? CallerCountryIsoCode { get; init; }

		/// <summary>Gets or sets the caller department name.</summary>
		[JsonPropertyName("caller_department")]
		public string CallerDepartment { get; init; }

		/// <summary>Gets or sets the caller private IP address.</summary>
		[JsonPropertyName("caller_device_private_ip")]
		public string CallerDevicePrivateIp { get; init; }

		/// <summary>Gets or sets the caller public IP address.</summary>
		[JsonPropertyName("caller_device_public_ip")]
		public string CallerDevicePublicIp { get; init; }

		/// <summary>Gets or sets the caller device type.</summary>
		[JsonPropertyName("caller_device_type")]
		public string CallerDeviceType { get; init; }

		/// <summary>Gets or sets the caller DID  (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; init; }

		/// <summary>Gets or sets the caller email.</summary>
		[JsonPropertyName("caller_email")]
		public string CallerEmail { get; init; }

		/// <summary>Gets or sets the caller employee id.</summary>
		[JsonPropertyName("caller_employee_id")]
		public string CallerEmployeeId { get; init; }

		/// <summary>Gets or sets the caller extension id.</summary>
		[JsonPropertyName("caller_ext_id")]
		public string CallerExtensionId { get; init; }

		/// <summary>Gets or sets the caller extension number.</summary>
		[JsonPropertyName("caller_ext_number")]
		public string CallerExtensionNumber { get; init; }

		/// <summary>Gets or sets the caller extension type.</summary>
		[JsonPropertyName("caller_ext_type")]
		public CallElementExtensionType? CallerExtensionType { get; init; }

		/// <summary>Gets or sets the caller name.</summary>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; init; }

		/// <summary>Gets or sets the number type of the caller.</summary>
		[JsonPropertyName("caller_number_type")]
		public CallElementNumberType? CallerNumberType { get; init; }

		/// <summary>Gets or sets the caller site id.</summary>
		[JsonPropertyName("caller_site_id")]
		public string CallerSiteId { get; init; }

		/// <summary>Gets or sets the connect type of the call.</summary>
		[JsonPropertyName("connect_type")]
		public CallElementConnectType? ConnectType { get; init; }

		/// <summary>Gets or sets the call direction (inbound/outbound).</summary>
		[JsonPropertyName("direction")]
		public CallLogDirection? Direction { get; init; }

		/// <summary>Gets or sets the call end time.</summary>
		[JsonPropertyName("end_time")]
		public DateTime? EndTime { get; init; }

		/// <summary>Gets or sets a value indicating whether the call is end-to-end encryption.</summary>
		[JsonPropertyName("end_to_end")]
		public bool? EndToEnd { get; init; }

		/// <summary>Gets or sets an event within a call log.</summary>
		[JsonPropertyName("event")]
		public CallElementEventType? EventType { get; init; }

		/// <summary>Gets or sets user's primary group id.</summary>
		[JsonPropertyName("group_id")]
		public string GroupId { get; init; }

		/// <summary>Gets or sets a value indicating whether hiding caller id is enabled.</summary>
		[JsonPropertyName("hide_caller_id")]
		public bool? HideCallerId { get; init; }

		/// <summary>Gets or sets the call hold time, in seconds.</summary>
		[JsonPropertyName("hold_time")]
		public int? HoldTime { get; init; }

		/// <summary>Gets or sets the id of the call history.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the operator extension id.</summary>
		[JsonPropertyName("operator_ext_id")]
		public string OperatorExtensionId { get; init; }

		/// <summary>Gets or sets the operator extension number.</summary>
		[JsonPropertyName("operator_ext_number")]
		public string OperatorExtensionNumber { get; init; }

		/// <summary>Gets or sets the operator extension type.</summary>
		[JsonPropertyName("operator_ext_type")]
		public CallElementExtensionType? OperatorExtensionType { get; init; }

		/// <summary>Gets or sets the operator name.</summary>
		[JsonPropertyName("operator_name")]
		public string OperatorName { get; init; }

		/// <summary>Gets or sets the unique identifier of the call recording.</summary>
		[JsonPropertyName("recording_id")]
		public string RecordingId { get; init; }

		/// <summary>Gets or sets the recording type.</summary>
		[JsonPropertyName("recording_type")]
		public CallElementRecordingType RecordingType { get; init; }

		/// <summary>Gets or sets the detail result of an event for a call log.</summary>
		/*
		 * "result" when parsing a webhook
		 * "call_results" when parsing the response of an API endpoint.
		 */
		[MultipleValuesEnumMember(DefaultValue = "result", OtherValues = new[] { "call_result" })]
		public CallElementResult Result { get; init; }

		/// <summary>Gets or sets the result reason of an event for a call log.</summary>
		[JsonPropertyName("result_reason")]
		public CallElementResultReason? ResultReason { get; init; }

		/// <summary>Gets or sets the call start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; init; }

		/// <summary>Gets or sets the call talk time, in seconds.</summary>
		[JsonPropertyName("talk_time")]
		public int? TalkTime { get; init; }

		/// <summary>Gets or sets the id of the call voicemail.</summary>
		[JsonPropertyName("voicemail_id")]
		public string VoicemailId { get; init; }

		/// <summary>Gets or sets the call waiting time, in seconds.</summary>
		[JsonPropertyName("wait_time")]
		public int? WaitTime { get; init; }
	}
}
