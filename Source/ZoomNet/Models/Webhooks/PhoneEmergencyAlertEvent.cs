using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when Zoom phone users dials emergency number from their Zoom phone.
	/// </summary>
	public class PhoneEmergencyAlertEvent : Event
	{
		/// <summary>
		/// Gets or sets caller's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets emergency call information.
		/// </summary>
		[JsonPropertyName("object")]
		public EmergencyCallAlert Alert { get; set; }
	}
}
