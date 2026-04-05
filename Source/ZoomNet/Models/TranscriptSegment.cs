using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a segment of transcribed audio, including timing, speaker, and text information.
	/// </summary>
	/// <remarks>A transcript segment defines a contiguous interval within an audio transcript, typically
	/// corresponding to a single speaker's utterance. Use this type to access or display individual portions of a
	/// transcript, such as for highlighting or speaker attribution.</remarks>
	public sealed class TranscriptSegment
	{
		/// <summary>
		/// Gets the start time of the interval.
		/// </summary>
		public TimeSpan Start { get; internal set; }

		/// <summary>
		/// Gets the time of day at which the interval ends.
		/// </summary>
		public TimeSpan End { get; internal set; }

		/// <summary>
		/// Gets the name of the speaker associated with the content.
		/// </summary>
		public string Speaker { get; internal set; }

		/// <summary>
		/// Gets the text content associated with this instance.
		/// </summary>
		public string Text { get; internal set; }
	}
}
