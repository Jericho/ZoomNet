using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>

	public class TrackingFields : ITrackingFields
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackingFields" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal TrackingFields(IClient client)
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
