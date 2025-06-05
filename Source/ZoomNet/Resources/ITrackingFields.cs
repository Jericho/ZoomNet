using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage tracking fields on your Zoom account.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/methods/#tag/Tracking-Field">Zoom documentation</a> for more information.
	/// </remarks>
	public interface ITrackingFields
	{
		/// <summary>
		/// Create a tracking field.
		/// </summary>
		/// <param name="name">Name for the tracking field.</param>
		/// <param name="recommendedValues">Enumeration of recommended values.</param>
		/// <param name="isRequired">Indicates whether the tracking field is required.</param>
		/// <param name="isVisible">Indicates whether the tracking field is visible.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The newly created <see cref="TrackingField">tracking field</see>.</returns>
		Task<TrackingField> CreateAsync(string name, IEnumerable<string> recommendedValues = null, bool? isRequired = null, bool? isVisible = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a tracking field.
		/// </summary>
		/// <param name="trackingFieldId">The tracking field unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteAsync(string trackingFieldId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a tracking fields.
		/// </summary>
		/// <param name="trackingFieldId">The tracking field unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="TrackingField">tracking field</see>.</returns>
		Task<TrackingField> GetAsync(string trackingFieldId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all the tracking fields on your Zoom account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="TrackingField">tracking fields</see>.</returns>
		Task<TrackingField[]> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a tracking field.
		/// </summary>
		/// <param name="trackingFieldId">The tracking field unique identifier.</param>
		/// <param name="name">Name for the tracking field.</param>
		/// <param name="recommendedValues">Enumeration of recommended values.</param>
		/// <param name="isRequired">Indicates whether the tracking field is required.</param>
		/// <param name="isVisible">Indicates whether the tracking field is visible.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateAsync(string trackingFieldId, string name = null, IEnumerable<string> recommendedValues = null, bool? isRequired = null, bool? isVisible = null, CancellationToken cancellationToken = default);
	}
}
