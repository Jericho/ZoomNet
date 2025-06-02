using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room basic profile.
	/// </summary>
	public class RoomBasicProfile
	{
		/// <summary>
		/// Gets or sets the code used to complete the setup of the Zoom Room.
		/// </summary>
		[JsonPropertyName("activation_code")]
		public string ActivationCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to hide this Zoom Room from your contact list. True to hide. False to show.
		/// </summary>
		[JsonPropertyName("hide_room_in_contacts")]
		public bool HideRoomInContacts { get; set; }

		/// <summary>
		/// Gets or sets the name of the Zoom Room.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the short display name of the Zoom Room.
		/// </summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to require a code to exit out of the Zoom Rooms application to switch between other apps.
		/// </summary>
		[JsonPropertyName("required_code_to_ext")]
		public bool RequireCodeToExit { get; set; }

		/// <summary>
		/// Gets or sets the 1-16 digit number or characters used to secure your Zoom Rooms application.
		/// This code must be entered on your Zoom Room Controller to change settings or sign out.
		/// </summary>
		[JsonPropertyName("room_passcode")]
		public string Passcode { get; set; }

		/// <summary>
		/// Gets or sets the email address to use for reporting Zoom Room issues.
		/// </summary>
		[JsonPropertyName("support_email")]
		public string SupportEmail { get; set; }

		/// <summary>
		/// Gets or sets the phone number to use for reporting Zoom Room issues.
		/// </summary>
		[JsonPropertyName("support_phone")]
		public string SupportPhone { get; set; }

		/// <summary>
		/// Gets or sets the calendar ressource ID.
		/// </summary>
		[JsonPropertyName("calendar_resource_id")]
		public string CalendarResourceId { get; set; }

		/// <summary>
		/// Gets or sets the room type.
		/// </summary>
		[JsonPropertyName("zoom_room_type")]
		public RoomType Type { get; set; }

		/// <summary>
		/// Gets or sets the location ID of the place where the Zoom Room was added.
		/// </summary>
		[JsonPropertyName("location_id")]
		public string LocationId { get; set; }

		/// <summary>
		/// Gets or sets the list of tag ID associated with the Zoom Room.
		/// </summary>
		[JsonPropertyName("tag_ids")]
		public string[] Tags { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Rooms Scheduling Display(s) of this Zoom Room will show the <see cref="DisplayName"/> instead of the <see cref="Name"/>.
		/// Note: this setting is only effective if a display_name is configured.
		/// </summary>
		[JsonPropertyName("use_display_name_on_scheduling_display")]
		public bool UseDisplayNameOnSchedulingDisplay { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Rooms Controller(s) and Zoom Rooms display(s) of this Zoom Room will show the <see cref="DisplayName"/> instead of the <see cref="Name"/>.
		/// Note: this setting is only effective if a display_name is configured.
		/// </summary>
		[JsonPropertyName("use_display_name_on_zoom_rooms_display_and_controller")]
		public bool UseDisplayNameOnDisplayAndController { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Room will use the <see cref="DisplayName"/> instead of the <see cref="Name"/> in meetings.
		/// The <see cref="DisplayName"/> will be shown in the Zoom meeting participants list and as the Zoom Rooms' label in the meeting video layout of other meeting participants.
		/// Note: this setting is only effective if a display_name is configured and the use_on_zoom_rooms_and_controller setting is enabled.
		/// </summary>
		[JsonPropertyName("use_display_name_in_meetings")]
		public bool UseDisplayNameInMeetings { get; set; }

		/// <summary>
		/// Gets or sets the in-room participant capacity of the Zoom Room.
		/// </summary>
		[JsonPropertyName("capacity")]
		public int Capacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Room show the seating capacity the display(s) of the Zoom Room.
		/// Note: this setting is only effective if a capacity is configured.
		/// </summary>
		[JsonPropertyName("show_room_capacity_on_the_zoom_rooms")]
		public bool ShowCapacity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Room show the seating capacity the Zoom Rooms Scheduling Display(s) of the Zoom Room.
		/// Note: this setting is only effective if a capacity is configured.
		/// </summary>
		[JsonPropertyName("show_room_capacity_on_the_scheduling_display")]
		public bool ShowCapacityOnSchedulingDisplay { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user assigned to a Personal Zoom Room.
		/// Note: this field will only be present for Personal Zoom Rooms.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }
	}
}
