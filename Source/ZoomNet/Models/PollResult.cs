using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// The result of a poll for a given participant.
	/// </summary>
	public class PollResult
	{
		/// <summary>
		/// Gets or sets the name of the user who submitted answers to the poll.
		/// </summary>
		/// <remarks>
		/// If "anonymous" option is enabled for a poll, the participant's polling information will be kept anonymous and the value of 'name' field will be "Anonymous Attendee".
		/// </remarks>
		/// <value>
		/// The name of the participant.
		/// </value>
		[JsonProperty("file_name", NullValueHandling = NullValueHandling.Ignore)]
		public string ParticipantName { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user who submitted answers to the poll.
		/// </summary>
		/// <value>
		/// The email address of the participant.
		/// </value>
		[JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
		public string ParticipantEmail { get; set; }

		/// <summary>
		/// Gets or sets the answers to questions asked during the poll.
		/// </summary>
		/// <value>
		/// The answers.
		/// </value>
		[JsonProperty("question_details", NullValueHandling = NullValueHandling.Ignore)]
		public PollAnswer[] Details { get; set; }
	}
}
