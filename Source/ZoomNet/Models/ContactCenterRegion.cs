using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contect Center region.
	/// </summary>
	public class ContactCenterRegion
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("region_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the region's name.</summary>
		[JsonPropertyName("region_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the region SIP zone's ID.</summary>
		[JsonPropertyName("sip_zone_id")]
		public string SipZoneId { get; set; }
	}
}
