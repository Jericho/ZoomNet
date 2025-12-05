using Microsoft.Extensions.Logging;
using System;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Options for the Zoom client.
	/// </summary>
	public class ZoomClientOptions
	{
		private const string ZOOM_GLOBAL_BASE_URI = "https://api.zoom.us/v2";
		private const string ZOOM_AUSTRALIA_BASE_URI = "https://api-au.zoom.us/v2";
		private const string ZOOM_CANADA_BASE_URI = "https://api-ca.zoom.us/v2";
		private const string ZOOM_EUROPE_BASE_URI = "https://api-eu.zoom.us/v2";
		private const string ZOOM_INDIA_BASE_URI = "https://api-in.zoom.us/v2";
		private const string ZOOM_SAUDI_ARABIA_BASE_URI = "https://api-sa.zoom.us/v2";
		private const string ZOOM_SINGAPORE_BASE_URI = "https://api-sg.zoom.us/v2";
		private const string ZOOM_UNITED_STATES_URI = "https://api-us.zoom.us/v2";

		/// <summary>
		/// Gets or sets the log levels for successful calls (HTTP status code in the 200-299 range).
		/// </summary>
		public LogLevel LogLevelSuccessfulCalls { get; set; } = LogLevel.Debug;

		/// <summary>
		/// Gets or sets the log levels for failed calls (HTTP status code outside of the 200-299 range).
		/// </summary>
		public LogLevel LogLevelFailedCalls { get; set; } = LogLevel.Error;

		/// <summary>
		/// Gets the base URL of the Zoom API endpoint.
		/// </summary>
		public Uri ApiBaseUrl { get; private set; } = new Uri(ZOOM_GLOBAL_BASE_URI);

		/// <summary>
		/// Configures the client to use Zoom's global base URL for API requests.
		/// </summary>
		/// <remarks>Use this method when you want the client to target Zoom's global API endpoints, rather than
		/// region-specific or custom endpoints.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the global base URL applied.</returns>
		public ZoomClientOptions WithGlobalBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_GLOBAL_BASE_URI);
		}

		/// <summary>
		/// Configures the client to use the Australia regional Zoom API base URL.
		/// </summary>
		/// <remarks>Use this method to target Zoom's Australia data center for API requests. This is useful for
		/// compliance or latency considerations when operating in the Australia region.</remarks>
		/// <returns>A new instance of <see cref="ZoomClientOptions"/> with the Australia base URL applied.</returns>
		public ZoomClientOptions WithAustraliaBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_AUSTRALIA_BASE_URI);
		}

		/// <summary>
		/// Configures the client to use the Zoom Canada API base URL.
		/// </summary>
		/// <remarks>Use this method to target Zoom's Canada-specific API endpoints. This is useful when regional data
		/// residency or compliance requirements apply.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the Canada API base URL applied.</returns>
		public ZoomClientOptions WithCanadaBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_CANADA_BASE_URI);
		}

		/// <summary>
		/// Configures the client to use the European Union Zoom API base URL.
		/// </summary>
		/// <remarks>Use this method to ensure that all API requests are directed to Zoom's European Union
		/// infrastructure, which may be required for compliance with regional data regulations.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the base URL set to the European Union endpoint.</returns>
		public ZoomClientOptions WithEuropeanUnionBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_EUROPE_BASE_URI);
		}

		/// <summary>
		/// Configures the client options to use the India-specific Zoom API base URL.
		/// </summary>
		/// <remarks>Use this method when you need to target the India region for Zoom API requests. This is useful
		/// for compliance with regional data residency requirements or to optimize latency for users in India.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the India base URL applied.</returns>
		public ZoomClientOptions WithIndiaBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_INDIA_BASE_URI);
		}

		/// <summary>
		/// Configures the client to use the Zoom API base URL for Saudi Arabia.
		/// </summary>
		/// <remarks>Use this method when you need to target the Zoom API endpoints hosted in Saudi Arabia. This is
		/// useful for compliance with regional data residency requirements or to optimize latency for users in that
		/// region.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the Saudi Arabia base URL applied.</returns>
		public ZoomClientOptions WithSaudiArabiaBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_SAUDI_ARABIA_BASE_URI);
		}

		/// <summary>
		/// Configures the client options to use the Singapore Zoom API base URL.
		/// </summary>
		/// <remarks>Use this method to target the Singapore region when making API requests. This is useful for
		/// compliance or latency considerations when your application or users are based in Singapore.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the Singapore API base URL applied.</returns>
		public ZoomClientOptions WithSingaporeBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_SINGAPORE_BASE_URI);
		}

		/// <summary>
		/// Configures the client options to use the United States Zoom API base URL.
		/// </summary>
		/// <remarks>Use this method to target the United States region when making API requests. This is useful when
		/// your application or users are primarily based in the United States, or when regional compliance is
		/// required.</remarks>
		/// <returns>A new <see cref="ZoomClientOptions"/> instance with the United States base URL applied.</returns>
		public ZoomClientOptions WithUnitedStatesBaseUrl()
		{
			return this.WithCustomBaseUrl(ZOOM_UNITED_STATES_URI);
		}

		/// <summary>
		/// Sets a custom base URL for API requests and returns the updated options instance.
		/// </summary>
		/// <param name="baseUrl">The base URL to use for API requests. Cannot be null or empty. Must be a valid absolute URI.</param>
		/// <returns>The current <see cref="ZoomClientOptions"/> instance with the updated base URL.</returns>
		public ZoomClientOptions WithCustomBaseUrl(string baseUrl)
		{
			ArgumentNullException.ThrowIfEmpty(baseUrl);

			this.ApiBaseUrl = new Uri(baseUrl);
			return this;
		}
	}
}
