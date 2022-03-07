using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Ip address access control settings.
	/// </summary>
	public class IpAddressAccessControlSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the cloud recordings of this account can only be accessed by the IP addresses defined in the <see cref="IpAddressesAndRanges"/> property.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the IP addresses or ranges that have access to the cloud recordings.</summary>
		/// <remarks>Separate multiple IP ranges with comma. Use n.n.n.n, n.n.n.n/n or n.n.n.n - n.n.n.n syntax where n is a number.</remarks>
		[JsonPropertyName("ip_addresses_or_ranges")]
		public string IpAddressesAndRanges { get; set; }
	}
}
