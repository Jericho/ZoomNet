using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage tracking fields on your Zoom account.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/methods/#tag/Tracking-Field">Zoom documentation</a> for more information.
	/// </remarks>
	public class TrackingFields : ITrackingFields
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Accounts" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal TrackingFields(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<TrackingField[]> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"tracking_fields")
				.WithCancellationToken(cancellationToken)
				.AsObject<TrackingField[]>("tracking_fields");
		}
	}
}
