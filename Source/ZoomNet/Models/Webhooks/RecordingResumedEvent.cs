namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time a previously paused recording of a meeting or webinar is resumed.</summary>
	public record RecordingResumedEvent : RecordingProgressEvent
	{
	}
}
