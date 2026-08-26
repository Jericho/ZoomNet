namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate when a reminder is scheduled.</summary>
	public enum ReminderSchedule
	{
		/// <summary>No reminder.</summary>
		None = 0,

		/// <summary>One hour before the meeting.</summary>
		OneHourBeforeMeeting = 1,

		/// <summary>One day before the meeting.</summary>
		OneDayBeforeMeeting = 2,

		/// <summary>One hour and one day before the meeting.</summary>
		OneHourAndOneDayBeforeMeeting = 3,

		/// <summary>One week before the meeting.</summary>
		OneWeekBeforeMeeting = 4,

		/// <summary>Send 1 hour and 1 week before webinar.</summary>
		OneHourAndOneWeekBeforeMeeting = 5,

		/// <summary>Send 1 day and 1 week before webinar.</summary>
		OneDayAndOneWeekBeforeMeeting = 6,

		/// <summary>Send 1 hour, 1 day and 1 week before webinar.</summary>
		OneHourAndOneDayAndOneWeekBeforeMeeting = 7
	}
}
