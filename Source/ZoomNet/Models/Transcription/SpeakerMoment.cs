using System;
using System.Collections.Generic;

namespace ZoomNet.Models.Transcription
{
	public sealed class SpeakerMoment
	{
		public TimeSpan Timestamp { get; internal set; }
		public IReadOnlyList<SpeakerInfo> Speakers { get; internal set; }
	}
}
