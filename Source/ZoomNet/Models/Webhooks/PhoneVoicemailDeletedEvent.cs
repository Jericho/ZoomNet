namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a voicemail is deleted (sent to the trash).
	/// </summary>
	public class PhoneVoicemailDeletedEvent : PhoneVoicemailEvent
	{
		/// <summary>
		/// Gets or sets deleted voicemails information.
		/// </summary>
		public VoicemailBasicInfo[] Voicemails { get; set; }
	}
}
