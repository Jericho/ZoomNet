using Microsoft.Extensions.Logging;
using System;
using ZoomNet.IntegrationTests.Tests;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class ChatbotTestSuite : TestSuite
	{
		private static readonly Type[] _tests = new Type[]
		{
			typeof(Chatbot),
		};

		public ChatbotTestSuite(IZoomClient client, ILoggerFactory loggerFactory) :
				base(client, loggerFactory, _tests, false)
		{
		}
	}
}
