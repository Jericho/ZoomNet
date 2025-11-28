using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Basic information about voicemail (currently includes only id).
	/// </summary>
	/// <remarks>
	/// Used in <see cref="Webhooks.PhoneVoicemailDeletedEvent"/> and <see cref="Webhooks.PhoneVoicemailPermanentlyDeletedEvent"/>.
	/// </remarks>
	public class VoicemailBasicInfo
	{
		/// <summary>
		/// Gets or sets unique identifier of the voicemail.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
