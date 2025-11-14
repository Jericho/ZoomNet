using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// User's TSP account information.
	/// </summary>
	/// <remarks>
	/// A user can have a maximum of two TSP accounts.
	/// </remarks>
	public class TspAccount
	{
		/// <summary>
		/// Gets or sets the TSP account id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the conference code (max length 16).
		/// </summary>
		[JsonPropertyName("conference_code")]
		public string ConferenceCode { get; set; }

		/// <summary>
		/// Gets or sets the leader PIN (max length 16).
		/// </summary>
		[JsonPropertyName("leader_pin")]
		public string LeaderPin { get; set; }

		/// <summary>
		/// Gets or sets the TSP bridge zone.
		/// </summary>
		[JsonPropertyName("tsp_bridge")]
		public string TspBridge { get; set; }

		/// <summary>
		/// Gets or sets information about the TSP dial-in numbers.
		/// </summary>
		[JsonPropertyName("dial_in_numbers")]
		public DialInNumber[] DialInNumbers { get; set; }
	}
}
