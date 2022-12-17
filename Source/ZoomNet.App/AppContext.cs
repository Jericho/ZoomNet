using System.Text.Json.Serialization;

namespace ZoomNet.App
{
	/// <summary>
	/// The app context is a set of parameters that describe the execution context for the application.
	/// These parameters include user identifiers, meeting identifiers, and other contextual information.
	/// </summary>
	public class AppContext
	{
		/// <summary>
		/// The context type where this app is opened, could be 'panel', 'meeting', or 'webinar'.
		/// </summary>
		[JsonPropertyName("typ")]
		public string ContextType { get; set; }

		/// <summary>
		/// The Zoom user id who open this app.
		/// </summary>
		[JsonPropertyName("uid")]
		public string UserId { get; set; }

		/// <summary>
		/// The Zoom meeting uuid identifies the meeting in which this app is opened, only returned when value of typ is 'meeting'.
		/// </summary>
		[JsonPropertyName("mid")]
		public string MeetingUuid { get; set; }

		/// <summary>
		/// Action payload supplied in the deeplink.
		/// </summary>
		[JsonPropertyName("act")]
		public string Action { get; set; }

		/// <summary>
		/// No idea... it's not documented.
		/// </summary>
		[JsonPropertyName("dev")]
		public string Dev { get; set; }

		/// <summary>
		/// The create timestamp of this context.
		/// </summary>
		[JsonPropertyName("ts")]
		public long CreatedTimestamp { get; set; }

		/// <summary>
		/// The expiration timestamp of this context.
		/// </summary>
		[JsonPropertyName("exp")]
		public long ExpirationTimestamp { get; set; }

		/// <summary>
		/// The theme of the host application.
		/// </summary>
		[JsonPropertyName("theme")]
		public string Theme { get; set; }

		/// <summary>
		/// No idea... it's not documented.
		/// </summary>
		[JsonPropertyName("aud")]
		public string Aud { get; set; }

		/// <summary>
		/// The issuer of the token.
		/// </summary>
		[JsonPropertyName("iss")]
		public string Issuer { get; set; }

		/// <summary>
		/// No idea... it's not documented.
		/// </summary>
		[JsonPropertyName("entitlements")]
		public object[] Entitlements { get; set; }
	}
}
