using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ZoomNet.Models.Transcription;

namespace ZoomNet.Utilities
{
	internal static class SpeakerTimelineParser
	{
		public static IReadOnlyList<SpeakerMoment> Parse(string json)
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			var root = JsonDocument.Parse(json).RootElement;
			var timelineProp = root.GetProperty("timeline");
			var timelineRefineProp = root.GetProperty("timeline_refine");
			var allTimeLineItems = timelineProp.EnumerateArray().Concat(timelineRefineProp.EnumerateArray());

			var moments = new List<SpeakerMoment>();

			foreach (var item in allTimeLineItems)
			{
				var ts = item.GetPropertyValue("ts", string.Empty);
				if (!TimeSpan.TryParse(ts, out var timeStamp))
					continue;

				var speakers = new List<SpeakerInfo>();

				foreach (var user in item.GetProperty("users").EnumerateArray())
				{
					speakers.Add(new SpeakerInfo
					{
						Username = user.GetPropertyValue("username", string.Empty),
						MultiplePeople = user.GetPropertyValue("multiple_people", false),
						UserId = user.GetPropertyValue("user_id", string.Empty),
						ZoomUserId = user.GetPropertyValue("zoom_userid", string.Empty),
						AvaturUrl = user.GetPropertyValue("avatar_url", string.Empty),
						ClientType = user.GetPropertyValue("client_type", 0),
						Email = user.GetPropertyValue("email_address", string.Empty)
					});
				}

				moments.Add(new SpeakerMoment
				{
					Timestamp = timeStamp,
					Speakers = speakers
				});
			}

			return moments
				.OrderBy(m => m.Timestamp)
				.ToList();
		}
	}
}
