using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Participant.
	/// </summary>
	public class Participant
	{
		/// <summary>
		/// Gets or sets the participant uuid.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the participant's email address.
		/// </summary>
		[JsonProperty(PropertyName = "user_email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the participant's display name.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Sets the participant's email address.
		/// </summary>
		/// <remarks>
		/// The participant-related webhooks use "email" in their JSON payload instead of "user_email".
		/// </remarks>
		[JsonProperty(PropertyName = "email")]
		private string ParticipantEmail { set { Email = value; } }

		/// <summary>
		/// Sets the participant's display name.
		/// </summary>
		/// <remarks>
		/// The participant-related webhooks use "user_name" in their JSON payload instead of "name".
		/// </remarks>
		[JsonProperty(PropertyName = "user_name")]
		private string ParticipantDisplayName { set { DisplayName = value; } }
	}
}
