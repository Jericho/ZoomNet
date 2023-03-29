using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Meetings : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** MEETINGS *****\n").ConfigureAwait(false);

			// GET ALL THE MEETINGS
			var paginatedScheduledMeetings = await client.Meetings.GetAllAsync(myUser.Id, MeetingListType.Scheduled, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedScheduledMeetings.TotalRecords} scheduled meetings").ConfigureAwait(false);

			var paginatedLiveMeetings = await client.Meetings.GetAllAsync(myUser.Id, MeetingListType.Live, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedLiveMeetings.TotalRecords} live meetings").ConfigureAwait(false);

			var paginatedUpcomingMeetings = await client.Meetings.GetAllAsync(myUser.Id, MeetingListType.Upcoming, 100, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedUpcomingMeetings.TotalRecords} upcoming meetings").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedScheduledMeetings.Records
				.Union(paginatedLiveMeetings.Records)
				.Union(paginatedUpcomingMeetings.Records)
				.Where(m => m.Topic.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldMeeting =>
				{
					await client.Meetings.DeleteAsync(oldMeeting.Id, null, false, false, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Meeting {oldMeeting.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			// For an unknown reason, using myUser.Id to retrieve meeting templates causes an "Invalid token" exception.
			// That's why I use "me" on the following line:
			var templates = await client.Meetings.GetTemplatesAsync("me", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieved {templates.Length} meeting templates").ConfigureAwait(false);

			var settings = new MeetingSettings()
			{
				Audio = AudioType.Telephony,
				RegistrationType = RegistrationType.RegisterOnceAttendAll,
				ApprovalType = ApprovalType.Manual,
				JoinBeforeHost = true,
				JoinBeforeHostTime = JoinBeforeHostTime.FiveMinutes,
			};
			var trackingFields = new Dictionary<string, string>()
			{
				{ "field1", "value1"},
				{ "field2", "value2"}
			};

			// Instant meeting
			var newInstantMeeting = await client.Meetings.CreateInstantMeetingAsync(myUser.Id, "ZoomNet Integration Testing: instant meeting", "The agenda", "p@ss!w0rd", settings, trackingFields, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} created").ConfigureAwait(false);

			var instantMeeting = (InstantMeeting)await client.Meetings.GetAsync(newInstantMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {instantMeeting.Id} retrieved").ConfigureAwait(false);

			var localRecordingToken = await client.Meetings.GetTokenForLocalRecordingAsync(newInstantMeeting.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"The token for local recording is: {localRecordingToken}").ConfigureAwait(false);

			await client.Meetings.EndAsync(newInstantMeeting.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} ended").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(newInstantMeeting.Id, null, false, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Instant meeting {newInstantMeeting.Id} deleted").ConfigureAwait(false);

			// Scheduled meeting
			var start = DateTime.UtcNow.AddMonths(1);
			var duration = 30;
			var newScheduledMeeting = await client.Meetings.CreateScheduledMeetingAsync(myUser.Id, "ZoomNet Integration Testing: scheduled meeting", "The agenda", start, duration, TimeZones.UTC, "p@ss!w0rd", settings, trackingFields, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {newScheduledMeeting.Id} created").ConfigureAwait(false);

			var updatedSettings = new MeetingSettings() { Audio = AudioType.Voip };
			await client.Meetings.UpdateScheduledMeetingAsync(newScheduledMeeting.Id, topic: "ZoomNet Integration Testing: UPDATED scheduled meeting", settings: updatedSettings, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {newScheduledMeeting.Id} updated").ConfigureAwait(false);

			var scheduledMeeting = (ScheduledMeeting)await client.Meetings.GetAsync(newScheduledMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {scheduledMeeting.Id} retrieved").ConfigureAwait(false);

			var requiredFields = new[]
			{
				RegistrationField.PurchasingTimeFrame,
				RegistrationField.RoleInPurchaseProcess
			};
			var optionalFields = new[]
			{
				RegistrationField.Address,
				RegistrationField.City,
				RegistrationField.Country,
				RegistrationField.PostalCode,
				RegistrationField.State,
				RegistrationField.Phone,
				RegistrationField.Industry,
				RegistrationField.Organization,
				RegistrationField.JobTitle,
				RegistrationField.NumberOfEmployees,
				RegistrationField.Comments
			};
			var customQuestions = new[]
			{
				new RegistrationCustomQuestionForMeeting
				{
					Title = "Are you happy?",
					Type = RegistrationCustomQuestionTypeForMeeting.Single,
					IsRequired = true,
					Answers = new[] { "Yes", "No", "Maybe", "I don't know" }
				},
				new RegistrationCustomQuestionForMeeting
				{
					Title = "Tell us about yourself",
					Type = RegistrationCustomQuestionTypeForMeeting.Short,
					IsRequired = false
				}
			};
			await client.Meetings.UpdateRegistrationQuestionsAsync(newScheduledMeeting.Id, requiredFields, optionalFields, customQuestions, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Added {customQuestions.Length} custom registration questions to this meeting.").ConfigureAwait(false);

			var registrationQuestions = await client.Meetings.GetRegistrationQuestionsAsync(newScheduledMeeting.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Here's a quick summary of the registration form for meeting {newScheduledMeeting.Id}:").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.RequiredFields.Length} required fields.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.OptionalFields.Length} optional fields.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.Questions.Count(q => q.IsRequired)} required custom questions.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.Questions.Count(q => !q.IsRequired)} optional custom questions.").ConfigureAwait(false);

			if (myUser.Type == UserType.Licensed)
			{
				var registrants = new List<BatchRegistrant>
				{
					new BatchRegistrant { Email = "firstBatchRegistrant@example.com", FirstName = "Mariful", LastName = "Maruf" },
					new BatchRegistrant { Email = "secondBatchRegistrant@example.com", FirstName = "Abdullah", LastName = "Galib" }
				};
				var registrantsInfo = await client.Meetings.PerformBatchRegistrationAsync(scheduledMeeting.Id, registrants, true).ConfigureAwait(false);
				await log.WriteLineAsync($"Registrants {registrantsInfo} added to meeting {scheduledMeeting.Id}").ConfigureAwait(false);

				var registrationAnswers1 = new[]
				{
					new RegistrationAnswer { Title = "Are you happy?", Answer = "Yes" }
				};
				var registrantInfo1 = await client.Meetings.AddRegistrantAsync(scheduledMeeting.Id, "first@example.com", "John", "Doe", "123 Main street", "Ottawa", Country.Canada, "K1Y 1A1", "Ontario", "5551234567", "Software", "MyOrg", "CEO", PurchasingTimeFrame.Within_a_month, RoleInPurchaseProcess.Influencer, NumberOfEmployees.Between_0501_and_1000, "I don't have any questions at this time", registrationAnswers1, Language.French_France, true, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registrant {registrantInfo1.Id} added to meeting {scheduledMeeting.Id}").ConfigureAwait(false);

				var registrationAnswers2 = new[]
				{
					new RegistrationAnswer { Title = "Are you happy?", Answer = "No" },
					new RegistrationAnswer { Title = "Tell us about yourself", Answer = "Don't you know who I am?" },
				};
				var registrantInfo2 = await client.Meetings.AddRegistrantAsync(scheduledMeeting.Id, "second@example.com", "Bill", "Murray", "999 5th avanue", "New York City", Country.United_States_of_America, "12345", "Florida", "5551234567", "Entertainment", "Hollywood Industrial Complex", "Comedian", PurchasingTimeFrame.No_timeframe, RoleInPurchaseProcess.Not_Involved, NumberOfEmployees.Between_0001_and_0020, "I have many question that I'll ask during the meeting", registrationAnswers2, Language.English_US, false, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registrant {registrantInfo2.Id} added to meeting {scheduledMeeting.Id}").ConfigureAwait(false);

				var registrant1 = await client.Meetings.GetRegistrantAsync(scheduledMeeting.Id, registrantInfo1.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Retrieve registrant {registrant1.Id}").ConfigureAwait(false);

				var registrant2 = await client.Meetings.GetRegistrantAsync(scheduledMeeting.Id, registrantInfo2.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Retrieve registrant {registrant2.Id}").ConfigureAwait(false);

				var pendingRegistrations = await client.Meetings.GetRegistrantsAsync(scheduledMeeting.Id, RegistrantStatus.Pending, null, 30, null, cancellationToken).ConfigureAwait(false);
				var approvedRegistrations = await client.Meetings.GetRegistrantsAsync(scheduledMeeting.Id, RegistrantStatus.Approved, null, 30, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"This meeting has {pendingRegistrations.TotalRecords} registrations awaiting approval and {approvedRegistrations.TotalRecords} approved registrations").ConfigureAwait(false);

				await client.Meetings.CancelRegistrantAsync(scheduledMeeting.Id, registrantInfo1.Id, "first@example.com", null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registration {registrant1.Id} was canceled").ConfigureAwait(false);

				await client.Meetings.RejectRegistrantAsync(scheduledMeeting.Id, registrantInfo2.Id, "second@example.com", null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registration {registrant2.Id} was rejected").ConfigureAwait(false);

				await client.Meetings.DeleteRegistrantAsync(scheduledMeeting.Id, registrantInfo1.Id, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registration {registrant1.Id} was deleted").ConfigureAwait(false);

				await client.Meetings.DeleteRegistrantAsync(scheduledMeeting.Id, registrantInfo2.Id, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync($"Registration {registrant1.Id} was deleted").ConfigureAwait(false);

				var surveyQuestions = new[]
				{
					new SurveyQuestion
					{
						Name = "Did you like the meeting?",
						IsRequired = true,
						ShowAsDropdown = false,
						Type = SurveyQuestionType.Single,
						Answers = new[] { "Yes", "No"}
					},
					new SurveyQuestion
					{
						Name = "Rate the presenter",
						IsRequired = true,
						ShowAsDropdown = false,
						Type = SurveyQuestionType.Rating_Scale,
						RatingMinimumValue = 1,
						RatingMaximumValue = 10,
						RatingLowScoreLabel = "Mediocre",
						RatingHighScoreLabel = "Excellent"
					},
					new SurveyQuestion
					{
						Name = "Select the product(s) that interest you",
						IsRequired = false,
						ShowAsDropdown = false,
						Type = SurveyQuestionType.Multiple,
						Answers = new[] { "Gizmo number 1", "Gizmo number 2", "SuperDuper Gizmo"}
					},
					new SurveyQuestion
					{
						Name = "Comment",
						IsRequired = false,
						Type = SurveyQuestionType.Long,
						MinimumNumberOfCharacters = 10,
						MaximumNumberOfCharacters = 1999
					},
				};
				await client.Meetings.UpdateSurveyAsync(scheduledMeeting.Id, surveyQuestions, true, true, null, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Survey was updated").ConfigureAwait(false);

				var survey = await client.Meetings.GetSurveyAsync(scheduledMeeting.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Survey was retrieved").ConfigureAwait(false);

				await client.Meetings.DeleteSurveyAsync(scheduledMeeting.Id, cancellationToken).ConfigureAwait(false);
				await log.WriteLineAsync("Survey was deleted").ConfigureAwait(false);
			}

			await client.Meetings.DeleteAsync(newScheduledMeeting.Id, null, false, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled meeting {newScheduledMeeting.Id} deleted").ConfigureAwait(false);

			// Recurring meeting
			var recurrenceInfo = new RecurrenceInfo()
			{
				EndDateTime = DateTime.Now.AddMonths(2),
				EndTimes = 2,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Friday },
				Type = RecurrenceType.Weekly
			};
			var newRecurringMeeting = await client.Meetings.CreateRecurringMeetingAsync(myUser.Id, "ZoomNet Integration Testing: recurring meeting", "The agenda", start, duration, recurrenceInfo, TimeZones.UTC, "p@ss!w0rd", settings, trackingFields, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} created").ConfigureAwait(false);

			await client.Meetings.UpdateRecurringMeetingAsync(newRecurringMeeting.Id, topic: "ZoomNet Integration Testing: UPDATED recurring meeting", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} updated").ConfigureAwait(false);

			var recurringMeeting = (RecurringMeeting)await client.Meetings.GetAsync(newRecurringMeeting.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {recurringMeeting.Id} retrieved").ConfigureAwait(false);

			var inviteLinks = await client.Meetings.CreateInviteLinksAsync(recurringMeeting.Id, new[] { "Bob", "Bill", "John" }, 7200, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"{inviteLinks.Length} invite links created").ConfigureAwait(false);

			var occurenceId = recurringMeeting.Occurrences[0].OccurrenceId;
			await client.Meetings.UpdateMeetingOccurrenceAsync(newRecurringMeeting.Id, occurenceId, duration: 99, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} occurence {occurenceId} updated").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(newRecurringMeeting.Id, null, false, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting {newRecurringMeeting.Id} deleted").ConfigureAwait(false);

			// Recurring meeting with no fixed time
			var newRecurringNoFixTimeMeeting = await client.Meetings.CreateRecurringMeetingAsync(myUser.Id, "ZoomNet Integration Testing: recurring meeting with no fixed time", "The agenda", start, duration, null, TimeZones.UTC, "p@ss!w0rd", settings, null, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting with no fixed time {newRecurringNoFixTimeMeeting.Id} created").ConfigureAwait(false);

			await client.Meetings.DeleteAsync(newRecurringNoFixTimeMeeting.Id, null, false, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring meeting with no fixed time  {newRecurringNoFixTimeMeeting.Id} deleted").ConfigureAwait(false);
		}
	}
}
