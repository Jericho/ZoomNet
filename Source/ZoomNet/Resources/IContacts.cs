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
		/// Retrieve the current user's contacts.
		/// </summary>
		/// <param name="type">The type of contacts.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact">contacts</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Contact>> GetAllAsync(ContactType type = ContactType.Internal, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Search contacts.
		/// </summary>
		/// <param name="keyword">The search keyword: either first name, last na,me or email of the contact.</param>
		/// <param name="queryPresenceStatus">Indicate whether you want the status of a contact to be included in the response.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact">contacts</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Contact>> SearchAsync(string keyword, bool queryPresenceStatus = true, int recordsPerPage = 1, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve information about a specific contact of the current Zoom user.
		/// </summary>
		/// <param name="contactId">The unique identifier or email address of the contact.</param>
		/// <param name="queryPresenceStatus">Indicate whether you want the status of a contact to be included in the response.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Contact"/>.
		/// </returns>
		Task<Contact> GetAsync(string contactId, bool queryPresenceStatus = true, CancellationToken cancellationToken = default);
	}
}
