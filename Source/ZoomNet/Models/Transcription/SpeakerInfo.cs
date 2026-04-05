namespace ZoomNet.Models.Transcription
{
	/// <summary>
	/// Represents metadata about a participant detected in Zoom's diarization
	/// timeline at a specific moment in the recording. This information is used
	/// to infer which user was speaking during a given time range.
	/// </summary>
	public sealed class SpeakerInfo
	{
		/// <summary>
		/// Gets the display name of the participant, if available.
		/// </summary>
		public string Username { get; internal set; }

		/// <summary>
		/// Gets the Zoom internal user identifier associated with the participant.
		/// This value corresponds to the <c>user_id</c> field in Zoom's timeline data.
		/// </summary>
		public string UserId { get; internal set; }

		/// <summary>
		/// Gets a value indicating whether Zoom detected multiple people speaking
		/// or contributing audio on the same channel for this participant entry.
		/// </summary>
		public bool MultiplePeople { get; internal set; }

		/// <summary>
		/// Gets the email address associated with the participant, if provided
		/// in the diarization metadata.
		/// </summary>
		public string Email { get; internal set; }

		/// <summary>
		/// Gets the unique identifier for the associated Zoom user.
		/// </summary>
		public string ZoomUserId { get; internal set; }

		/// <summary>
		/// Gets the URL of the user's avatar image.
		/// </summary>
		public string AvaturUrl { get; internal set; }

		/// <summary>
		/// Gets the type identifier for the client.
		/// </summary>
		public int ClientType { get; internal set; }
	}
}
