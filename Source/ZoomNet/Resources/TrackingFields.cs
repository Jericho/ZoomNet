using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class TrackingFields : ITrackingFields
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackingFields" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal TrackingFields(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<TrackingField> CreateAsync(string name, IEnumerable<string> recommendedValues = null, bool? isRequired = null, bool? isVisible = null, CancellationToken cancellationToken = default)
		{
			// Generally speaking, I prefer to defer to the API to enforce validation but in this case, I think it's reasonable to enforce a limit on the name length
			// because the API returns an error message that does not specify the cause of the error when the name is too long.
			// See: https://devforum.zoom.us/t/unable-to-update-an-existing-tracking-field/130784
			if ((name?.Length ?? 0) > 50) throw new ArgumentOutOfRangeException(nameof(name), "You are attempting to set the name of a tracking field with a value that exceeds the 50 character limit");

			var data = new JsonObject
			{
				{ "field", name },
				{ "recommended_values", recommendedValues?.ToArray() },
				{ "required", isRequired },
				{ "visible", isVisible }
			};

			return _client
				.PostAsync($"tracking_fields")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<TrackingField>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string trackingFieldId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"tracking_fields/{trackingFieldId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<TrackingField> GetAsync(string trackingFieldId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"tracking_fields/{trackingFieldId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<TrackingField>();
		}

		/// <inheritdoc/>
		public Task<TrackingField[]> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"tracking_fields")
				.WithCancellationToken(cancellationToken)
				.AsObject<TrackingField[]>("tracking_fields");
		}

		/// <inheritdoc/>
		public Task UpdateAsync(string trackingFieldId, string name = null, IEnumerable<string> recommendedValues = null, bool? isRequired = null, bool? isVisible = null, CancellationToken cancellationToken = default)
		{
			// Generally speaking, I prefer to defer to the API to enforce validation but in this case, I think it's reasonable to enforce a limit on the name length
			// because the API returns an error message that does not specify the cause of the error when the name is too long.
			// See: https://devforum.zoom.us/t/unable-to-update-an-existing-tracking-field/130784
			if ((name?.Length ?? 0) > 50) throw new ArgumentOutOfRangeException(nameof(name), "You are attempting to set the name of a tracking field with a value that exceeds the 50 character limit");

			var data = new JsonObject
			{
				{ "field", name },
				{ "recommended_values", recommendedValues?.ToArray() },
				{ "required", isRequired },
				{ "visible", isVisible }
			};

			return _client
				.PatchAsync($"tracking_fields/{trackingFieldId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
