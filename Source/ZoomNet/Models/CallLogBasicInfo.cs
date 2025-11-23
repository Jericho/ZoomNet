using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Basic information about call log.
	/// </summary>
	public class CallLogBasicInfo
	{
		/// <summary>
		/// Gets or sets the unique call log id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the unique id of the call.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }
	}
}
