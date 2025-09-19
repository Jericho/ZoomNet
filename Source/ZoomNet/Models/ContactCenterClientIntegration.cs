using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the contact center's client integration.
	/// </summary>
	public enum ContactCenterClientIntegration
	{
		/// <summary>Default.</summary>
		[EnumMember(Value = "Default")]
		Default,

		/// <summary>Salesforce.</summary>
		[EnumMember(Value = "Salesforce")]
		Salesforce,

		/// <summary>Zendesk.</summary>
		[EnumMember(Value = "Zendesk")]
		Zendesk,

		/// <summary>ServiceNow.</summary>
		[EnumMember(Value = "ServiceNow")]
		ServiceNow,

		/// <summary>Microsoft_Dynamics_365.</summary>
		[EnumMember(Value = "Microsoft_Dynamics_365")]
		MicrosoftDynamics365,
	}
}
