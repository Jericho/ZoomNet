using System.Collections.Generic;

namespace ZoomNet.Models.Transcription
{
	/// <summary>
	/// Represents a unified transcript that combines Zoom's VTT closed‑captioning data
	/// with the TIMELINE word‑level transcript to provide a richer, more detailed
	/// representation of the recorded meeting's spoken content.
	/// </summary>
	public sealed class RichTranscript
	{
		/// <summary>
		/// Gets the list of transcript segments derived primarily from the VTT file.
		/// Each segment contains a time range, optional speaker attribution, and the
		/// corresponding block of spoken text.
		/// </summary>
		public IReadOnlyList<TranscriptSegment> Segments { get; internal set; }

		/// <summary>
		/// Gets the diarization timeline entries extracted from Zoom's
		/// timeline and timeline_refine data. These entries indicate which
		/// participants were detected as speaking at specific timestamps.
		/// </summary>
		public IReadOnlyList<SpeakerMoment> Diarization { get; internal set; }

		/// <summary>
		/// Gets the distinct list of speakers identified across both the VTT and
		/// TIMELINE transcripts. The list is normalized and ordered alphabetically.
		/// </summary>
		public IReadOnlyList<string> Speakers { get; internal set; }
	}
}
