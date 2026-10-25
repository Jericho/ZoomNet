using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Phone call recording transcript timeline speaking user.</summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public record PhoneCallRecordingTranscriptTimelineUser
	{
		/// <summary>Gets or sets the client type.</summary>
		/// <remarks>Not documented by Zoom.<br/> Most likely is actually an enumerator.</remarks>
		[JsonPropertyName("client_type")]
		public int ClientType { get; init; }

		/// <summary>Gets or sets a value indicating whether multiple people are speaking.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("multiple_people")]
		public bool IsMultiplePeople { get; init; }

		/// <summary>Gets or sets the user ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("user_id")]
		public string UserId { get; init; }

		/// <summary>Gets or sets the username.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("username")]
		public string Username { get; init; }

		/// <summary>Gets or sets the Zoom user ID.</summary>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("zoom_userid")]
		public string ZoomUserId { get; init; }
	}
}
