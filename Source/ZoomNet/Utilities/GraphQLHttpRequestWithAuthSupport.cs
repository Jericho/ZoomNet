using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ZoomNet.Utilities
{
	internal class GraphQLHttpRequestWithAuthSupport : GraphQLHttpRequest
	{
		public AuthenticationHeaderValue Authentication { get; set; }

		public override HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
		{
			var r = base.ToHttpRequestMessage(options, serializer);
			r.Headers.Authorization = Authentication;
			return r;
		}
	}
}
