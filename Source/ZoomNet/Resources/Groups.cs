using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage groups under an account.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IGroups" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/groups/groups">Zoom documentation</a> for more information.
	/// </remarks>
	public class Groups : IGroups
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Groups" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Groups(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all groups on your account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Group">groups</see>.
		/// </returns>
		public Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"groups")
				.WithCancellationToken(cancellationToken)
				.AsObject<Group[]>("groups");
		}
	}
}
