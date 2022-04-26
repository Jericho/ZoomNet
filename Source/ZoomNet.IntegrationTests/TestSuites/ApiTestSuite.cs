using Microsoft.Extensions.Logging;
using System;
using ZoomNet.IntegrationTests.Tests;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class ApiTestSuite : TestSuite
	{
		private static readonly Type[] _tests = new Type[]
		{
			typeof(Accounts),
			typeof(Billing),
			typeof(CallHistory),
			typeof(CallLogs),
			typeof(Chat),
			typeof(CloudRecordings),
			typeof(Contacts),
			typeof(Dashboards),
			typeof(Events),
			typeof(ExternalContacts),
			typeof(Groups),
			typeof(Meetings),
			typeof(Reports),
			typeof(Roles),
			typeof(Rooms),
			typeof(TrackingFields),
			typeof(Users),
			typeof(Webinars),
			typeof(Workspaces),
		};

		public ApiTestSuite(IZoomClient client, ILoggerFactory loggerFactory) :
			base(client, loggerFactory, _tests, true)
		{
		}
	}
}
