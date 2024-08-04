using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class GraphQLTestSuite : TestSuite
	{
		public GraphQLTestSuite(IConnectionInfo connectionInfo, IWebProxy proxy, ILoggerFactory loggerFactory) :
			base(connectionInfo, proxy, loggerFactory, Array.Empty<Type>(), false)
		{
		}

		public override async Task<ResultCodes> RunTestsAsync(CancellationToken cancellationToken)
		{
			var logger = base.LoggerFactory.CreateLogger<ZoomGraphQLClient>();

			// Configure cancellation (this allows you to press CTRL+C or CTRL+Break to stop the websocket client)
			var exitEvent = new ManualResetEvent(false);
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				exitEvent.Set();
			};

			// Configure ZoomNet GraphQL client
			using var graphQLClient = new ZoomGraphQLClient(base.ConnectionInfo, base.Proxy, null, logger);

			//var query = "{ user(userId:\"me\") { profile{id} } }";

			var query = @"
{
   user(userId:""me"", ){

       profile{
           id
           email
           employeeUniqueId
           firstName
           lastName
           role{
               id
               name
           }
       }
       meetings(first:100,meetingType:PREVIOUS_MEETINGS){
           edges{
               id
               topic
               startTime
           }

       }
       recordings(first:100){
           edges{
               uuid
               topic
           }
       }

   }
}";

			await graphQLClient.SendQueryAsync<Meeting>(query, null, cancellationToken).ConfigureAwait(false);

			return ResultCodes.Success;
		}
	}
}
