using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage contacts.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IContacts" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/contacts/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IContacts
	{
		/// <summary>
		/// Retrieve a user's chat channels.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ChatChannel">channels</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Contact>> GetUserContactsAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
	}
}
