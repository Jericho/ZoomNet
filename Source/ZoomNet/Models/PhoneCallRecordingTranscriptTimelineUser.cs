using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call recording transcript timeline speaking user.
	/// </summary>
	/// <remarks>Not documented by Zoom.</remarks>
	public class PhoneCallRecordingTranscriptTimelineUser
	{
		/// <summary>Gets or sets the username.</summary>
		/// <value>The user name.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("username")]
		public string Username { get; set; }

		/// <summary>Gets or sets a value indicating whether multiple people are speaking.</summary>
		/// <value>Whether multiple people are speaking.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("multiple_people")]
		public bool IsMultiplePeople { get; set; }

		/// <summary>Gets or sets the user ID.</summary>
		/// <value>The user ID.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>Gets or sets the Zoom user ID.</summary>
		/// <value>The Zoom user ID.</value>
		/// <remarks>Not documented by Zoom.</remarks>
		[JsonPropertyName("zoom_userid")]
		public string ZoomUserId { get; set; }

		/// <summary>Gets or sets the client type.</summary>
		/// <value>The client type.</value>
		/// <remarks>
		/// Not documented by Zoom.<br/>
		/// Most likely is actually an enumerator.
		/// </remarks>
		[JsonPropertyName("client_type")]
		public int ClientType { get; set; }
	}
}
