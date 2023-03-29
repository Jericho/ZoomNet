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
		/// Retrieve summary information about all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="WebinarSummary">webinar summaries</see>.
		/// </returns>
		/// <remarks>
		/// To obtain the full details about a given webinar you must invoke <see cref="Webinars.GetAsync(long, string, CancellationToken)"/>.
		/// </remarks>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<WebinarSummary>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve summary information about all webinars for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="WebinarSummary">webinar summaries</see>.
		/// </returns>
		/// <remarks>
		/// To obtain the full details about a given webinar you must invoke <see cref="Webinars.GetAsync(long, string, CancellationToken)"/>.
		/// </remarks>
		Task<PaginatedResponseWithToken<WebinarSummary>> GetAllAsync(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a scheduled webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		Task<ScheduledWebinar> CreateScheduledWebinarAsync(string userId, string topic, string agenda, DateTime start, int duration, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a recurring webinar for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer. If passed in, Zoom advise using the userId in the <paramref name="userId"/> field, rather than email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new webinar.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the webinar.</exception>
		Task<RecurringWebinar> CreateRecurringWebinarAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, TimeZones? timeZone = TimeZones.UTC, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the details of a webinar occurrence.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateWebinarOccurrenceAsync(long webinarId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, WebinarSettings settings = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing scheduled webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateScheduledWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing recurring webinar for a user.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="topic">Webinar topic.</param>
		/// <param name="agenda">Webinar description.</param>
		/// <param name="start">Webinar start time.</param>
		/// <param name="duration">Webinar duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the webinar. By default the password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters. This can be updated within Zoom account settings.</param>
		/// <param name="settings">Webinar settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRecurringWebinarAsync(long webinarId, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, RecurrenceInfo recurrence = null, string password = null, WebinarSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

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
		/// <param name="virtualBackgroundId">The virtual background ID to bind.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task AddPanelistAsync(long webinarId, string email, string fullName, string virtualBackgroundId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add multiple panelists to a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="panelists">The panelists to add to the webinar.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task AddPanelistsAsync(long webinarId, IEnumerable<(string Email, string FullName, string VirtualBackgroundId)> panelists, CancellationToken cancellationToken = default);

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
		/// <param name="trackingSourceId">The tracking source ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string trackingSourceId = null, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// List the users that have registered for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="trackingSourceId">The tracking source ID.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long webinarId, RegistrantStatus status, string trackingSourceId = null, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

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
		/// <param name="firstName">Registrant's first name.</param>
		/// <param name="lastName">Registrant's last name.</param>
		/// <param name="address">Registrant's address.</param>
		/// <param name="city">Registrant's city.</param>
		/// <param name="country">Registrant's country.</param>
		/// <param name="postalCode">Registrant's zip or postal code.</param>
		/// <param name="stateOrProvince">Registrant's state or province.</param>
		/// <param name="phoneNumber">Registrant's phone number.</param>
		/// <param name="industry">Registrant's industry.</param>
		/// <param name="organization">Registrant's organization.</param>
		/// <param name="jobTitle">Registrant's job title.</param>
		/// <param name="timeFrame">This field can be used to gauge interest of attendees towards buying your product or service.</param>
		/// <param name="role">Registrant's role in purchase decision.</param>
		/// <param name="employees">Number of employees.</param>
		/// <param name="comments">A field that allows registrant to provide any questions or comments that they might have.</param>
		/// <param name="questionAnswers">Answers to the custom registration questions.</param>
		/// <param name="language">Registrant's language preference for confirmation emails.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="RegistrantInfo" />.
		/// </returns>
		Task<RegistrantInfo> AddRegistrantAsync(long webinarId, string email, string firstName, string lastName, string address = null, string city = null, Country? country = null, string postalCode = null, string stateOrProvince = null, string phoneNumber = null, string industry = null, string organization = null, string jobTitle = null, PurchasingTimeFrame? timeFrame = null, RoleInPurchaseProcess? role = null, NumberOfEmployees? employees = null, string comments = null, IEnumerable<RegistrationAnswer> questionAnswers = null, Language? language = null, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Register up to 30 registrants at once for a webinar that requires registration.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrants">An array of registrants.</param>
		/// <param name="autoApprove">Indicates if the registrant should be automatically approved.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="BatchRegistrantInfo" />.
		/// </returns>
		Task<BatchRegistrantInfo[]> PerformBatchRegistrationAsync(long webinarId, IEnumerable<BatchRegistrant> registrants, bool autoApprove = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a webinar registrant.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="registrantId">The registrant id.</param>
		/// <param name="occurrenceId">The webinar occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteRegistrantAsync(long webinarId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default);

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
		/// An array of <see cref="RegistrationCustomQuestionForWebinar"/>.
		/// </returns>
		Task<RegistrationQuestionsForWebinar> GetRegistrationQuestionsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="requiredFields">List of fields that must be answer when registering for the webinar.</param>
		/// <param name="optionalFields">List of fields that can be answer when registering for the webinar.</param>
		/// <param name="customQuestions">Additional questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRegistrationQuestionsAsync(long webinarId, IEnumerable<RegistrationField> requiredFields, IEnumerable<RegistrationField> optionalFields, IEnumerable<RegistrationCustomQuestionForWebinar> customQuestions, CancellationToken cancellationToken = default);

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
		/// An array of <see cref="WebinarTemplate" />.
		/// </returns>
		Task<WebinarTemplate[]> GetTemplatesAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a webinar's live stream information.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="streamUrl">Streaming URL.</param>
		/// <param name="streamKey">Stream name and key.</param>
		/// <param name="pageUrl">The live stream page URL.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateLiveStreamAsync(long webinarId, string streamUrl, string streamKey, string pageUrl, CancellationToken cancellationToken = default);

		/// <summary>
		/// Start a webinar's live stream.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="displaySpeakerName">Display the name of the active speaker during a live stream.</param>
		/// <param name="speakerName">The name of the speaker.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StartLiveStreamAsync(long webinarId, bool displaySpeakerName, string speakerName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Stop a webinar's live stream.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StopLiveStreamAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the details of a webinar's live stream.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task<LiveStreamingSettings> GetLiveStreamSettingsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a batch of invitation links for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="names">The display name of the attendees.</param>
		/// <param name="timeToLive">The invite link's expiration time, in seconds.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="InviteLink" />.
		/// </returns>
		Task<InviteLink[]> CreateInviteLinksAsync(long webinarId, IEnumerable<string> names, long timeToLive = 7200, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a webinar survey.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteSurveyAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the survey for a webinar.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Survey"/>.
		/// </returns>
		Task<Survey> GetSurveyAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a webinar's survey.
		/// </summary>
		/// <param name="webinarId">The webinar ID.</param>
		/// <param name="questions">The custom questions.</param>
		/// <param name="allowAnonymous">Whether to allow participants to anonymously answer survey questions.</param>
		/// <param name="showInBrowser">Whether the survey will be displayed in the attendee's browser.</param>
		/// <param name="thirdPartySurveyLink">The link to the third party webinar survey.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateSurveyAsync(long webinarId, IEnumerable<SurveyQuestion> questions = null, bool allowAnonymous = true, bool showInBrowser = true, string thirdPartySurveyLink = null, CancellationToken cancellationToken = default);
	}
}
