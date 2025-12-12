using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings related to playing the recording beep tone.
	/// </summary>
	public class PlayRecordingBeepTone
	{
		/// <summary>
		/// Gets or sets a value indicating whether to play the side tone beep for recorded users while recording.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets whether to play the recording beep tone all participants in the call or only the recording user.
		/// </summary>
		/// <remarks>
		/// Provided only when <see cref="Enabled"/> is true.
		/// </remarks>
		[JsonPropertyName("play_beep_member")]
		public PlayBeepMember? PlayBeepMember { get; set; }

		/// <summary>
		/// Gets or sets the beep time interval in seconds.
		/// </summary>
		/// <remarks>
		/// Provided only when <see cref="Enabled"/> is true.
		/// </remarks>
		[JsonPropertyName("play_beep_time_interval")]
		public int? PlayBeepTimeInterval { get; set; }

		/// <summary>
		/// Gets or sets the side tone beep volume.
		/// </summary>
		/// <remarks>
		/// Provided only when <see cref="Enabled"/> is true.
		/// </remarks>
		[JsonPropertyName("play_beep_volume")]
		public int? PlayBeepVolume { get; set; }
	}
}
