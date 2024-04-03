using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc cref="ISms" select="remarks"/>
	public class Sms : ISms
	{
		#region private fields

		private readonly IClient _client;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Sms" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Sms(IClient client)
		{
			_client = client;
		}

		#endregion

		#region SMS endpoints

		/// <inheritdoc cref="ISms.GetSmsSessionDetailsAsync" select="remarks" />
		public Task<PaginatedResponseWithToken<SmsMessage>> GetSmsSessionDetailsAsync(
			string sessionId, DateTime? from, DateTime? to, bool? orderAscending = true, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 100)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 100");
			}

			return _client
				.GetAsync($"phone/sms/sessions/{sessionId}")
				.WithArgument("sort", orderAscending.HasValue ? orderAscending.Value ? 1 : 2 : null)
				.WithArgument("from", from?.ToZoomFormat(dateOnly: false))
				.WithArgument("to", to?.ToZoomFormat(dateOnly: false))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<SmsMessage>("sms_histories");
		}

		#endregion
	}
}
