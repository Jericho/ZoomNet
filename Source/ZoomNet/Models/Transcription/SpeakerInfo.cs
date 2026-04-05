namespace ZoomNet.Models.Transcription
{
	public sealed class SpeakerInfo
	{
		public string Username { get; internal set; }
		public bool MultiplePeople { get; internal set; }
		public string UserId { get; internal set; }
		public string ZoomUserId { get; internal set; }
		public string AvaturUrl { get; internal set; }
		public int ClientType { get; internal set; }
		public string Email { get; internal set; }
	}
}
