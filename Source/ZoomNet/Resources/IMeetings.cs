using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage meetings.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/meetings">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IMeetings
	{
		/// <summary>
		/// Retrieve summary information about all meetings of the specified type for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="type">The type of meetings. Allowed values: Scheduled, Live, Upcoming.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingSummary">meeting summaries</see>.
		/// </returns>
		/// <remarks>
		/// To obtain the full details about a given meeting you must invoke <see cref="Meetings.GetAsync(long, string, CancellationToken)"/>.
		/// </remarks>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<MeetingSummary>> GetAllAsync(string userId, MeetingListType type = MeetingListType.Scheduled, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve summary information about all meetings of the specified type for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="type">The type of meetings. Allowed values: Scheduled, Live, Upcoming.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingSummary">meeting summaries</see>.
		/// </returns>
		/// <remarks>
		/// To obtain the full details about a given meeting you must invoke <see cref="Meetings.GetAsync(long, string, CancellationToken)"/>.
		/// </remarks>
		Task<PaginatedResponseWithToken<MeetingSummary>> GetAllAsync(string userId, MeetingListType type = MeetingListType.Scheduled, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates an instant meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		Task<InstantMeeting> CreateInstantMeetingAsync(string userId, string topic, string agenda, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a scheduled meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		Task<ScheduledMeeting> CreateScheduledMeetingAsync(string userId, string topic, string agenda, DateTime start, int duration, TimeZones? timeZone = TimeZones.UTC, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a recurring meeting for a user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time. If omitted, a 'Recurring meeting with no fixed time' will be created.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="templateId">Template Identifer.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new meeting.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the meeting.</exception>
		Task<RecurringMeeting> CreateRecurringMeetingAsync(string userId, string topic, string agenda, DateTime? start, int duration, RecurrenceInfo recurrence, TimeZones? timeZone = TimeZones.UTC, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, string templateId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the details of a meeting occurrence.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateMeetingOccurrenceAsync(long meetingId, string occurrenceId, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, MeetingSettings settings = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the details of a scheduled meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateScheduledMeetingAsync(long meetingId, string userId = null, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the details of a recurring meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="topic">Meeting topic.</param>
		/// <param name="agenda">Meeting description.</param>
		/// <param name="start">Meeting start time. If omitted, a 'Recurring meeting with no fixed time' will be created.</param>
		/// <param name="duration">Meeting duration (minutes).</param>
		/// <param name="timeZone">The time zone for start time.</param>
		/// <param name="recurrence">Recurrence information.</param>
		/// <param name="password">Password to join the meeting. Password may only contain the following characters: [a-z A-Z 0-9 @ - _ *]. Max of 10 characters.</param>
		/// <param name="settings">Meeting settings.</param>
		/// <param name="trackingFields">Tracking fields.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRecurringMeetingAsync(long meetingId, string userId = null, string topic = null, string agenda = null, DateTime? start = null, int? duration = null, TimeZones? timeZone = null, RecurrenceInfo recurrence = null, string password = null, MeetingSettings settings = null, IDictionary<string, string> trackingFields = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		Task<Meeting> GetAsync(long meetingId, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="notifyHost">If true, a notification email is sent to the host and alternative host.</param>
		/// <param name="notifyRegistrants">If true, a notification email is sent to the registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAsync(long meetingId, string occurrenceId = null, bool notifyHost = true, bool notifyRegistrants = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// End a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task EndAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Recover a deleted meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task RecoverAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// List registrants of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		[Obsolete("Zoom is in the process of deprecating the \"page number\" and \"page count\" fields.")]
		Task<PaginatedResponse<Registrant>> GetRegistrantsAsync(long meetingId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);

		/// <summary>
		/// List registrants of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="status">The registrant status.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		Task<PaginatedResponseWithToken<Registrant>> GetRegistrantsAsync(long meetingId, RegistrantStatus status, string occurrenceId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add a registrant to a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
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
		/// <param name="autoApprove">Indicates if the registrant should be automatically approved.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="RegistrantInfo" />.
		/// </returns>
		Task<RegistrantInfo> AddRegistrantAsync(long meetingId, string email, string firstName, string lastName, string address = null, string city = null, Country? country = null, string postalCode = null, string stateOrProvince = null, string phoneNumber = null, string industry = null, string organization = null, string jobTitle = null, PurchasingTimeFrame? timeFrame = null, RoleInPurchaseProcess? role = null, NumberOfEmployees? employees = null, string comments = null, IEnumerable<RegistrationAnswer> questionAnswers = null, Language? language = null, bool autoApprove = false, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Register up to 30 registrants at once for a meeting that requires registration.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrants">An array of registrants.</param>
		/// <param name="autoApprove">Indicates if the registrant should be automatically approved.</param>
		/// <param name="registrantsConfirmationEmail">Indicates if send confirmation Email to Registrants.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="BatchRegistrantInfo" />.
		/// </returns>
		Task<BatchRegistrantInfo[]> PerformBatchRegistrationAsync(long meetingId, IEnumerable<BatchRegistrant> registrants, bool autoApprove = false, bool registrantsConfirmationEmail = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a meeting registrant.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant id.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteRegistrantAsync(long meetingId, string registrantId, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a meeting registrant.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Registrant"/>.
		/// </returns>
		Task<Registrant> GetRegistrantAsync(long meetingId, string registrantId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Approve multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be approved.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ApproveRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject a registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Reject multiple registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be rejected.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task RejectRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancel a previously approved registration for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantId">The registrant ID.</param>
		/// <param name="registrantEmail">The registrant's email address.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task CancelRegistrantAsync(long meetingId, string registrantId, string registrantEmail, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Cancel multiple previously approved registrations for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="registrantsInfo">ID and email for each registrant to be cancelled.</param>
		/// <param name="occurrenceId">The meeting occurrence id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task CancelRegistrantsAsync(long meetingId, IEnumerable<(string RegistrantId, string RegistrantEmail)> registrantsInfo, string occurrenceId = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all polls for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Poll" />.
		/// </returns>
		Task<Poll[]> GetPollsAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task<Poll> CreatePollAsync(long meetingId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a poll.
		/// </summary>
		/// <param name="meetingId">The meeting id.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Poll" />.
		/// </returns>
		Task<Poll> GetPollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="title">Title for the poll.</param>
		/// <param name="questions">The poll questions.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdatePollAsync(long meetingId, long pollId, string title, IEnumerable<PollQuestion> questions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a poll for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="pollId">The poll id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeletePollAsync(long meetingId, long pollId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the questions that are to be answered by users while registering for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="RegistrationCustomQuestionForMeeting"/>.
		/// </returns>
		Task<RegistrationQuestionsForMeeting> GetRegistrationQuestionsAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update the questions that are to be answered by users while registering for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="requiredFields">List of fields that must be answer when registering for the meeting.</param>
		/// <param name="optionalFields">List of fields that can be answer when registering for the meeting.</param>
		/// <param name="customQuestions">Additional questions to be answered.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRegistrationQuestionsAsync(long meetingId, IEnumerable<RegistrationField> requiredFields, IEnumerable<RegistrationField> optionalFields, IEnumerable<RegistrationCustomQuestionForMeeting> customQuestions, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the meeting invite note that was sent for a specific meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The invite note.
		/// </returns>
		Task<string> GetInvitationAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a meeting's live stream information.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="streamUrl">Streaming URL.</param>
		/// <param name="streamKey">Stream name and key.</param>
		/// <param name="pageUrl">The live stream page URL.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateLiveStreamAsync(long meetingId, string streamUrl, string streamKey, string pageUrl, CancellationToken cancellationToken = default);

		/// <summary>
		/// Start a meeting's live stream.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="displaySpeakerName">Display the name of the active speaker during a live stream.</param>
		/// <param name="speakerName">The name of the speaker.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StartLiveStreamAsync(long meetingId, bool displaySpeakerName, string speakerName, CancellationToken cancellationToken = default);

		/// <summary>
		/// Stop a meeting's live stream.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StopLiveStreamAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get the details of a meeting's live stream.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task<LiveStreamingSettings> GetLiveStreamSettingsAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all the templates that are available to be used by a user.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingTemplate" />.
		/// </returns>
		Task<MeetingTemplate[]> GetTemplatesAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a batch of invitation links for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="names">The display name of the attendees.</param>
		/// <param name="timeToLive">The invite link's expiration time, in seconds.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="InviteLink" />.
		/// </returns>
		Task<InviteLink[]> CreateInviteLinksAsync(long meetingId, IEnumerable<string> names, long timeToLive = 7200, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a meeting survey.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteSurveyAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the survey for a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// A <see cref="Survey"/>.
		/// </returns>
		Task<Survey> GetSurveyAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a meeting's survey.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="questions">The custom questions.</param>
		/// <param name="allowAnonymous">Whether to allow participants to anonymously answer survey questions.</param>
		/// <param name="showInBrowser">Whether the survey will be displayed in the attendee's browser.</param>
		/// <param name="thirdPartySurveyLink">The link to the third party meeting survey.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateSurveyAsync(long meetingId, IEnumerable<SurveyQuestion> questions = null, bool allowAnonymous = true, bool showInBrowser = true, string thirdPartySurveyLink = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Start recording a live meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StartCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Pause recording a live meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task PauseCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Resume recording a live meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task ResumeCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Stop recording a live meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task StopCloudRecordingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Invite multiple participants to join a live meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="emailAddresses">The email addresses of the people you want to invite.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task InviteParticipantsAsync(long meetingId, IEnumerable<string> emailAddresses, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a meeting template from an existing meeting.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="templateName">The template name.</param>
		/// <param name="saveRecurrence">Indicates whether the recurrence meeting template will be saved as the scheduled meeting or not.</param>
		/// <param name="overwrite">Indicates whether an existing meeting template from the same meeting should be overwritten or not.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The template ID.</returns>
		Task<string> CreateTemplateFromExistingMeeting(string userId, long meetingId, string templateName, bool saveRecurrence = false, bool overwrite = false, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a meeting's closed caption token.
		/// This token lets you use a third-party service to stream text to their closed captioning software to the Zoom meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The token.</returns>
		Task<string> GetTokenForClosedCaptioningAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a meeting's join token to allow for local recording.
		/// The join token lets a recording bot implemented using Zoom Meeting SDK to connect to a Zoom meeting.
		/// The recording bot can then automatically start locally recording.
		/// This supports both regular and raw local recording types.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The join token.</returns>
		Task<string> GetTokenForLocalRecordingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a meeting's archive token to allow local archiving.
		/// The archive token allows a meeting SDK app or bot to get archive permission to access the meeting's raw audio and video media stream in real-time.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The token.</returns>
		Task<string> GetTokenForLocalArchivingAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a meeting's join token to allow live streaming.
		/// The join token allows a recording bot implemented using Zoom meeting SDK to connect to a Zoom meeting "hosted by the issuer of the token", and can call the streaming method automatically.
		/// It supports both regular live streaming, and raw streaming.
		/// </summary>
		/// <param name="meetingId">The meeting ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The token.</returns>
		Task<string> GetTokenForLiveStreamingAsync(long meetingId, CancellationToken cancellationToken = default);
	}
}
