using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents the details of a phone number carrier in the Zoom Phone API.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// <item><term>code</term><description>The unique identifier of the carrier.</description></item>
	/// <item><term>name</term><description>The name of the carrier.</description></item>
	/// </list>
	/// </remarks>
	public class PhoneNumberSipGroup
	{
		/// <summary>
		/// Gets or sets the unique identifier of the SIP group.
		/// </summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the name of the SIP group.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
