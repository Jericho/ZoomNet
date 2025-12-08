using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow users to send and receive messages.
	/// </summary>
	public class OutboundSmsSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether message copying is allowed.
		/// </summary>
		[JsonPropertyName("allow_copy")]
		public bool? AllowCopy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether content pasting is allowed.
		/// </summary>
		[JsonPropertyName("allow_paste")]
		public bool? AllowPaste { get; set; }
	}
}
