using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about a session reservation option.
	/// </summary>
	public class EventSessionReservationInfo
	{
		/// <summary>Gets or sets a value indicating whether the session reservations are enabled.</summary>
		/// <remarks>This option is supported for multi-session type events only.</remarks>
		[JsonPropertyName("allow_reservations")]
		public bool AllowReservations { get; set; }

		/// <summary>Gets or sets the maximum number of session reservations allowed.</summary>
		[JsonPropertyName("max_capacity")]
		public int MaxCapacity { get; set; }
	}
}
