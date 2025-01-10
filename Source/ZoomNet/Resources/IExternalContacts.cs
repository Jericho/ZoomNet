using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to access Zoom Phone API endpoints responsible for setting and retrieving data of external contacts.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/api/rest/reference/phone/methods/#tag/External-Contacts">
	/// Zoom API documentation</a> for more information.
	/// </remarks>
	public interface IExternalContacts
	{
		/// <summary>
		/// Retrieves a list of all of an account's external contacts.
		/// </summary>
		/// <param name="pageSize">The number of records returned from a single API call. Default is 30.</param>
		/// <param name="nextPageToken">
		/// The next page token paginates through a large set of results.
		/// A next page token is returned whenever the set of available results exceeds the current page size.
		/// </param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>
		/// A task representing the asynchronous operation. The task result contains an array of external contacts in type of <see cref="ExternalContact"/>.
		/// </returns>
		Task<PaginatedResponseWithToken<ExternalContactDetails>> ListExternalContactsAsync(
			int pageSize = 30,
			string nextPageToken = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets an external contact's information.
		/// </summary>
		/// <param name="externalContactId">The external contact id.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains external contact details.</returns>
		Task<ExternalContactDetails> GetExternalContactDetailsAsync(
			string externalContactId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds an external contact.
		/// </summary>
		/// <param name="externalContact">The external contact information.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains external contact details.</returns>
		Task<ExternalContact> AddExternalContactAsync(
			ExternalContactDetails externalContact, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes an external contact.
		/// </summary>
		/// <param name="externalContactId">The external contact id.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task DeleteExternalContactAsync(
			string externalContactId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update an external contact information by <see cref="ExternalContact.ExternalContactId"/>.
		/// </summary>
		/// <param name="externalContact">External contact information.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task UpdateExternalContactAsync(
			ExternalContactDetails externalContact, CancellationToken cancellationToken = default);
	}
}
