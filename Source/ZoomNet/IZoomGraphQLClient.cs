using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom GraphQL client.
	/// </summary>
	public interface IZoomGraphQLClient
	{
		/// <summary>
		/// Send a request to Zoom's GrapgQL server.
		/// </summary>
		/// <typeparam name="T">The type of object returned from the query.</typeparam>
		/// <param name="request">The GraphQL request.</param>
		/// <param name="variables">The variables.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The result of the request.</returns>
		Task<T> SendQueryAsync<T>(string request, object variables = null, CancellationToken cancellationToken = default);
	}
}
