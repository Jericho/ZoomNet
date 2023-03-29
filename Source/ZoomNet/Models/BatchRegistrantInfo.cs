using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Batch registrant Info.
	/// </summary>
	public class BatchRegistrantInfo
	{
		/// <summary>
		/// Gets or sets the registrant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("registrant_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets registant's email.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the meeting or webinar.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets she participant PIN code. Which is used to authenticate audio participants before they join the meeting.
		/// </summary>
		[JsonPropertyName("participant_pin_code")]
		public int ParticipantPinCode { get; set; }
	}
}
