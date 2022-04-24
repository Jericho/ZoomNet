using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Json;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// Internal extension methods.
	/// </summary>
	internal static class Internal
	{
		internal enum UnixTimePrecision
		{
			Seconds = 0,
			Milliseconds = 1
		}

		private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts a 'unix time', which is expressed as the number of seconds (or milliseconds) since
		/// midnight on January 1st 1970, to a .Net <see cref="DateTime" />.
		/// </summary>
		/// <param name="unixTime">The unix time.</param>
		/// <param name="precision">The precision of the provided unix time.</param>
		/// <returns>
		/// The <see cref="DateTime" />.
		/// </returns>
		internal static DateTime FromUnixTime(this long unixTime, UnixTimePrecision precision = UnixTimePrecision.Seconds)
		{
			if (precision == UnixTimePrecision.Seconds) return EPOCH.AddSeconds(unixTime);
			if (precision == UnixTimePrecision.Milliseconds) return EPOCH.AddMilliseconds(unixTime);
			throw new Exception($"Unknown precision: {precision}");
		}

		/// <summary>
		/// Converts a .Net <see cref="DateTime" /> into a 'Unix time', which is expressed as the number
		/// of seconds (or milliseconds) since midnight on January 1st 1970.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="precision">The desired precision.</param>
		/// <returns>
		/// The numer of seconds/milliseconds since midnight on January 1st 1970.
		/// </returns>
		internal static long ToUnixTime(this DateTime date, UnixTimePrecision precision = UnixTimePrecision.Seconds)
		{
			var diff = date.ToUniversalTime() - EPOCH;
			if (precision == UnixTimePrecision.Seconds) return Convert.ToInt64(diff.TotalSeconds);
			if (precision == UnixTimePrecision.Milliseconds) return Convert.ToInt64(diff.TotalMilliseconds);
			throw new Exception($"Unknown precision: {precision}");
		}

		/// <summary>
		/// Converts a .Net <see cref="DateTime" /> into a string that can be accepted by the Zoom API.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="timeZone">The time zone.</param>
		/// <param name="dateOnly">Indicates if you want only the date to be converted to a string (ignoring the time).</param>
		/// <returns>
		/// The string representation of the date expressed in the Zoom format.
		/// </returns>
		internal static string ToZoomFormat(this DateTime? date, TimeZones? timeZone = null, bool dateOnly = false)
		{
			if (!date.HasValue) return null;
			return date.Value.ToZoomFormat(timeZone, dateOnly);
		}

		/// <summary>
		/// Converts a .Net <see cref="DateTime" /> into a string that can be accepted by the Zoom API.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="timeZone">The time zone.</param>
		/// <param name="dateOnly">Indicates if you want only the date to be converted to a string (ignoring the time).</param>
		/// <returns>
		/// The string representation of the date expressed in the Zoom format.
		/// </returns>
		internal static string ToZoomFormat(this DateTime date, TimeZones? timeZone = null, bool dateOnly = false)
		{
			const string dateOnlyFormat = "yyyy-MM-dd";
			const string defaultDateFormat = dateOnlyFormat + "'T'HH:mm:ss";
			const string utcDateFormat = defaultDateFormat + "'Z'";

			if (dateOnly)
			{
				if (timeZone.HasValue && timeZone.Value == TimeZones.UTC) return date.ToUniversalTime().ToString(dateOnlyFormat);
				else return date.ToString(dateOnlyFormat);
			}
			else
			{
				if (timeZone.HasValue && timeZone.Value == TimeZones.UTC) return date.ToUniversalTime().ToString(utcDateFormat);
				else return date.ToString(defaultDateFormat);
			}
		}

		/// <summary>
		/// Reads the content of the HTTP response as string asynchronously.
		/// </summary>
		/// <param name="httpContent">The content.</param>
		/// <param name="encoding">The encoding. You can leave this parameter null and the encoding will be
		/// automatically calculated based on the charset in the response. Also, UTF-8
		/// encoding will be used if the charset is absent from the response, is blank
		/// or contains an invalid value.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The string content of the response.</returns>
		/// <remarks>
		/// This method is an improvement over the built-in ReadAsStringAsync method
		/// because it can handle invalid charset returned in the response. For example
		/// you may be sending a request to an API that returns a blank charset or a
		/// misspelled one like 'utf8' instead of the correctly spelled 'utf-8'. The
		/// built-in method throws an exception if an invalid charset is specified
		/// while this method uses the UTF-8 encoding in that situation.
		///
		/// My motivation for writing this extension method was to work around a situation
		/// where the 3rd party API I was sending requests to would sometimes return 'utf8'
		/// as the charset and an exception would be thrown when I called the ReadAsStringAsync
		/// method to get the content of the response into a string because the .Net HttpClient
		/// would attempt to determine the proper encoding to use but it would fail due to
		/// the fact that the charset was misspelled. I contacted the vendor, asking them
		/// to either omit the charset or fix the misspelling but they didn't feel the need
		/// to fix this issue because:
		/// "in some programming languages, you can use the syntax utf8 instead of utf-8".
		/// In other words, they are happy to continue using the misspelled value which is
		/// supported by "some" programming languages instead of using the properly spelled
		/// value which is supported by all programming languages.
		/// </remarks>
		/// <example>
		/// <code>
		/// var httpRequest = new HttpRequestMessage
		/// {
		///     Method = HttpMethod.Get,
		///     RequestUri = new Uri("https://api.vendor.com/v1/endpoint")
		/// };
		/// var httpClient = new HttpClient();
		/// var response = await httpClient.SendAsync(httpRequest, CancellationToken.None).ConfigureAwait(false);
		/// var responseContent = await response.Content.ReadAsStringAsync(null).ConfigureAwait(false);
		/// </code>
		/// </example>
		internal static async Task<string> ReadAsStringAsync(this HttpContent httpContent, Encoding encoding, CancellationToken cancellationToken = default)
		{
			var content = string.Empty;

			if (httpContent != null)
			{
				var contentStream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);

				if (encoding == null) encoding = httpContent.GetEncoding(Encoding.UTF8);

				// This is important: we must make a copy of the response stream otherwise we would get an
				// exception on subsequent attempts to read the content of the stream
				using (var ms = Utils.MemoryStreamManager.GetStream())
				{
					const int DefaultBufferSize = 81920;
					await contentStream.CopyToAsync(ms, DefaultBufferSize, cancellationToken).ConfigureAwait(false);
					ms.Position = 0;
					using (var sr = new StreamReader(ms, encoding))
					{
						content = await sr.ReadToEndAsync().ConfigureAwait(false);
					}

					// It's important to rewind the stream
					if (contentStream.CanSeek) contentStream.Position = 0;
				}
			}

			return content;
		}

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="defaultEncoding">The default encoding.</param>
		/// <returns>
		/// The encoding.
		/// </returns>
		/// <remarks>
		/// This method tries to get the encoding based on the charset or uses the
		/// 'defaultEncoding' if the charset is empty or contains an invalid value.
		/// </remarks>
		/// <example>
		///   <code>
		/// var httpRequest = new HttpRequestMessage
		/// {
		/// Method = HttpMethod.Get,
		/// RequestUri = new Uri("https://my.api.com/v1/myendpoint")
		/// };
		/// var httpClient = new HttpClient();
		/// var response = await httpClient.SendAsync(httpRequest, CancellationToken.None).ConfigureAwait(false);
		/// var encoding = response.Content.GetEncoding(Encoding.UTF8);
		/// </code>
		/// </example>
		internal static Encoding GetEncoding(this HttpContent content, Encoding defaultEncoding)
		{
			var encoding = defaultEncoding;
			try
			{
				var charset = content?.Headers?.ContentType?.CharSet;
				if (!string.IsNullOrEmpty(charset))
				{
					encoding = Encoding.GetEncoding(charset);
				}
			}
			catch
			{
				encoding = defaultEncoding;
			}

			return encoding;
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to an object of the desired type.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<T> AsObject<T>(this IResponse response, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonSerializerOptions options = null)
		{
			return response.Message.Content.AsObject<T>(propertyName, throwIfPropertyIsMissing, options);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to an object of the desired type.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<T> AsObject<T>(this IRequest request, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonSerializerOptions options = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsObject<T>(propertyName, throwIfPropertyIsMissing, options).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this IResponse response, string propertyName = null, JsonSerializerOptions options = null)
		{
			return response.Message.Content.AsPaginatedResponse<T>(propertyName, options);
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this IRequest request, string propertyName = null, JsonSerializerOptions options = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponse<T>(propertyName, options).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this IResponse response, string propertyName, JsonSerializerOptions options = null)
		{
			return response.Message.Content.AsPaginatedResponseWithToken<T>(propertyName, options);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this IRequest request, string propertyName, JsonSerializerOptions options = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponseWithToken<T>(propertyName, options).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this IResponse response, string propertyName, JsonSerializerOptions options = null)
		{
			return response.Message.Content.AsPaginatedResponseWithTokenAndDateRange<T>(propertyName, options);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this IRequest request, string propertyName, JsonSerializerOptions options = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponseWithTokenAndDateRange<T>(propertyName, options).ConfigureAwait(false);
		}

		/// <summary>Get a raw JSON document representation of the response.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<JsonDocument> AsRawJsonDocument(this IResponse response, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			return response.Message.Content.AsRawJsonDocument(propertyName, throwIfPropertyIsMissing);
		}

		/// <summary>Get a raw JSON document representation of the response.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<JsonDocument> AsRawJsonDocument(this IRequest request, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsRawJsonDocument(propertyName, throwIfPropertyIsMissing).ConfigureAwait(false);
		}

		internal static IRequest WithHttp200TreatedAsFailure(this IRequest request, string customExceptionMessage = null)
		{
			var currentErrorHandler = request.Filters.OfType<ZoomErrorHandler>().SingleOrDefault();
			var newErrorHandler = new ZoomErrorHandler(true, customExceptionMessage);

			// Replace the current error handler which treats HTTP200 as success with a handler that treats HTTP200 as failure
			request.Filters.Replace(currentErrorHandler, newErrorHandler);

			return request;
		}

		/// <summary>Set the body content of the HTTP request.</summary>
		/// <typeparam name="T">The type of object to serialize into a JSON string.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="body">The value to serialize into the HTTP body content.</param>
		/// <returns>Returns the request builder for chaining.</returns>
		/// <remarks>
		/// This method is equivalent to IRequest.AsBody&lt;T&gt;(T body) because omitting the media type
		/// causes the first formatter in MediaTypeFormatterCollection to be used by default and the first
		/// formatter happens to be the JSON formatter. However, I don't feel good about relying on the
		/// default ordering of the items in the MediaTypeFormatterCollection.
		/// </remarks>
		internal static IRequest WithJsonBody<T>(this IRequest request, T body)
		{
			return request.WithBody(bodyBuilder => bodyBuilder.Model(body, new MediaTypeHeaderValue("application/json")));
		}

		/// <summary>Asynchronously retrieve the response body as a <see cref="string"/>.</summary>
		/// <param name="response">The response.</param>
		/// <param name="encoding">The encoding. You can leave this parameter null and the encoding will be
		/// automatically calculated based on the charset in the response. Also, UTF-8
		/// encoding will be used if the charset is absent from the response, is blank
		/// or contains an invalid value.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<string> AsString(this IResponse response, Encoding encoding)
		{
			return response.Message.Content.ReadAsStringAsync(encoding);
		}

		/// <summary>Asynchronously retrieve the response body as a <see cref="string"/>.</summary>
		/// <param name="request">The request.</param>
		/// <param name="encoding">The encoding. You can leave this parameter null and the encoding will be
		/// automatically calculated based on the charset in the response. Also, UTF-8
		/// encoding will be used if the charset is absent from the response, is blank
		/// or contains an invalid value.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<string> AsString(this IRequest request, Encoding encoding)
		{
			IResponse response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsString(encoding).ConfigureAwait(false);
		}

		/// <summary>
		///  Converts the value of the current System.TimeSpan object to its equivalent string
		///  representation by using a human readable format.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns>Returns the human readable representation of the TimeSpan.</returns>
		internal static string ToDurationString(this TimeSpan timeSpan)
		{
			void AppendFormatIfNecessary(StringBuilder stringBuilder, string timePart, int value)
			{
				if (value <= 0) return;
				stringBuilder.AppendFormat($" {value} {timePart}{(value > 1 ? "s" : string.Empty)}");
			}

			// In case the TimeSpan is extremely short
			if (timeSpan.TotalMilliseconds <= 1) return "1 millisecond";

			var result = new StringBuilder();
			AppendFormatIfNecessary(result, "day", timeSpan.Days);
			AppendFormatIfNecessary(result, "hour", timeSpan.Hours);
			AppendFormatIfNecessary(result, "minute", timeSpan.Minutes);
			AppendFormatIfNecessary(result, "second", timeSpan.Seconds);
			AppendFormatIfNecessary(result, "millisecond", timeSpan.Milliseconds);
			return result.ToString().Trim();
		}

		/// <summary>
		/// Ensure that a string starts with a given prefix.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="prefix">The prefix.</param>
		/// <returns>The value including the prefix.</returns>
		internal static string EnsureStartsWith(this string value, string prefix)
		{
			return !string.IsNullOrEmpty(value) && value.StartsWith(prefix) ? value : string.Concat(prefix, value);
		}

		/// <summary>
		/// Ensure that a string ends with a given suffix.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="suffix">The sufix.</param>
		/// <returns>The value including the suffix.</returns>
		internal static string EnsureEndsWith(this string value, string suffix)
		{
			return !string.IsNullOrEmpty(value) && value.EndsWith(suffix) ? value : string.Concat(value, suffix);
		}

		internal static JsonElement? GetProperty(this JsonElement element, string name, bool throwIfMissing = true)
		{
			var parts = name.Split('/');
			if (!element.TryGetProperty(parts[0], out var property))
			{
				if (throwIfMissing) throw new ArgumentException($"Unable to find '{name}'", nameof(name));
				else return null;
			}

			foreach (var part in parts.Skip(1))
			{
				if (!property.TryGetProperty(part, out property))
				{
					if (throwIfMissing) throw new ArgumentException($"Unable to find '{name}'", nameof(name));
					else return null;
				}
			}

			return property;
		}

		internal static T GetPropertyValue<T>(this JsonElement element, string name, T defaultValue)
		{
			return GetPropertyValue<T>(element, name, default, false);
		}

		internal static T GetPropertyValue<T>(this JsonElement element, string name)
		{
			return GetPropertyValue<T>(element, name, default, true);
		}

		internal static async Task<TResult[]> ForEachAsync<T, TResult>(this IEnumerable<T> items, Func<T, Task<TResult>> action, int maxDegreeOfParalellism)
		{
			var allTasks = new List<Task<TResult>>();
			using (var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParalellism))
			{
				foreach (var item in items)
				{
					await throttler.WaitAsync();
					allTasks.Add(
						Task.Run(async () =>
						{
							try
							{
								return await action(item).ConfigureAwait(false);
							}
							finally
							{
								throttler.Release();
							}
						}));
				}

				var results = await Task.WhenAll(allTasks).ConfigureAwait(false);
				return results;
			}
		}

		internal static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> action, int maxDegreeOfParalellism)
		{
			var allTasks = new List<Task>();
			using (var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParalellism))
			{
				foreach (var item in items)
				{
					await throttler.WaitAsync();
					allTasks.Add(
						Task.Run(async () =>
						{
							try
							{
								await action(item).ConfigureAwait(false);
							}
							finally
							{
								throttler.Release();
							}
						}));
				}

				await Task.WhenAll(allTasks).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Gets the attribute of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of the desired attribute.</typeparam>
		/// <param name="enumVal">The enum value.</param>
		/// <returns>The attribute.</returns>
		internal static T GetAttributeOfType<T>(this Enum enumVal)
			where T : Attribute
		{
			return enumVal.GetType()
				.GetTypeInfo()
				.DeclaredMembers
				.SingleOrDefault(x => x.Name == enumVal.ToString())
				?.GetCustomAttribute<T>(false);
		}

		/// <summary>
		/// Indicates if an object contain a numerical value.
		/// </summary>
		/// <param name="value">The object.</param>
		/// <returns>A boolean indicating if the object contains a numerical value.</returns>
		internal static bool IsNumber(this object value)
		{
			return value is sbyte
				   || value is byte
				   || value is short
				   || value is ushort
				   || value is int
				   || value is uint
				   || value is long
				   || value is ulong
				   || value is float
				   || value is double
				   || value is decimal;
		}

		/// <summary>
		/// Returns the first value for a specified header stored in the System.Net.Http.Headers.HttpHeaderscollection.
		/// </summary>
		/// <param name="headers">The HTTP headers.</param>
		/// <param name="name">The specified header to return value for.</param>
		/// <returns>A string.</returns>
		internal static string GetValue(this HttpHeaders headers, string name)
		{
			if (headers == null) return null;

			if (headers.TryGetValues(name, out IEnumerable<string> values))
			{
				return values.FirstOrDefault();
			}

			return null;
		}

		internal static IEnumerable<KeyValuePair<string, string>> ParseQuerystring(this Uri uri)
		{
			var querystringParameters = uri
				.Query.TrimStart('?')
				.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(value => value.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
				.Select(splitValue =>
				{
					if (splitValue.Length == 1)
					{
						return new KeyValuePair<string, string>(splitValue[0].Trim(), null);
					}
					else
					{
						return new KeyValuePair<string, string>(splitValue[0].Trim(), splitValue[1].Trim());
					}
				});

			return querystringParameters;
		}

		internal static (WeakReference<HttpRequestMessage> RequestReference, string Diagnostic, long RequestTimeStamp, long ResponseTimestamp) GetDiagnosticInfo(this IResponse response)
		{
			var diagnosticId = response.Message.RequestMessage.Headers.GetValue(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			DiagnosticHandler.DiagnosticsInfo.TryGetValue(diagnosticId, out (WeakReference<HttpRequestMessage> RequestReference, string Diagnostic, long RequestTimeStamp, long ResponseTimestamp) diagnosticInfo);
			return diagnosticInfo;
		}

		internal static async Task<(bool, string, int?)> GetErrorMessageAsync(this HttpResponseMessage message)
		{
			// Assume there is no error
			var isError = false;

			// Default error code
			int? errorCode = null;

			// Default error message
			var errorMessage = $"{(int)message.StatusCode}: {message.ReasonPhrase}";

			/*
				In case of an error, the Zoom API returns a JSON string that looks like this:
				{
					"code": 300,
					"message": "This meeting has not registration required: 544993922"
				}
			*/

			var responseContent = await message.Content.ReadAsStringAsync(null).ConfigureAwait(false);

			if (!string.IsNullOrEmpty(responseContent))
			{
				try
				{
					var rootJsonElement = JsonDocument.Parse(responseContent).RootElement;
					errorCode = rootJsonElement.TryGetProperty("code", out JsonElement jsonErrorCode) ? (int?)jsonErrorCode.GetInt32() : (int?)null;
					errorMessage = rootJsonElement.TryGetProperty("message", out JsonElement jsonErrorMessage) ? jsonErrorMessage.GetString() : (errorCode.HasValue ? $"Error code: {errorCode}" : errorMessage);

					isError = errorCode.HasValue;
				}
				catch
				{
					// Intentionally ignore parsing errors
				}
			}

			return (isError, errorMessage, errorCode);
		}

		internal static void Replace<T>(this ICollection<T> collection, T oldValue, T newValue)
		{
			// In case the collection is ordered, we'll be able to preserve the order
			if (collection is IList<T> collectionAsList)
			{
				var oldIndex = collectionAsList.IndexOf(oldValue);
				collectionAsList.RemoveAt(oldIndex);
				collectionAsList.Insert(oldIndex, newValue);
			}
			else
			{
				// No luck, so just remove then add
				collection.Remove(oldValue);
				collection.Add(newValue);
			}
		}

		/// <summary>Convert an enum to its string representation.</summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="enumValue">The value.</param>
		/// <returns>The string representation of the enum value.</returns>
		/// <remarks>Inspired by: https://stackoverflow.com/questions/10418651/using-enummemberattribute-and-doing-automatic-string-conversions .</remarks>
		internal static string ToEnumString<T>(this T enumValue)
			where T : Enum
		{
			if (TryToEnumString(enumValue, out string stringValue)) return stringValue;
			return enumValue.ToString();
		}

		internal static bool TryToEnumString<T>(this T enumValue, out string stringValue)
			where T : Enum
		{
			var enumMemberAttribute = enumValue.GetAttributeOfType<EnumMemberAttribute>();
			if (enumMemberAttribute != null)
			{
				stringValue = enumMemberAttribute.Value;
				return true;
			}

			var jsonPropertyNameAttribute = enumValue.GetAttributeOfType<JsonPropertyNameAttribute>();
			if (jsonPropertyNameAttribute != null)
			{
				stringValue = jsonPropertyNameAttribute.Name;
				return true;
			}

			var descriptionAttribute = enumValue.GetAttributeOfType<DescriptionAttribute>();
			if (descriptionAttribute != null)
			{
				stringValue = descriptionAttribute.Description;
				return true;
			}

			stringValue = null;
			return false;
		}

		/// <summary>Parses a string into its corresponding enum value.</summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="str">The string value.</param>
		/// <returns>The enum representation of the string value.</returns>
		/// <remarks>Inspired by: https://stackoverflow.com/questions/10418651/using-enummemberattribute-and-doing-automatic-string-conversions .</remarks>
		internal static T ToEnum<T>(this string str)
			where T : Enum
		{
			if (TryToEnum(str, out T enumValue)) return enumValue;

			throw new ArgumentException($"There is no value in the {typeof(T).Name} enum that corresponds to '{str}'.");
		}

		internal static bool TryToEnum<T>(this string str, out T enumValue)
			where T : Enum
		{
			var enumType = typeof(T);
			foreach (var name in Enum.GetNames(enumType))
			{
				var customAttributes = enumType.GetField(name).GetCustomAttributes(true);

				// See if there's a matching 'EnumMember' attribute
				if (customAttributes.OfType<EnumMemberAttribute>().Any(attribute => string.Equals(attribute.Value, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (T)Enum.Parse(enumType, name);
					return true;
				}

				// See if there's a matching 'JsonPropertyName' attribute
				if (customAttributes.OfType<JsonPropertyNameAttribute>().Any(attribute => string.Equals(attribute.Name, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (T)Enum.Parse(enumType, name);
					return true;
				}

				// See if there's a matching 'Description' attribute
				if (customAttributes.OfType<DescriptionAttribute>().Any(attribute => string.Equals(attribute.Description, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (T)Enum.Parse(enumType, name);
					return true;
				}

				// See if the value matches the name
				if (string.Equals(name, str, StringComparison.OrdinalIgnoreCase))
				{
					enumValue = (T)Enum.Parse(enumType, name);
					return true;
				}
			}

			enumValue = default;
			return false;
		}

		internal static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
		{
			return JsonSerializer.Deserialize<T>(element, options ?? ZoomNetJsonFormatter.DeserializerOptions);
		}

		internal static void Add<T>(this JsonObject jsonObject, string propertyName, T value)
		{
			if (value is IEnumerable<T> items)
			{
				var jsonArray = new JsonArray();
				foreach (var item in items)
				{
					jsonArray.Add(item);
				}

				jsonObject.Add(propertyName, jsonArray);
			}
			else
			{
				jsonObject.Add(propertyName, JsonValue.Create(value));
			}
		}

		/// <summary>Asynchronously converts the JSON encoded content and convert it to an object of the desired type.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<T> AsObject<T>(this HttpContent httpContent, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null, cancellationToken).ConfigureAwait(false);

			if (string.IsNullOrEmpty(propertyName))
			{
				return JsonSerializer.Deserialize<T>(responseContent, options ?? ZoomNetJsonFormatter.DeserializerOptions);
			}

			var jsonDoc = JsonDocument.Parse(responseContent, (JsonDocumentOptions)default);
			if (jsonDoc.RootElement.TryGetProperty(propertyName, out JsonElement property))
			{
				var propertyContent = property.GetRawText();
				return JsonSerializer.Deserialize<T>(propertyContent, options ?? ZoomNetJsonFormatter.DeserializerOptions);
			}
			else if (throwIfPropertyIsMissing)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}
			else
			{
				return default;
			}
		}

		/// <summary>Get a raw JSON object representation of the response.</summary>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<JsonDocument> AsRawJsonDocument(this HttpContent httpContent, string propertyName = null, bool throwIfPropertyIsMissing = true, CancellationToken cancellationToken = default)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null, cancellationToken).ConfigureAwait(false);

			var jsonDoc = JsonDocument.Parse(responseContent, (JsonDocumentOptions)default);

			if (string.IsNullOrEmpty(propertyName))
			{
				return jsonDoc;
			}

			if (jsonDoc.RootElement.TryGetProperty(propertyName, out JsonElement property))
			{
				var propertyContent = property.GetRawText();
				return JsonDocument.Parse(propertyContent, (JsonDocumentOptions)default);
			}
			else if (throwIfPropertyIsMissing)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}
			else
			{
				return default;
			}
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this HttpContent httpContent, string propertyName, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
		{
			// Get the content as a queryable json document
			var doc = await httpContent.AsRawJsonDocument(null, false, cancellationToken).ConfigureAwait(false);
			var rootElement = doc.RootElement;

			// Get the various metadata properties
			var pageCount = rootElement.GetPropertyValue("page_count", 0);
			var pageNumber = rootElement.GetPropertyValue("page_number", 0);
			var pageSize = rootElement.GetPropertyValue("page_size", 0);
			var totalRecords = rootElement.GetPropertyValue("total_records", (int?)null);

			// Get the property that holds the records
			var jsonProperty = rootElement.GetProperty(propertyName, false);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			if (!jsonProperty.HasValue && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponse<T>()
			{
				PageCount = pageCount,
				PageNumber = pageNumber,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? JsonSerializer.Deserialize<T[]>(jsonProperty.Value, options ?? ZoomNetJsonFormatter.DeserializerOptions) : Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this HttpContent httpContent, string propertyName, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
		{
			// Get the content as a queryable json document
			var doc = await httpContent.AsRawJsonDocument(null, false, cancellationToken).ConfigureAwait(false);
			var rootElement = doc.RootElement;

			// Get the various metadata properties
			var nextPageToken = rootElement.GetPropertyValue("next_page_token", string.Empty);
			var pageSize = rootElement.GetPropertyValue("page_size", 0);
			var totalRecords = rootElement.GetPropertyValue("total_records", (int?)null);

			// Get the property that holds the records
			var jsonProperty = rootElement.GetProperty(propertyName, false);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			if (!jsonProperty.HasValue && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithToken<T>()
			{
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? JsonSerializer.Deserialize<T[]>(jsonProperty.Value, options ?? ZoomNetJsonFormatter.DeserializerOptions) : Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="options">Options to control behavior Converter during parsing.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this HttpContent httpContent, string propertyName, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
		{
			// Get the content as a queryable json document
			var doc = await httpContent.AsRawJsonDocument(null, false, cancellationToken).ConfigureAwait(false);
			var rootElement = doc.RootElement;

			// Get the various metadata properties
			var from = DateTime.ParseExact(rootElement.GetPropertyValue("from", string.Empty), "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var to = DateTime.ParseExact(rootElement.GetPropertyValue("to", string.Empty), "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var nextPageToken = rootElement.GetPropertyValue("next_page_token", string.Empty);
			var pageSize = rootElement.GetPropertyValue("page_size", 0);
			var totalRecords = rootElement.GetPropertyValue("total_records", (int?)null);

			// Get the property that holds the records
			var jsonProperty = rootElement.GetProperty(propertyName, false);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			if (!jsonProperty.HasValue && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithTokenAndDateRange<T>()
			{
				From = from,
				To = to,
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? JsonSerializer.Deserialize<T[]>(jsonProperty.Value, options ?? ZoomNetJsonFormatter.DeserializerOptions) : Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		private static T GetPropertyValue<T>(this JsonElement element, string name, T defaultValue, bool throwIfMissing)
		{
			var property = element.GetProperty(name, throwIfMissing);
			if (!property.HasValue) return defaultValue;

			var typeOfT = typeof(T);

			if (typeOfT.IsEnum)
			{
				switch (property.Value.ValueKind)
				{
					case JsonValueKind.String: return (T)Enum.Parse(typeof(T), property.Value.GetString());
					case JsonValueKind.Number: return (T)Enum.ToObject(typeof(T), property.Value.GetInt16());
					default: throw new ArgumentException($"Unable to convert a {property.Value.ValueKind} into a {typeof(T).FullName}", nameof(T));
				}
			}

			if (typeOfT.IsGenericType && typeOfT.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				typeOfT = Nullable.GetUnderlyingType(typeOfT);
			}

			switch (typeOfT)
			{
				case Type boolType when boolType == typeof(bool): return (T)(object)property.Value.GetBoolean();
				case Type strType when strType == typeof(string): return (T)(object)property.Value.GetString();
				case Type bytesType when bytesType == typeof(byte[]): return (T)(object)property.Value.GetBytesFromBase64();
				case Type sbyteType when sbyteType == typeof(sbyte): return (T)(object)property.Value.GetSByte();
				case Type byteType when byteType == typeof(byte): return (T)(object)property.Value.GetByte();
				case Type shortType when shortType == typeof(short): return (T)(object)property.Value.GetInt16();
				case Type ushortType when ushortType == typeof(ushort): return (T)(object)property.Value.GetUInt16();
				case Type intType when intType == typeof(int): return (T)(object)property.Value.GetInt32();
				case Type uintType when uintType == typeof(uint): return (T)(object)property.Value.GetUInt32();
				case Type longType when longType == typeof(long): return (T)(object)property.Value.GetInt64();
				case Type ulongType when ulongType == typeof(ulong): return (T)(object)property.Value.GetUInt64();
				case Type doubleType when doubleType == typeof(double): return (T)(object)property.Value.GetDouble();
				case Type floatType when floatType == typeof(float): return (T)(object)property.Value.GetSingle();
				case Type decimalType when decimalType == typeof(decimal): return (T)(object)property.Value.GetDecimal();
				case Type datetimeType when datetimeType == typeof(DateTime): return (T)(object)property.Value.GetDateTime();
				case Type offsetType when offsetType == typeof(DateTimeOffset): return (T)(object)property.Value.GetDateTimeOffset();
				case Type guidType when guidType == typeof(Guid): return (T)(object)property.Value.GetGuid();
				default: throw new ArgumentException($"Unsable to map {typeof(T).FullName} to a corresponding JSON type", nameof(T));
			}
		}
	}
}
