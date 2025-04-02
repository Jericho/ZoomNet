using Pathoschild.Http.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Rooms : IRooms
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Rooms" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Rooms(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Room>> GetAllAsync(string parentLocationId = null, RoomType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms")
				.WithArgument("parent_location_id", parentLocationId)
				.WithArgument("type", type?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Room>("locations");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<RoomLocation>> GetAllLocationsAsync(string parentLocationId = null, RoomType? type = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"rooms/locations")
				.WithArgument("parent_location_id", parentLocationId)
				.WithArgument("type", type?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<RoomLocation>("locations");
		}

		/// <inheritdoc/>
		public Task UpdateRoomsStructureAsync(IEnumerable<RoomType> structure, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "structures", structure.ToArray() }
			};

			return _client
				.PutAsync("rooms/locations/structure")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<RoomLocation> CreateLocationAsync(string name, string parentId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "parent_location_id", parentId }
			};

			return _client
				.PostAsync("rooms/locations")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<RoomLocation>();
		}
	}
}
