namespace ZoomNet.Models
{
	/// <summary>Follow-up email notification schedules.</summary>
	public enum FollowupEmailNotificationSchedule
	{
		/// <summary>Do not send a follow-up email notification.</summary>
		None = 0,

		/// <summary>Send 1 day after the scheduled end date.</summary>
		OneDay = 1,

		/// <summary>Send 2 days after the scheduled end date.</summary>
		TwoDays = 2,

		/// <summary>Send 3 days after the scheduled end date.</summary>
		ThreeDays = 3,

		/// <summary>Send 4 days after the scheduled end date.</summary>
		FourDays = 4,

		/// <summary>Send 5 days after the scheduled end date.</summary>
		FiveDays = 5,

		/// <summary>Send 6 days after the scheduled end date.</summary>
		SixDays = 6,

		/// <summary>Send 7 days after the scheduled end date.</summary>
		SevenDays = 7
	}
}
