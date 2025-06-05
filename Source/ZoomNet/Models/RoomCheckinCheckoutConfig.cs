using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room check-in and check-out config.
	/// </summary>
	public class RoomCheckinCheckoutConfig
	{
		/// <summary>
		/// Gets or sets a value indicating whether to enable check-in and check-out options.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the check-in and check-out options subsettings.
		/// </summary>
		[JsonPropertyName("checkin_and_checkout_options")]
		public RoomCheckinCheckoutOptions Options { get; set; }
	}
}
