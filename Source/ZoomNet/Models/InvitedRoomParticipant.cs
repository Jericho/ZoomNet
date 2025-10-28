using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about the participant invited to join a meeting through phone (call out) from a Zoom room.
	/// </summary>
	public class InvitedRoomParticipant
	{
		/// <summary>
		/// Gets or sets the type of call out.
		/// </summary>
		/// <remarks>
		/// Supported values - 'h323' and 'sip'.
		/// </remarks>
		[JsonPropertyName("call_type")]
		public string CallType { get; set; }

		/// <summary>
		/// Gets or sets the user's device IP address.
		/// </summary>
		[JsonPropertyName("device_ip")]
		public string DeviceIp { get; set; }
	}
}
