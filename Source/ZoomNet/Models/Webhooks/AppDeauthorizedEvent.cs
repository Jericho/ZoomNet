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
