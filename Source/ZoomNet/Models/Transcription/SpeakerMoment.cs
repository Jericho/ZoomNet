using System;
using System.Collections.Generic;

namespace ZoomNet.Models.Transcription
{
	/// <summary>
	/// Represents a diarized moment in the recording where Zoom detected one or more
	/// participants as speaking or contributing audio at a specific point in time.
	/// </summary>
	public sealed class SpeakerMoment
	{
		/// <summary>
		/// Gets the timestamp within the recording at which the diarization snapshot
		/// was taken. This value corresponds to the <c>ts</c> field in Zoom's
		/// timeline data.
		/// </summary>
		public TimeSpan Timestamp { get; internal set; }

		/// <summary>
		/// Gets the collection of participants detected at this moment in time.
		/// Each entry contains metadata about the user, such as their display name,
		/// Zoom user ID, and whether multiple people were detected on the same audio
		/// channel.
		/// </summary>
		public IReadOnlyList<SpeakerInfo> Speakers { get; internal set; }
	}
}
