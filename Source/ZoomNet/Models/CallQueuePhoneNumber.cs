using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a phone number assigned to a call queue.</summary>
	public class CallQueuePhoneNumber
	{
		/// <summary>Gets or sets the unique identifier of the phone number.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("number")]
		public string Number { get; set; }

		/// <summary>Gets or sets the source of the phone number.</summary>
		[JsonPropertyName("source")]
		public CallQueueNumberSource Source { get; set; }
	}
}
