using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify user TSP webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region tests

		[Fact]
		public void UserTspCreated()
		{
			var parsedEvent = ParseWebhookEvent<UserTspCreatedEvent>(WebhooksDataResource.user_tsp_created);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspCreated);

			VerifyUserTspEvent(parsedEvent);
		}

		[Fact]
		public void UserTspDeleted()
		{
			var parsedEvent = ParseWebhookEvent<UserTspDeletedEvent>(WebhooksDataResource.user_tsp_deleted);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspDeleted);

			VerifyUserTspEvent(parsedEvent);
		}

		[Fact]
		public void UserTspUpdated()
		{
			var parsedEvent = ParseWebhookEvent<UserTspUpdatedEvent>(WebhooksDataResource.user_tsp_updated);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspUpdated);

			VerifyUserTspEvent(parsedEvent, dialInNumbersCount: 1);

			VerifyTspAccount(parsedEvent.OldAccount, conferenceCode: "0130", leaderPin: "11188124", tspBridge: string.Empty, dialInNumbersCount: 1);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="UserTspEvent"/> properties.
		/// </summary>
		private static void VerifyUserTspEvent(UserTspEvent parsedEvent, int dialInNumbersCount = 3)
		{
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe(OperatorId);

			VerifyUserTspAccount(parsedEvent.UserTspAccount, dialInNumbersCount);
		}

		/// <summary>
		/// Verify <see cref="UserTspAccount"/> properties.
		/// </summary>
		private static void VerifyUserTspAccount(UserTspAccount userTspAccount, int dialInNumbersCount)
		{
			userTspAccount.ShouldNotBeNull();
			userTspAccount.Email.ShouldBe(UserEmail);
			userTspAccount.Id.ShouldBe("DYbbbbf7dPkkg8w");

			VerifyTspAccount(userTspAccount.Account, dialInNumbersCount: dialInNumbersCount);
		}

		/// <summary>
		/// Verify <see cref="TspAccount"/> properties.
		/// </summary>
		private static void VerifyTspAccount(
			TspAccount account,
			string conferenceCode = "123456789",
			string leaderPin = "123456",
			string tspBridge = "US_TSP_TB",
			int dialInNumbersCount = 3)
		{
			account.ShouldNotBeNull();
			account.Id.ShouldBe("2");
			account.ConferenceCode.ShouldBe(conferenceCode);
			account.LeaderPin.ShouldBe(leaderPin);
			account.TspBridge.ShouldBe(tspBridge);

			account.DialInNumbers.ShouldNotBeNull();
			account.DialInNumbers.Length.ShouldBe(dialInNumbersCount);

			if (dialInNumbersCount == 3)
			{
				VerifyDialInNumber(account.DialInNumbers[0], "1", "US", "123456789", PhoneNumberType.Toll);
				VerifyDialInNumber(account.DialInNumbers[1], "91", "IN", "980000021", PhoneNumberType.TollFree);
				VerifyDialInNumber(account.DialInNumbers[2], "1", "US", "2222456532", PhoneNumberType.MediaLink);
			}

			if (dialInNumbersCount == 1)
			{
				VerifyDialInNumber(account.DialInNumbers[0], "1", "US", "+1 1000200200", PhoneNumberType.TollFree);
			}
		}

		/// <summary>
		/// Verify <see cref="DialInNumber"/> properties.
		/// </summary>
		private static void VerifyDialInNumber(
			DialInNumber dialInNumber,
			string code,
			string countryLabel,
			string number,
			PhoneNumberType type)
		{
			dialInNumber.ShouldNotBeNull();
			dialInNumber.CountryCode.ShouldBe(code);
			dialInNumber.CountryLabel.ShouldBe(countryLabel);
			dialInNumber.Number.ShouldBe(number);
			dialInNumber.Type.ShouldBe(type);
		}

		#endregion
	}
}
