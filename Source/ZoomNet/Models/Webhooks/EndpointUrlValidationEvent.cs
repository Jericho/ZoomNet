using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ZoomNet.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when Zoom wants to validate the event notification endpoint URL you have configured in your account.
	/// </summary>
	public class EndpointUrlValidationEvent : Event
	{
		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		public string PlainToken { get; set; }
	}
}
