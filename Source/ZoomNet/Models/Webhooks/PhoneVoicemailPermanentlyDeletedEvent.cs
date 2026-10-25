namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a voicemail is permanently deleted from the trash.</summary>
	public record PhoneVoicemailPermanentlyDeletedEvent : PhoneVoicemailEvent
	{
		/// <summary>Gets or sets deleted voicemails information.</summary>
		public VoicemailBasicInfo[] Voicemails { get; init; }
	}
}
