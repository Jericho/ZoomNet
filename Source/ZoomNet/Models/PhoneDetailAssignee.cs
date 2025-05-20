using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
    /// <summary>
    /// Represents the details of a phone number assignee in the Zoom Phone API.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <item><term>extension_number</term><description>The extension number of the assignee.</description></item>
    /// <item><term>id</term><description>The unique identifier of the assignee.</description></item>
    /// <item><term>name</term><description>The name of the assignee.</description></item>
    /// <item><term>type</term><description>The type of the assignee, which can be one of the following:
    /// </list>
    /// </remarks>
    public class PhoneDetailAssignee
	{
        /// <summary>
        /// The extension number of the assignee.
        /// </summary>
		[JsonPropertyName("extension_number")]
		public long? ExtensionNumber { get; set; }

		/// <summary>
        /// The unique identifier of the assignee.
        /// </summary>
        [JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
        /// The name of the assignee.
        /// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
        /// The type of the assignee.
        /// </summary>
		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}
