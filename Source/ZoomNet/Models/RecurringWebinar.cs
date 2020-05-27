using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Webinar" />
	public class RecurringWebinar : Webinar
	{
		/// <summary>
		/// Gets or sets the occurrences.
		/// </summary>
		[JsonProperty(PropertyName = "occurrences", NullValueHandling = NullValueHandling.Ignore)]
		public MeetingOccurrence[] Occurrences { get; set; }

		/// <summary>
		/// Gets or sets the recurrence info.
		/// </summary>
		[JsonProperty(PropertyName = "recurrence", NullValueHandling = NullValueHandling.Ignore)]
		public RecurrenceInfo RecurrenceInfo { get; set; }
	}
}
