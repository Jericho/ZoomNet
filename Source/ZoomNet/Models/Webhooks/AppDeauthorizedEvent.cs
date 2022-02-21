using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user uninstalls or deauthorizes your app.
	/// </summary>
	public class AppDeauthorizedEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in wich the event occured.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the user Id.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the app's client Id.
		/// </summary>
		[JsonPropertyName("client_id")]
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has authorized you to store their data even after they uninstall your app.
		/// If the value is 'false', you must delete the user's data and call the [Data Compliance API](https://marketplace.zoom.us/docs/api-reference/zoom-api/data-compliance/compliance) within ten days of receiving the deauthorization webhook.
		/// If the value is 'true', no further action is required on your end.
		/// </summary>
		[JsonPropertyName("user_data_retention")]
		public bool CanPreserveUserData { get; set; }

		/// <summary>
		/// Gets or sets the time at which the user uninstalled your app.
		/// </summary>
		[JsonPropertyName("deauthorization_time")]
		public DateTime DeauthorizationTime { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for each instance of deauthorization / app uninstallation done by a user.
		/// </summary>
		[JsonPropertyName("signature")]
		public string Signature { get; set; }
	}
}
