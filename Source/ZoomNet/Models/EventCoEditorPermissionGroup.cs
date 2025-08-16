using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the permission group assigned to a coeditor.
	/// </summary>
	public enum EventCoEditorPermissionGroup
	{
		/// <summary>Publish.</summary>
		[EnumMember(Value = "Publish")]
		Publish,

		/// <summary>Event Configuration.</summary>
		[EnumMember(Value = "EventConfiguration")]
		EventConfiguration,

		/// <summary>Event Branding.</summary>
		[EnumMember(Value = "EventBranding")]
		EventBranding,

		/// <summary>Registration and Join.</summary>
		[EnumMember(Value = "Registration & Join")]
		RegistrationAndJoin,

		/// <summary>Venue.</summary>
		[EnumMember(Value = "Venue")]
		Venue,

		/// <summary>Event Experience.</summary>
		[EnumMember(Value = "EventExperience")]
		EventExperience,

		/// <summary>Event Planning.</summary>
		[EnumMember(Value = "EventPlanning")]
		EventPlanning,

		/// <summary>Special Role.</summary>
		[EnumMember(Value = "SpecialRole")]
		SpecialRole,

		/// <summary>Post Event.</summary>
		[EnumMember(Value = "PostEvent")]
		PostEvent,

		/// <summary>Analytics.</summary>
		[EnumMember(Value = "Analytics")]
		Analytics,

		/// <summary>Email.</summary>
		[EnumMember(Value = "Email")]
		Email,

		/// <summary>Integrations.</summary>
		[EnumMember(Value = "Integrations")]
		Integrations,
	}
}
