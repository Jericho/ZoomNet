using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location basic profile.
	/// </summary>
	public class RoomLocationBasicProfile
	{
		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		[JsonPropertyName("address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to require code to exit out of your Zoom Rooms application to switch between other apps.
		/// </summary>
		[JsonPropertyName("required_code_to_ext")]
		public bool? CodeIsRequiredToExit { get; set; }

		/// <summary>
		/// Gets or sets the 1-16 digit number or characters used to secure your Zoom Rooms application.
		/// </summary>
		[JsonPropertyName("room_passcode")]
		public string Passcode { get; set; }

		/// <summary>
		/// Gets or sets the email address to be used for reporting Zoom Room issues.
		/// </summary>
		[JsonPropertyName("support_email")]
		public string SupportEmail { get; set; }

		/// <summary>
		/// Gets or sets the phone number to be used for reporting Zoom Room issues.
		/// </summary>
		[JsonPropertyName("support_phone")]
		public string SupportPhone { get; set; }

		/// <summary>
		/// Gets or sets the timezone (only returned for location type - city).
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones? Timezone { get; set; }

		/// <summary>
		/// Gets or sets the parent location id.
		/// </summary>
		[JsonPropertyName("parent_location_id")]
		public string ParentLocationId { get; set; }
	}
}
