using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Basic information about phone call recording (only ids).
	/// </summary>
	public class PhoneCallRecordingBasicInfo
	{
		/// <summary>
		/// Gets or sets the call recording ID.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the unique call ID.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }
	}
}
