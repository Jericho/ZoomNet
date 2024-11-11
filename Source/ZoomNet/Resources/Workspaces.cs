using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

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
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"workspaces")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Workspace>("workspaces");
		}
	}
}
