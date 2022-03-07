using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics of a participant.
	/// </summary>
	public class ReportMeetingParticipant : ReportParticipant
	{
		/// <summary>
		/// Gets or sets the RegistrantID of the participant.
		/// </summary>
		/// <value>
		/// The RegistrantID of the participant. Only returned if registrant_id is included in the include_fields query parameter.
		/// </value>
		[JsonPropertyName("registrant_id")]
		public string RegistrantId { get; set; }
	}
}
