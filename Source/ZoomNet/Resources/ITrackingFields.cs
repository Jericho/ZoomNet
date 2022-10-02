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
		/// Retrieve all the tracking fields on your Zoom account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="TrackingField">tracking fields</see>.
		/// </returns>
		Task<TrackingField[]> GetAllAsync(CancellationToken cancellationToken = default);
	}
}
