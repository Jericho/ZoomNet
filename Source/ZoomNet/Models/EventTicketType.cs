using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Ticket type.
	/// </summary>
	public class EventTicketType
	{
		/// <summary>Gets or sets the ticket type id.</summary>
		[JsonPropertyName("ticket_type_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the ticket type.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the currency.</summary>
		[JsonPropertyName("currency")]
		public string Currency { get; set; }

		/// <summary>Gets or sets a value indicating whether the ticket is free.</summary>
		[JsonPropertyName("free")]
		public bool IsFree { get; set; }

		/// <summary>Gets or sets the price.</summary>
		[JsonPropertyName("price")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)] // This allows us to overcome the fact that the API returns this value as a string rather than a number
		public double Price { get; set; }

		/// <summary>Gets or sets the total number of tickets available.</summary>
		[JsonPropertyName("quantity")]
		public int QuantityTotal { get; set; }

		/// <summary>Gets or sets the start of ticket sales.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the end of ticket sales.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; set; }

		/// <summary>Gets or sets the description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the number of tickets that have been sold.</summary>
		[JsonPropertyName("sold_quantity")]
		public int QuantitySold { get; set; }

		/// <summary>Gets or sets the list of session IDs allowed for this ticket type or ALL.</summary>
		/// <remarks>'ALL' refers to all the sessions in the event.</remarks>
		[JsonPropertyName("sessions")]
		public string[] Sessions { get; set; }

		/// <summary>Gets or sets the list of session IDs pre-bookmarked or ALL.</summary>
		/// <remarks>ALL refers to all the sessions in the event.</remarks>
		[JsonPropertyName("bookmarked_sessions")]
		public string[] BookmarkedSessions { get; set; }

		/// <summary>Gets or sets a value indicating whether the session is visible in the event lobby.</summary>
		//[JsonPropertyName("private_visibility_rules")]
		//public VisibilityRule VisibilityRules { get; set; }
	}
}
