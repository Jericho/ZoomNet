using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.UnitTests
{
	internal class MockFluentHttpResponse : IResponse
	{
		public MockFluentHttpResponse(HttpResponseMessage message, MediaTypeFormatterCollection formatters = null, CancellationToken cancellationToken = default)
		{
			Message = message;
			Formatters = formatters;
			CancellationToken = cancellationToken;
		}

		public bool IsSuccessStatusCode => Message.IsSuccessStatusCode;

		public HttpStatusCode Status => Message.StatusCode;

		public HttpResponseMessage Message { get; private set; }

		public MediaTypeFormatterCollection Formatters { get; private set; }

		public CancellationToken CancellationToken { get; private set; }

		public IResponse WithCancellationToken(CancellationToken cancellationToken)
		{
			CancellationToken = cancellationToken;
			return this;
		}

		public Task<T> As<T>()
		{
			return Message.Content.ReadAsAsync<T>(Formatters, CancellationToken);
		}

		public Task<T[]> AsArray<T>()
		{
			return As<T[]>();
		}

		public Task<byte[]> AsByteArray()
		{
			return this.AssertContent().ReadAsByteArrayAsync(
#if NET5_0_OR_GREATER
                this.CancellationToken
#endif
			);
		}

		public Task<string> AsString()
		{
			return this.AssertContent().ReadAsStringAsync(
#if NET5_0_OR_GREATER
                this.CancellationToken
#endif
			);
		}

		public async Task<Stream> AsStream()
		{
			Stream stream = await this.AssertContent()
				.ReadAsStreamAsync(
#if NET5_0_OR_GREATER
                this.CancellationToken
#endif
				)
				.ConfigureAwait(false);
			stream.Position = 0;
			return stream;
		}

		public async Task<JToken> AsRawJson()
		{
			string content = await this.AsString().ConfigureAwait(false);
			return JToken.Parse(content);
		}

		public async Task<JObject> AsRawJsonObject()
		{
			string content = await this.AsString().ConfigureAwait(false);
			return JObject.Parse(content);
		}

		public async Task<JArray> AsRawJsonArray()
		{
			string content = await this.AsString().ConfigureAwait(false);
			return JArray.Parse(content);
		}

		private HttpContent AssertContent()
		{
			return this.Message?.Content ?? throw new NullReferenceException("The response has no body to read.");
		}
	}
}
