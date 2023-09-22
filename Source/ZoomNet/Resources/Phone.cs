using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc cref="IPhone" select="remarks"/>
	public class Phone : IPhone
	{
		#region private fields

		private readonly IClient _client;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Phone(IClient client)
		{
			_client = client;
		}

		#endregion

		#region Recordings endpoints

		/// <inheritdoc cref="IPhone.GetRecordingAsync" select="remarks"/>
		public Task<PhoneCallRecording> GetRecordingAsync(
			string callId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_logs/{callId}/recordings")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecording>();
		}

		/// <inheritdoc cref="IPhone.GetRecordingTranscriptAsync" select="remarks"/>
		public Task<PhoneCallRecordingTranscript> GetRecordingTranscriptAsync(
			string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/recording_transcript/download/{recordingId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecordingTranscript>();
		}

		#endregion
	}
}
