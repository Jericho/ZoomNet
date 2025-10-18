using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact Center system status.
	/// </summary>
	public abstract class ContactCenterSystemStatus
	{
		/// <summary>Gets or sets the category.</summary>
		[JsonPropertyName("status_category")]
		public ContactCenterAgentStatusCategory Category { get; set; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("status_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the status name.</summary>
		[JsonPropertyName("status_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the status type.</summary>
		[JsonPropertyName("status_type")]
		public ContactCenterAgentStatusType Type { get; set; }
	}
}
