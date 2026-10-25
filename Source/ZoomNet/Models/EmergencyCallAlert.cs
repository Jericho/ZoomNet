using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Emergency call alert information as provided in <see cref="Webhooks.PhoneEmergencyAlertEvent"/>.</summary>
	public record EmergencyCallAlert
	{
		/// <summary>Gets or sets the call id.</summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; init; }

		/// <summary>Gets or sets the callee information.</summary>
		[JsonPropertyName("callee")]
		public EmergencyCallParty Callee { get; init; }

		/// <summary>Gets or sets the caller information.</summary>
		[JsonPropertyName("caller")]
		public EmergencyCallParty Caller { get; init; }

		/// <summary>Gets or sets the call destination.</summary>
		[JsonPropertyName("deliver_to")]
		public EmergencyCallDestination DeliverTo { get; init; }

		/// <summary>Gets or sets the emergency address.</summary>
		[JsonPropertyName("emergency_address")]
		public EmergencyAddress EmergencyAddress { get; init; }

		/// <summary>Gets or sets caller device location.</summary>
		[JsonPropertyName("location")]
		public DeviceLocation Location { get; init; }

		/// <summary>Gets or sets the date and time when the ringing started.</summary>
		[JsonPropertyName("ringing_start_time")]
		public DateTime RingingStartedOn { get; init; }

		/// <summary>Gets or sets the call source.</summary>
		[JsonPropertyName("router")]
		public EmergencyCallSource Router { get; init; }
	}
}
