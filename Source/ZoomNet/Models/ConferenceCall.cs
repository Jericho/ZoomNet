using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about conference call.
	/// </summary>
	public class ConferenceCall
	{
		/// <summary>
		/// Gets or sets the conference id.
		/// </summary>
		/// <remarks>
		/// No conference id is provided if <see cref="EnableMultiplePartyConference"/> is false.
		/// </remarks>
		[JsonPropertyName("conference_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the call id.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the conference call started.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime StartedOn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to enable multiple parties to join the conference call.
		/// </summary>
		[JsonPropertyName("enable_multiple_party_conference")]
		public bool EnableMultiplePartyConference { get; set; }

		/// <summary>
		/// Gets or sets the reason a conference call failed to start.
		/// </summary>
		/// <remarks>
		/// Empty string means success.
		/// </remarks>
		[JsonPropertyName("failure_reason")]
		public string FailureReason { get; set; }

		/// <summary>
		/// Gets or sets the call owner.
		/// </summary>
		[JsonPropertyName("owner")]
		public CallLogOwnerInfo Owner { get; set; }
	}
}
