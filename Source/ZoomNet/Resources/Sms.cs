using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Sms : ISms
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sms" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Sms(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<SmsMessage>> GetSmsSessionDetailsAsync(
			string sessionId, DateTime? from, DateTime? to, bool? orderAscending = true, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 100);

			return _client
				.GetAsync($"phone/sms/sessions/{sessionId}")
				.WithArgument("sort", orderAscending.HasValue ? orderAscending.Value ? 1 : 2 : null)
				.WithArgument("from", from?.ToZoomFormat(timeZone: TimeZones.UTC, dateOnly: false))
				.WithArgument("to", to?.ToZoomFormat(timeZone: TimeZones.UTC, dateOnly: false))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<SmsMessage>("sms_histories");
		}
	}
}
