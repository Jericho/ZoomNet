using GraphQL;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom GraphQL client.
	/// </summary>
	public interface IZoomGraphQLClient
	{
		Task<T> SendQueryAsync<T>(GraphQLRequest request, CancellationToken cancellationToken = default);
	}
}
