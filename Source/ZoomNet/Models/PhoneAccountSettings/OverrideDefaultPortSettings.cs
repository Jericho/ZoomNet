using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to set a port assignment range.
	/// Ports are used by Zoom desktop, mobile clients, Zoom Rooms and Zoom Phone Appliances during a call.
	/// The range should be between 9,000 and 9,999.
	/// At least 50 ports need to be configured to make sure the functionality is not affected.
	/// </summary>
	public class OverrideDefaultPortSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets minimum port number used by Zoom.
		/// </summary>
		[JsonPropertyName("min_port")]
		public int? MinPort { get; set; }

		/// <summary>
		/// Gets or sets maximum port number used by Zoom.
		/// </summary>
		[JsonPropertyName("max_port")]
		public int? MaxPort { get; set; }
	}
}
