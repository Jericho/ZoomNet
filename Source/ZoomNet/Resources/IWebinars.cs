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
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/webinars/">Zoom documentation</a> for more information.
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
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Webinar>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Webinar">webinars</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Webinar>> GetAllAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		Task<ScheduledWebinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

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

		/// <summary>
		/// Update the details of a webinar occurrence.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateWebinarOccurrenceAsync(long webinarId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, WebinarSettings settings = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing scheduled webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateScheduledWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing recurring webinar for a user.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRecurringWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, RecurrenceInfo recurrence = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the details of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Webinar" />.
		/// </returns>
		Task<Webinar> GetAsync(long webinarId, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="sendNotification">If true, a notification email is sent to the panelists and registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAsync(long webinarId, string occurrenceId = null, bool sendNotification = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// End a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task EndAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// List panelists of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Panelist"/>.
		/// </returns>
		Task<Panelist[]> GetPanelistsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add a single panelist to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="email">Panelist's email address.</param>
		/// <param name="fullName">Panelist's full name.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Panelist" />.
		/// </returns>
		Task<Panelist> AddPanelistAsync(long webinarId, string email, string fullName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add multiple panelists to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="panelists">The panelists to add to the webinar.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task<Panelist[]> AddPanelistsAsync(long webinarId, IEnumerable<(string Email, string FullName)> panelists, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove a single panelist from a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="panelistId">Panelist's email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RemovePanelistAsync(long webinarId, string panelistId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove all panelists from a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RemoveAllPanelistsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// List the users that have registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// List the users that have registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get details on a specific user who registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Registrant" />.
		/// </returns>
		Task<Registrant> GetRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add a registrant to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="email">A valid email address.</param>
		/// <param name="firstName">User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Registrant" />.
		/// </returns>
		Task<Registrant> AddRegistrantAsync(long webinarId, string email, string firstName, string lastName, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve a registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve multiple registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be approved.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject a registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject multiple registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be rejected.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancel a previously approved registration for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task CancelRegistrantAsync(long webinarId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancel multiple previously approved registrations for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be cancelled.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task CancelRegistrantsAsync(long webinarId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all polls for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Poll" />.
		/// </returns>
		Task<Poll[]> GetPollsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task<Poll> CreatePoll(long webinarId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a poll.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Poll" />.
		/// </returns>
		Task<Poll> GetPollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdatePollAsync(long webinarId, long pollId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a poll for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeletePollAsync(long webinarId, long pollId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollQuestion"/>.
		/// </returns>
		Task<PollQuestion[]> GetRegistrationQuestions(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="customQuestions">The questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRegistrationQuestions(long webinarId, IEnumerable<PollQuestion> customQuestions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all the tracking sources of a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="TrackingSource" />.
		/// </returns>
		Task<TrackingSource[]> GetTrackingSourcesAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all the templates created for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Template" />.
		/// </returns>
		Task<Template[]> GetWebinarTemplatesAsync(string userId, CancellationToken cancellationToken = default);
	}
}
