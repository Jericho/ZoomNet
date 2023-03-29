using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Webinars : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** WEBINARS *****\n").ConfigureAwait(false);

			// GET ALL THE WEBINARS
			var paginatedWebinars = await client.Webinars.GetAllAsync(myUser.Id, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"There are {paginatedWebinars.TotalRecords} webinars for user {myUser.Id}").ConfigureAwait(false);

			// CLEANUP PREVIOUS INTEGRATION TESTS THAT MIGHT HAVE BEEN INTERRUPTED BEFORE THEY HAD TIME TO CLEANUP AFTER THEMSELVES
			var cleanUpTasks = paginatedWebinars.Records
				.Where(m => m.Topic.StartsWith("ZoomNet Integration Testing:"))
				.Select(async oldWebinar =>
				{
					await client.Webinars.DeleteAsync(oldWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
					await log.WriteLineAsync($"Webinar {oldWebinar.Id} deleted").ConfigureAwait(false);
					await Task.Delay(250, cancellationToken).ConfigureAwait(false);    // Brief pause to ensure Zoom has time to catch up
				});
			await Task.WhenAll(cleanUpTasks).ConfigureAwait(false);

			var templates = await client.Webinars.GetTemplatesAsync(myUser.Id, cancellationToken).ConfigureAwait(false);

			var settings = new WebinarSettings()
			{
				Audio = AudioType.Telephony,
				RegistrationType = RegistrationType.RegisterOnceAttendAll,
				ApprovalType = ApprovalType.Manual
			};
			var trackingFields = new Dictionary<string, string>()
			{
				{ "field1", "value1"},
				{ "field2", "value2"}
			};

			// Scheduled webinar
			var start = DateTime.UtcNow.AddMonths(1);
			var duration = 30;
			var newScheduledWebinar = await client.Webinars.CreateScheduledWebinarAsync(myUser.Id, "ZoomNet Integration Testing: scheduled webinar", "The agenda", start, duration, TimeZones.UTC, "p@ss!w0rd", settings, trackingFields, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {newScheduledWebinar.Id} created").ConfigureAwait(false);

			var updatedSettings = new WebinarSettings() { Audio = AudioType.Voip };
			await client.Webinars.UpdateScheduledWebinarAsync(newScheduledWebinar.Id, topic: "ZoomNet Integration Testing: UPDATED scheduled webinar", settings: updatedSettings, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {newScheduledWebinar.Id} updated").ConfigureAwait(false);

			var scheduledWebinar = (ScheduledWebinar)await client.Webinars.GetAsync(newScheduledWebinar.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {scheduledWebinar.Id} retrieved").ConfigureAwait(false);

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
				new RegistrationCustomQuestionForWebinar
				{
					Title = "Are you happy?",
					Type = RegistrationCustomQuestionTypeForWebinar.SingleRadio,
					IsRequired = true,
					Answers = new[] { "Yes", "No", "Maybe", "I don't know" }
				},
				new RegistrationCustomQuestionForWebinar
				{
					Title = "Tell us about yourself",
					Type = RegistrationCustomQuestionTypeForWebinar.Short,
					IsRequired = false
				}
			};
			await client.Webinars.UpdateRegistrationQuestionsAsync(newScheduledWebinar.Id, requiredFields, optionalFields, customQuestions, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Added {customQuestions.Length} custom registration questions to this webinar.").ConfigureAwait(false);

			var registrationQuestions = await client.Webinars.GetRegistrationQuestionsAsync(newScheduledWebinar.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Here's a quick summary of the registration form for webinar {newScheduledWebinar.Id}:").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.RequiredFields.Length} required fields.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.OptionalFields.Length} optional fields.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.Questions.Count(q => q.IsRequired)} required custom questions.").ConfigureAwait(false);
			await log.WriteLineAsync($"  - there are {registrationQuestions.Questions.Count(q => !q.IsRequired)} optional custom questions.").ConfigureAwait(false);

			var registrants = new List<BatchRegistrant>
				{
					new BatchRegistrant { Email = "firstBatchRegistrant@example.com", FirstName = "Mariful", LastName = "Maruf" },
					new BatchRegistrant { Email = "secondBatchRegistrant@example.com", FirstName = "Abdullah", LastName = "Galib" }
				};
			var registrantsInfo = await client.Webinars.PerformBatchRegistrationAsync(scheduledWebinar.Id, registrants, true).ConfigureAwait(false);
			await log.WriteLineAsync($"Registrants {registrantsInfo} added to meeting {scheduledWebinar.Id}").ConfigureAwait(false);

			var registrationAnswers1 = new[]
			{
				new RegistrationAnswer { Title = "Are you happy?", Answer = "Yes" }
			};
			var registrantInfo1 = await client.Webinars.AddRegistrantAsync(scheduledWebinar.Id, "first@example.com", "John", "Doe", "123 Main street", "Ottawa", Country.Canada, "K1Y 1A1", "Ontario", "5551234567", "Software", "MyOrg", "CEO", PurchasingTimeFrame.Within_a_month, RoleInPurchaseProcess.Influencer, NumberOfEmployees.Between_0501_and_1000, "I don't have any questions at this time", registrationAnswers1, Language.French_France, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registrant {registrantInfo1.Id} added to webinar {scheduledWebinar.Id}").ConfigureAwait(false);

			var registrationAnswers2 = new[]
			{
				new RegistrationAnswer { Title = "Are you happy?", Answer = "No" },
				new RegistrationAnswer { Title = "Tell us about yourself", Answer = "Don't you know who I am?" },
			};
			var registrantInfo2 = await client.Webinars.AddRegistrantAsync(scheduledWebinar.Id, "second@example.com", "Bill", "Murray", "999 5th avanue", "New York City", Country.United_States_of_America, "12345", "Florida", "5551234567", "Entertainment", "Hollywood Industrial Complex", "Comedian", PurchasingTimeFrame.No_timeframe, RoleInPurchaseProcess.Not_Involved, NumberOfEmployees.Between_0001_and_0020, "I have many question that I'll ask during the webinar", registrationAnswers2, Language.English_US, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registrant {registrantInfo2.Id} added to webinar {scheduledWebinar.Id}").ConfigureAwait(false);

			var registrant1 = await client.Webinars.GetRegistrantAsync(scheduledWebinar.Id, registrantInfo1.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieve registrant {registrant1.Id}").ConfigureAwait(false);

			var registrant2 = await client.Webinars.GetRegistrantAsync(scheduledWebinar.Id, registrantInfo2.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Retrieve registrant {registrant2.Id}").ConfigureAwait(false);

			var pendingRegistrations = await client.Webinars.GetRegistrantsAsync(scheduledWebinar.Id, RegistrantStatus.Pending, null, null, 30, null, cancellationToken).ConfigureAwait(false);
			var approvedRegistrations = await client.Webinars.GetRegistrantsAsync(scheduledWebinar.Id, RegistrantStatus.Approved, null, null, 30, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"This webinar has {pendingRegistrations.TotalRecords} registrations awaiting approval and {approvedRegistrations.TotalRecords} approved registrations").ConfigureAwait(false);

			var surveyQuestions = new[]
			{
				new SurveyQuestion
				{
					Name = "Did you like the webinar?",
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
			await client.Webinars.UpdateSurveyAsync(scheduledWebinar.Id, surveyQuestions, true, true, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Survey was updated").ConfigureAwait(false);

			var survey = await client.Webinars.GetSurveyAsync(scheduledWebinar.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Survey was retrieved").ConfigureAwait(false);

			await client.Webinars.DeleteSurveyAsync(scheduledWebinar.Id, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync("Survey was deleted").ConfigureAwait(false);

			await client.Webinars.CancelRegistrantAsync(scheduledWebinar.Id, registrantInfo1.Id, "first@example.com", null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registration {registrant1.Id} was canceled").ConfigureAwait(false);

			await client.Webinars.RejectRegistrantAsync(scheduledWebinar.Id, registrantInfo2.Id, "second@example.com", null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registration {registrant2.Id} was rejected").ConfigureAwait(false);

			await client.Webinars.DeleteRegistrantAsync(scheduledWebinar.Id, registrantInfo1.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registration {registrant1.Id} was deleted").ConfigureAwait(false);

			await client.Webinars.DeleteRegistrantAsync(scheduledWebinar.Id, registrantInfo2.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Registration {registrant1.Id} was deleted").ConfigureAwait(false);

			await client.Webinars.DeleteAsync(newScheduledWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Scheduled webinar {newScheduledWebinar.Id} deleted").ConfigureAwait(false);

			// Recurring webinar
			var recurrenceInfo = new RecurrenceInfo()
			{
				EndTimes = 2,
				WeeklyDays = new[] { DayOfWeek.Monday, DayOfWeek.Friday },
				Type = RecurrenceType.Weekly
			};
			var newRecurringWebinar = await client.Webinars.CreateRecurringWebinarAsync(myUser.Id, "ZoomNet Integration Testing: recurring webinar", "The agenda", start, duration, recurrenceInfo, TimeZones.UTC, "p@ss!w0rd", settings, trackingFields, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} created").ConfigureAwait(false);

			await client.Webinars.UpdateRecurringWebinarAsync(newRecurringWebinar.Id, topic: "ZoomNet Integration Testing: UPDATED recurring webinar", cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} updated").ConfigureAwait(false);

			var recurringWebinar = (RecurringWebinar)await client.Webinars.GetAsync(newRecurringWebinar.Id, null, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {recurringWebinar.Id} retrieved").ConfigureAwait(false);

			var inviteLinks = await client.Webinars.CreateInviteLinksAsync(recurringWebinar.Id, new[] { "Bob", "Bill", "John" }, 7200, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"{inviteLinks.Length} invite links created").ConfigureAwait(false);

			var occurenceId = recurringWebinar.Occurrences[0].OccurrenceId;
			await client.Webinars.UpdateWebinarOccurrenceAsync(newRecurringWebinar.Id, occurenceId, duration: 99, cancellationToken: cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} occurence {occurenceId} updated").ConfigureAwait(false);

			await client.Webinars.DeleteAsync(newRecurringWebinar.Id, null, false, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Recurring webinar {newRecurringWebinar.Id} deleted").ConfigureAwait(false);
		}
	}
}
