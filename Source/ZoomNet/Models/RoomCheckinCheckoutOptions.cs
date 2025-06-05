using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room check-in and check-out options.
	/// </summary>
	public class RoomCheckinCheckoutOptions
	{
		/// <summary>
		/// Gets or sets the number of minutes participants are allowed to check in prior to the meeting start time.
		/// </summary>
		[JsonPropertyName("checkin_minutes_prior_to_meeting_start_time")]
		public int CheckinMinutesPriorToStartTime { get; set; }

		/// <summary>
		/// Gets or sets the number of minutes releasing a room due to no check-out or Zoom Room activity.
		/// </summary>
		[JsonPropertyName("allowed_minutes_before_release_room_after_no_checkout")]
		public int AllowedMinutesBeforeReleaseRoomAfterNoCheckout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to notify in-room participants on the Zoom Rooms display when users check in.
		/// </summary>
		[JsonPropertyName("enable_new_user_checkin_notification")]
		public bool EnableNewUserCheckinNotification { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to remove recurring meeting series from Zoom Rooms.
		/// </summary>
		[JsonPropertyName("enable_remove_recurring")]
		public bool EnableRemoveRecurring { get; set; }

		/// <summary>
		/// Gets or sets the allowed number of consecutive missed check-ins before removing the room from entire meeting series.
		/// </summary>
		[JsonPropertyName("allowed_consecutive_missed_checkin_before_removing_room")]
		public int AllowedConsecutiveMissedCheckinBeforeRemovingRoom { get; set; }
	}
}
