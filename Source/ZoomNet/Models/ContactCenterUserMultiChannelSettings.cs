using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the setting's multi-channel engagements for a Contact Center user.
	/// </summary>
	public class ContactCenterUserMultiChannelSettings
	{
		/// <summary>Gets or sets the maximum load percentage that a user needs to take in order to receive voice and video calls.</summary>
		[JsonPropertyName("email_max_agent_load")]
		public int MaxEmailLoadPercentage { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow users to receive voice or video engagements while handling chat and SMS engagements, based on the max_agent_load value.</summary>
		[JsonPropertyName("enabled")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the maximum load percentage that a user needs to take in order to receive voice and video calls.</summary>
		[JsonPropertyName("max_agent_load")]
		public int MaxLoadPercentage { get; set; }
	}
}
