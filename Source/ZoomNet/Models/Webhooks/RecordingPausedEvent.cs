namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time a recording is paused by a user.</summary>
	public record RecordingPausedEvent : RecordingProgressEvent
	{
	}
}
