using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An event access link.
	/// </summary>
	public class EventAccessLink
	{
		/// <summary>Gets or sets the access link id.</summary>
		[JsonPropertyName("access_link_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the access link.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the type of access link of the event.</summary>
		[JsonPropertyName("type")]
		public EventAccessLinkType Type { get; set; }

		/// <summary>Gets or sets a value indicating whether this is the default access link.</summary>
		/// <remarks>
		/// The default link ensures compatibility with the event links that are used throughout the attendee experience.
		/// These existing event links will operate as a registration link or group join link based upon the default setting.
		/// </remarks>
		[JsonPropertyName("is_default")]
		public bool IsDefault { get; set; }

		/// <summary>Gets or sets the event access link.</summary>
		[JsonPropertyName("url")]
		public string Url { get; set; }

		/// <summary>Gets or sets the event authentication method during registration or during join, depending on the access link type.</summary>
		[JsonPropertyName("authentication_method")]
		public EventAuthenticationMethod AuthenticationMethod { get; set; }

		/// <summary>Gets or sets the registration restricted to these specific domains.</summary>
		/// <remarks>This array returns a maximum of 50 domains.</remarks>
		[JsonPropertyName("allow_domain_list")]
		public string[] AllowDomainList { get; set; }

		/// <summary>Gets or sets the registration restricted to users by email addresses.</summary>
		/// <remarks>Only these users are allowed to register and these email addresses will receive an email invitation.</remarks>
		[JsonPropertyName("email_restrict_list")]
		public string[] EmailRestrictList { get; set; }

		/// <summary>Gets or sets the security at the time of joining.</summary>
		[JsonPropertyName("security_at_join")]
		public EventSecurityAtJoin SecurityAtJoin { get; set; }

		/// <summary>Gets or sets the The ticket type ID.</summary>
		/// <remarks>
		/// This field applies only to group-join link type.
		/// This establishes the access permissions of the user within the event.
		/// </remarks>
		[JsonPropertyName("ticket_type_id")]
		public string TicketTypeId { get; set; }

		/// <summary>Gets or sets the recurring event registration type.</summary>
		/// <remarks>This is applicable only for recurring events.</remarks>
		[JsonPropertyName("recurring_registration_option")]
		public RecurringEventRegistrationType RecurringRegistrationType { get; set; }
	}
}
