using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "toll_free_and_fee_based_toll_call")]
		public AudioConferencingCallSettings AudioConferencingCallSettings { get; set; }
	}
}
