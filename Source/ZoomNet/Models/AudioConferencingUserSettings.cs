using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Audio conferencing user settings.
	/// </summary>
	public class AudioConferencingUserSettings
	{
		/// <summary>
		/// Gets or sets the user's Toll-free and Fee-based Toll Call settings.
		/// </summary>
		[JsonPropertyName("toll_free_and_fee_based_toll_call")]
		public AudioConferencingCallSettings AudioConferencingCallSettings { get; set; }
	}
}
