using Microsoft.Extensions.Logging;
using System;
using System.Net;
using ZoomNet.IntegrationTests.Tests;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class ChatbotTestSuite : TestSuite
	{
		private static readonly Type[] _tests = new Type[]
		{
			typeof(Chatbot),
		};

		public ChatbotTestSuite(IConnectionInfo connectionInfo, IWebProxy proxy, ILoggerFactory loggerFactory) :
				base(connectionInfo, proxy, loggerFactory, _tests, false)
		{
		}
	}
}
