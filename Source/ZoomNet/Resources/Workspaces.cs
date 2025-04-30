using Pathoschild.Http.Client;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Workspaces : IWorkspaces
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Workspaces" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Workspaces(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Workspace>> GetAllAsync(string locationId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("workspaces")
				.WithArgument("location_id", locationId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Workspace>("workspaces");
		}

		/// <inheritdoc/>
		public Task<string> CreateAsync(string name, WorkspaceType type = WorkspaceType.Desk, string locationId = null, string calendarId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "workspace_name", name },
				{ "workspace_type", type.ToEnumString() },
				{ "location_id", locationId },
				{ "calendar_resource_id", calendarId }
			};

			return _client
				.PostAsync("workspaces")
				.WithBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("id");
		}
	}
}
