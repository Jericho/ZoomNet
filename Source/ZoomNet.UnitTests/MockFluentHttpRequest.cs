using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;

namespace ZoomNet.UnitTests
{
	internal class MockFluentHttpRequest : IRequest
	{
		public bool HeaderAdded { get; private set; }
		public string HeaderName { get; private set; }
		public string HeaderValue { get; private set; }
		public HttpRequestMessage Message { get; }
		public RequestOptions Options { get; }

		public MockFluentHttpRequest()
		{
			Message = new HttpRequestMessage(HttpMethod.Get, "https://api.zoom.us/v2/test");
			Options = new RequestOptions();
		}

		public IRequest WithHeader(string key, string value)
		{
			HeaderAdded = true;
			HeaderName = key;
			HeaderValue = value;
			return this;
		}

		// Implement other IRequest members as no-ops
		public IRequest WithBody(Func<IBodyBuilder, HttpContent> build) => this;
		public IRequest WithArgument(string key, object value) => this;
		public IRequest WithArguments<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> arguments) => this;
		public IRequest WithArguments(object arguments) => this;
		public IRequest WithCustom(Action<HttpRequestMessage> request) => this;
		public IRequest WithCancellationToken(CancellationToken cancellationToken) => this;
		public IRequest WithAuthentication(string scheme, string parameter) => this;
		public IRequest WithOptions(RequestOptions options) => this;
		public IRequest WithRequestCoordinator(Pathoschild.Http.Client.Retry.IRequestCoordinator requestCoordinator) => this;
		public IRequest WithFilter(IHttpFilter filter) => this;
		public IRequest WithoutFilter(IHttpFilter filter) => this;
		IRequest IRequest.WithoutFilter<TFilter>() => this;
		public System.Runtime.CompilerServices.TaskAwaiter<IResponse> GetAwaiter() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<IResponse> AsResponse() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<HttpResponseMessage> AsMessage() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<T> As<T>() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<T[]> AsArray<T>() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<byte[]> AsByteArray() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<string> AsString() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<System.IO.Stream> AsStream() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JToken> AsRawJson() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JObject> AsRawJsonObject() => throw new NotImplementedException();
		public System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JArray> AsRawJsonArray() => throw new NotImplementedException();
		public CancellationToken CancellationToken => CancellationToken.None;
		public MediaTypeFormatterCollection Formatters => null;
		public ICollection<IHttpFilter> Filters => new List<IHttpFilter>();
	}
}
