using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Opt-out reason for call queue.
	/// </summary>
	public class CallQueueOptOutReason
	{
		/// <summary>
		/// Gets or sets opt-out reason code.
		/// </summary>
		[JsonPropertyName("code")]
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether opt-out reason is enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this opt-out reason is system default one.
		/// </summary>
		[JsonPropertyName("system")]
		public bool IsSystem { get; set; }
	}
}
