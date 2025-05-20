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
    public class PhoneDetailCarrier
	{
        /// <summary>
        /// The unique identifier of the carrier.
        /// </summary>
		[JsonPropertyName("code")]
		public int? Code { get; set; }

        /// <summary>
        /// The name of the carrier.
        /// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
