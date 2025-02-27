using Microsoft.Extensions.Logging;
using System;
using System.Net;
using ZoomNet.IntegrationTests.Tests;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class ApiTestSuite : TestSuite
	{
		private static readonly Type[] _tests = new Type[]
		{
			typeof(Accounts),
			typeof(CallLogs),
			typeof(Chat),
			typeof(CloudRecordings),
			typeof(Contacts),
			typeof(Dashboards),
			typeof(ExternalContacts),
			typeof(Meetings),
			typeof(Reports),
			typeof(Roles),
			typeof(Users),
			typeof(Webinars),
		};

		public ApiTestSuite(IConnectionInfo connectionInfo, IWebProxy proxy, ILoggerFactory loggerFactory) :
			base(connectionInfo, proxy, loggerFactory, _tests, true)
		{
		}
	}
}
