using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an admin or end user makes any of the following changes to a device:
	/// - adding or removing a Direct Inward Dialing(DID) service
	/// - adding or removing a delegation
	/// - adding or removing a shared line group
	/// - changing line labels or caller ID information
	/// - changing the site where a user is assigned
	/// - changing the Session Initiation Protocol (SIP) Zone assignment for a site where the user is assigned
	/// - adding or removing a device from a user.
	/// </summary>
	public class PhoneGenericDeviceProvisionEvent : Event
	{
		/// <summary>
		/// Gets or sets the user's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets phone device information.
		/// </summary>
		[JsonPropertyName("object")]
		public PhoneDevice Device { get; set; }
	}
}
