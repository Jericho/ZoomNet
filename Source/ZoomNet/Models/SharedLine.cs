using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Shared line information.
	/// </summary>
	public class SharedLine
	{
		/// <summary>
		/// Gets or sets the alias.
		/// </summary>
		[JsonPropertyName("alias")]
		public string Alias { get; set; }

		/// <summary>
		/// Gets or sets outbound caller id.
		/// </summary>
		[JsonPropertyName("outbound_caller_id")]
		public string OutboundCallerId { get; set; }

		/// <summary>
		/// Gets or sets line subscription information.
		/// </summary>
		[JsonPropertyName("line_subscription")]
		public LineSubscription LineSubscription { get; set; }
	}
}
