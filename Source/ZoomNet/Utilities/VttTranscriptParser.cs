using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZoomNet.Models.Transcription;

namespace ZoomNet.Utilities
{
	internal static class VttTranscriptParser
	{
		public static IReadOnlyList<TranscriptSegment> Parse(string vttContent)
		{
			var segments = new List<TranscriptSegment>();
			if (string.IsNullOrWhiteSpace(vttContent)) return segments;

			using var reader = new StringReader(vttContent);

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				// Skip headers and empty lines
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("WEBVTT"))
					continue;

				// Look for timestamp line
				if (!TryParseTimestamp(line, out var start, out var end))
					continue;

				// Read text block
				var textLines = new List<string>();
				while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
				{
					textLines.Add(line);
				}

				var (speaker, text) = ExtractSpeaker(textLines);

				segments.Add(new TranscriptSegment
				{
					Start = start,
					End = end,
					Speaker = speaker,
					Text = text
				});
			}

			return segments;
		}

		private static bool TryParseTimestamp(string line, out TimeSpan start, out TimeSpan end)
		{
			start = end = default;

			var parts = line.Split([" --> "], StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 2) return false;

			return TimeSpan.TryParse(parts[0], out start)
				&& TimeSpan.TryParse(parts[1], out end);
		}

		private static (string Speaker, string Text) ExtractSpeaker(List<string> lines)
		{
			if (lines.Count == 0)
				return (null, string.Empty);

			var first = lines[0].Trim();

			// Zoom uses <v Speaker Name>
			if (first.StartsWith("<v ") && first.Contains('>'))
			{
				var end = first.IndexOf('>');
				var speaker = first.Substring(3, end - 3).Trim();
				var text = first.Substring(end + 1).Trim();

				// Merge remaining lines
				if (lines.Count > 1)
					text += " " + string.Join(" ", lines.Skip(1)).Trim();

				return (speaker, text);
			}

			// No speaker tag
			return (null, string.Join(" ", lines).Trim());
		}
	}
}
