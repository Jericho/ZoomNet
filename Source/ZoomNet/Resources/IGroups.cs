using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage groups under an account.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/groups/groups">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IGroups
	{
		/// <summary>
		/// Retrieve all groups on your account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Group">groups</see>.
		/// </returns>
		Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default);
	}
}
