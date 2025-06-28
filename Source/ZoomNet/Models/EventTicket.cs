using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Someone who has registered for an Event.
	/// </summary>
	public class EventTicket
	{
		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the ticket id.</summary>
		[JsonPropertyName("ticket_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the ticket type id.</summary>
		[JsonPropertyName("ticket_type_id")]
		public string TypeId { get; set; }

		/// <summary>Gets or sets the set of unique alphanumeric characters that references the external ticket ID.</summary>
		[JsonPropertyName("external_ticket_id")]
		public string ExternalTicketId { get; set; }

		/// <summary>Gets or sets the unique join link created for the ticket.</summary>
		[JsonPropertyName("event_join_link")]
		public string EventJoinUrl { get; set; }

		/// <summary>Gets or sets a value indicating whether to send email notifications.</summary>
		[JsonPropertyName("send_notification")]
		public bool SendNotifications { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to support guest join i.e. Non-Zoom users (Fast join without upfront authentication).
		/// If true then the registration_needed flag should not be set to true as it is an invalid combination.
		/// </summary>
		[JsonPropertyName("fast_join")]
		public bool FastJoin { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether egistrant needs to fill an online registration form.
		/// If true then the registration questions fields such as first_name, last_name, and address are not needed in the request body and are ignored if present.
		/// Also, if true then event_registration_link is returned instead of event_join_link in the response.
		/// </summary>
		[JsonPropertyName("registration_needed")]
		public bool RegistrationNeeded { get; set; }

		/// <summary>Gets or sets the list of session ID’s users want to register for the event.</summary>
		/// <remarks>This is applicable only for recurring events with session level registration enabled.</remarks>
		[JsonPropertyName("session_ids")]
		public string[] SessionIds { get; set; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>Gets or sets the address.</summary>
		[JsonPropertyName("address")]
		public string Address { get; set; }

		/// <summary>Gets or sets the city.</summary>
		[JsonPropertyName("city")]
		public string City { get; set; }

		/// <summary>Gets or sets the country.</summary>
		[JsonPropertyName("country")]
		public string Country { get; set; }

		/// <summary>Gets or sets the zip/postal Code.</summary>
		[JsonPropertyName("zip")]
		public string Zip { get; set; }

		/// <summary>Gets or sets the state/Province.</summary>
		[JsonPropertyName("state")]
		public string State { get; set; }

		/// <summary>Gets or sets the phone.</summary>
		[JsonPropertyName("phone")]
		public string Phone { get; set; }

		/// <summary>Gets or sets the industry.</summary>
		[JsonPropertyName("industry")]
		public string Industry { get; set; }

		/// <summary>Gets or sets the job Title.</summary>
		[JsonPropertyName("job_title")]
		public string JobTitle { get; set; }

		/// <summary>Gets or sets the organization.</summary>
		[JsonPropertyName("organization")]
		public string Organization { get; set; }

		/// <summary>
		/// Gets or sets the questions &amp; comments.
		/// </summary>
		[JsonPropertyName("comments")]
		public string Comments { get; set; }

		/// <summary>Gets or sets the custom questions.</summary>
		[JsonPropertyName("custom_questions")]
		[JsonConverter(typeof(TicketCustomQuestionsAnswersConverter))]
		public KeyValuePair<string, string>[] CustomQuestions { get; set; }
	}
}
