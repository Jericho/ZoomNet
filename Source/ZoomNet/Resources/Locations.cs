using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage locations.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/rooms-location/">Zoom documentation</a> for more information.
	/// </remarks>
	public sealed class Locations : ILocations
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Rooms" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Locations(IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all locations on your account.
		/// </summary>
		/// <param name="parentLocationId">
		/// A unique identifier for the parent location.
		/// For instance, if a Zoom Room is located in Floor 1 of Building A, the location of Building A will be the parent location of Floor 1.
		/// Use this parameter to filter the response by a specific location hierarchy level.
		/// </param>
		/// <param name="type">Use this parameter to filter the response by the type of location.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Location">locations</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<Location>> GetAllAsync(string parentLocationId = null, LocationType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
			       .GetAsync($"rooms/locations")
			       .WithArgument("parent_location_id", parentLocationId)
			       .WithArgument("type", type.HasValue ? JToken.Parse(JsonConvert.SerializeObject(type.Value)).ToString() : null)
			       .WithArgument("page_size", recordsPerPage)
			       .WithArgument("next_page_token", pagingToken)
			       .WithCancellationToken(cancellationToken)
			       .AsPaginatedResponseWithToken<Location>("locations");
		}
	}
}
