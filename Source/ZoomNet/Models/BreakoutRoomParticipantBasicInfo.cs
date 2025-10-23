using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Basic information about breakout room participant.
	/// </summary>
	public class BreakoutRoomParticipantBasicInfo
	{
		/// <summary>
		/// Gets or sets the participant's ID.
		/// This is a unique ID assigned to the participant upon joining the meeting and is only valid for that meeting.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the participant's user ID.
		/// This is the same value used in the Users APIs if the participant logged in to join the meeting.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the participant's main meeting user ID (user_id).
		/// </summary>
		[JsonPropertyName("parent_user_id")]
		public string ParentUserId { get;set; }

		/// <summary>
		/// Gets or sets the participant's username.
		/// </summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }
	}
}
