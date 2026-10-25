using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A Call Queue.</summary>
	public record CallQueue
	{
		/// <summary>Gets or sets the extension id.</summary>
		[JsonPropertyName("extension_id")]
		public string ExtensionId { get; init; }

		/// <summary>Gets or sets the extension number.</summary>
		[JsonPropertyName("extension_number")]
		public long? ExtensionNumber { get; init; }

		/// <summary>Gets or sets the unique identifier of the call queue.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the call queue.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the phone numbers assigned to the call queue.</summary>
		[JsonPropertyName("phone_numbers")]
		public List<CallQueuePhoneNumber> PhoneNumbers { get; init; }

		/// <summary>Gets or sets the site information.</summary>
		[JsonPropertyName("site")]
		public Site Site { get; init; }

		/// <summary>Gets or sets the status of the call queue.</summary>
		[JsonPropertyName("status")]
		public CallQueueStatus Status { get; init; }
	}
}
