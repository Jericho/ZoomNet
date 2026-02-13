using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Call history.
	/// </summary>
	public class CallHistory
	{
		/// <summary>Gets or sets the call answer time.</summary>
		[JsonPropertyName("answer_time")]
		public DateTime AnswerTime { get; set; }

		/// <summary>Gets or sets the call elements.</summary>
		[JsonPropertyName("call_elements")]
		public CallElement[] CallElements { get; set; }

		/// <summary>Gets or sets the call history id.</summary>
		[JsonPropertyName("call_history_uuid")]
		public string Uuid { get; set; }

		/// <summary>Gets or sets the unique id of the call.</summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>Gets or sets the call path.</summary>
		[JsonPropertyName("call_path")]
		/*
		 * The properties of the call_path nodes match the properties of the CallElement model class with four exceptions:
		 *   - CallElement has a "call_element_id" property which is not in call_path
		 *   - call_path has three additional properties: group_id, hide_caller_id and id
		 * I am making the conscious decision to ignore these four discrepencies in order to avoid creating yet another
		 * model class which would be 99% similar to the CallElement model class.
		 *
		 * It's not clear to me what the distinction between "call path" and "call elements" is.
		 */
		public CallElement[] CallPath { get; set; }

		/// <summary>Gets or sets the call type.</summary>
		[JsonPropertyName("call_type")]
		public CallElementCallType CallType { get; set; }

		/// <summary>Gets or sets the callee account code.</summary>
		[JsonPropertyName("callee_account_code")]
		public string CalleeAccountCode { get; set; }

		/// <summary>Gets or sets the callee DID (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("callee_did_number")]
		public string CalleeDidNumber { get; set; }

		/// <summary>Gets or sets the callee email.</summary>
		[JsonPropertyName("callee_email")]
		public string CalleeEmail { get; set; }

		/// <summary>Gets or sets the callee extension id.</summary>
		[JsonPropertyName("callee_ext_id")]
		public string CalleeExtensionId { get; set; }

		/// <summary>Gets or sets the callee extension number.</summary>
		[JsonPropertyName("callee_ext_number")]
		public string CalleeExtensionNumber { get; set; }

		/// <summary>Gets or sets the callee extension type.</summary>
		[JsonPropertyName("callee_ext_type")]
		public CallElementExtensionType? CalleeExtensionType { get; set; }

		/// <summary>Gets or sets the callee name.</summary>
		[JsonPropertyName("callee_name")]
		public string CalleeName { get; set; }

		/// <summary>Gets or sets the caller account code.</summary>
		[JsonPropertyName("caller_account_code")]
		public string CallerAccountCode { get; set; }

		/// <summary>Gets or sets the caller DID  (direct inward dial) number, in E.164 format.</summary>
		[JsonPropertyName("caller_did_number")]
		public string CallerDidNumber { get; set; }

		/// <summary>Gets or sets the caller email.</summary>
		[JsonPropertyName("caller_email")]
		public string CallerEmail { get; set; }

		/// <summary>Gets or sets the caller extension id.</summary>
		[JsonPropertyName("caller_ext_id")]
		public string CallerExtensionId { get; set; }

		/// <summary>Gets or sets the caller extension number.</summary>
		[JsonPropertyName("caller_ext_number")]
		public string CallerExtensionNumber { get; set; }

		/// <summary>Gets or sets the caller extension type.</summary>
		[JsonPropertyName("caller_ext_type")]
		public CallElementExtensionType? CallerExtensionType { get; set; }

		/// <summary>Gets or sets the caller name.</summary>
		[JsonPropertyName("caller_name")]
		public string CallerName { get; set; }

		/// <summary>Gets or sets the connect type of the call.</summary>
		[JsonPropertyName("connect_type")]
		public CallElementConnectType? ConnectType { get; set; }

		/// <summary>Gets or sets the name of the cost center of which the user belongs.</summary>
		[JsonPropertyName("cost_center")]
		public string CostCenter { get; set; }

		/// <summary>Gets or sets the name of the department of which the user belongs.</summary>
		[JsonPropertyName("department")]
		public string Department { get; set; }

		/// <summary>Gets or sets the call direction (inbound/outbound).</summary>
		[JsonPropertyName("direction")]
		public CallLogDirection Direction { get; set; }

		/// <summary>Gets or sets the call end time.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; set; }

		/// <summary>Gets or sets user's primary group id.</summary>
		[JsonPropertyName("group_id")]
		public string GroupId { get; set; }

		/// <summary>Gets or sets a value indicating whether hiding caller id is enabled.</summary>
		[JsonPropertyName("hide_caller_id")]
		public bool HideCallerId { get; set; }

		/// <summary>Gets or sets the unique id of the call log summary.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets a value indicating whether the call is international or not.</summary>
		[JsonPropertyName("international")]
		public bool IsInternational { get; set; }

		/// <summary>Gets or sets the unique identifier of the site the user belongs to.</summary>
		[JsonPropertyName("site_id")]
		public string SiteId { get; set; }

		/// <summary>Gets or sets the bame of the site the user belongs to.</summary>
		[JsonPropertyName("site_name")]
		public string SiteName { get; set; }

		/// <summary>Gets or sets the call start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }
	}
}
