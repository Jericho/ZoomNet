using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	public class CallQueueMember
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("level")]
		public string Level { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("receive_call")]
		public bool ReceiveCall { get; set; }

		[JsonPropertyName("extension_id")]
		public string ExtensionId { get; set; }
	}
}
