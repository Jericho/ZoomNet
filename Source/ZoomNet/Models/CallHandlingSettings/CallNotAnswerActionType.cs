namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// The types of action to take when a call is not answered.
	/// </summary>
	public enum CallNotAnswerActionType
	{
		/// <summary>
		/// Forward to a Voicemail.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToVoicemail = 1,

		/// <summary>
		/// Forward to the User.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToUser = 2,

		/// <summary>
		/// Forward to the Zoom Room.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToZoomRoom = 3,

		/// <summary>
		/// Forward to the Common Area.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToCommonArea = 4,

		/// <summary>
		/// Forward to the Cisco/Polycom Room.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToCiscoOrPolycomRoom = 5,

		/// <summary>
		/// Forward to the Auto Receptionist.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToAutoReceptionist = 6,

		/// <summary>
		/// Forward to a Call Queue.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToCallQueue = 7,

		/// <summary>
		/// Forward to a Shared Line Group.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToSharedLineGroup = 8,

		/// <summary>
		/// Forward to an External Contact.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToExternalContact = 9,

		/// <summary>
		/// Forward to a Phone Number.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToPhoneNumber = 10,

		/// <summary>
		/// Disconnect.
		/// Applicable to User, Call Queue, or Shared Line Group.
		/// </summary>
		Disconnect = 11,

		/// <summary>
		/// Play a message, then disconnect.
		/// Applicable to User, Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		PlayMessageThenDisconnect = 12,

		/// <summary>
		/// Forward to an Interactive Voice Response (IVR).
		/// Applicable to Auto Receptionist.
		/// </summary>
		ForwardToIvr = 14,

		/// <summary>
		/// Forward to a Partner Contact Center.
		/// Applicable to Auto Receptionist.
		/// </summary>
		ForwardToPartnerContactCenter = 15,

		/// <summary>
		/// Forward to Microsoft Teams Resource Account. Required the license of Zoom Phone for Microsoft Teams.
		/// Applicable to Call queue, Auto Receptionist, or Shared Line group.
		/// </summary>
		ForwardToMicrosoftTeamsResourceAccount = 18,

		/// <summary>
		///  Forward to a Zoom Contact Center. Required Zoom Contact Center license.
		///  Applicable to Call Queue, Auto Receptionist, or Shared Line Group.
		/// </summary>
		ForwardToZoomContactCenter = 19
	}
}
