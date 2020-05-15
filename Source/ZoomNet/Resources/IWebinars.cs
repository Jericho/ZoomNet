using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage webinars.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/webinars/webinars">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IWebinars
	{
		/// <summary>
		/// Retrieve all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Webinar">webinars</see>.
		/// </returns>
		Task<PaginatedResponse<Webinar>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a scheduled webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="password">Password to join the webinar. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		Task<Webinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a recurring webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the webinar. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		Task<RecurringWebinar> CreateRecurringWebinarAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);
	}
}
