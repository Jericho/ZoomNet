using HttpMultipartParser;
using Pathoschild.Http.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
using static ZoomNet.Utilities.DiagnosticHandler;

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

		private static readonly DateTime EPOCH = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
#if NET5_0_OR_GREATER
				var contentStream = await httpContent.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
#else
				var contentStream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
#endif
				encoding ??= httpContent.GetEncoding(Encoding.UTF8);

				// This is important: we must make a copy of the response stream otherwise we would get an
				// exception on subsequent attempts to read the content of the stream
				using (var ms = Utils.MemoryStreamManager.GetStream())
				{
					const int DefaultBufferSize = 81920;
					await contentStream.CopyToAsync(ms, DefaultBufferSize, cancellationToken).ConfigureAwait(false);
					ms.Position = 0;
					using (var sr = new StreamReader(ms, encoding))
					{
#if NET7_0_OR_GREATER
						content = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
#else
						content = await sr.ReadToEndAsync().ConfigureAwait(false);
#endif
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

		/// <summary>
		/// Returns the value of a parameter or the default value if it doesn't exist.
		/// </summary>
		/// <param name="parser">The parser.</param>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value of the parameter.</returns>
		internal static string GetParameterValue(this MultipartFormDataParser parser, string name, string defaultValue)
		{
			if (parser.HasParameter(name)) return parser.GetParameterValue(name);
			else return defaultValue;
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

		/// <summary>
		/// Replace the current error handler which treats HTTP200 as success with a handler that treats HTTP200 as failure.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="customExceptionMessage">An optional custom error message.</param>
		/// <returns>Returns the request builder for chaining.</returns>
		internal static IRequest WithHttp200TreatedAsFailure(this IRequest request, string customExceptionMessage = null)
		{
			return request
				.WithoutFilter<ZoomErrorHandler>()
				.WithFilter(new ZoomErrorHandler(true, customExceptionMessage));
		}

		/// <summary>Set the body content of the HTTP request.</summary>
		/// <typeparam name="T">The type of object to serialize into a JSON string.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="body">The value to serialize into the HTTP body content.</param>
		/// <param name="omitCharSet">Indicates if the charset should be omitted from the 'Content-Type' request header.</param>
		/// <returns>Returns the request builder for chaining.</returns>
		/// <remarks>
		/// This method is equivalent to IRequest.AsBody&lt;T&gt;(T body) because omitting the media type
		/// causes the first formatter in MediaTypeFormatterCollection to be used by default and the first
		/// formatter happens to be the JSON formatter. However, I don't feel good about relying on the
		/// default ordering of the items in the MediaTypeFormatterCollection.
		/// </remarks>
		internal static IRequest WithJsonBody<T>(this IRequest request, T body, bool omitCharSet = false)
		{
			return request.WithBody(bodyBuilder =>
			{
				var httpContent = bodyBuilder.Model(body, new MediaTypeHeaderValue("application/json"));

				if (omitCharSet && !string.IsNullOrEmpty(httpContent.Headers.ContentType.CharSet))
				{
					httpContent.Headers.ContentType.CharSet = string.Empty;
				}

				return httpContent;
			});
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
			static void AppendFormatIfNecessary(StringBuilder stringBuilder, string timePart, int value)
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
			return GetPropertyValue<T>(element, new[] { name }, defaultValue, false);
		}

		internal static T GetPropertyValue<T>(this JsonElement element, string[] names, T defaultValue)
		{
			return GetPropertyValue<T>(element, names, defaultValue, false);
		}

		internal static T GetPropertyValue<T>(this JsonElement element, string name)
		{
			return GetPropertyValue<T>(element, new[] { name }, default, true);
		}

		internal static T GetPropertyValue<T>(this JsonElement element, string[] names)
		{
			return GetPropertyValue<T>(element, names, default, true);
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

		internal static DiagnosticInfo GetDiagnosticInfo(this IResponse response)
		{
			var diagnosticId = response.Message.RequestMessage.Headers.GetValue(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			DiagnosticHandler.DiagnosticsInfo.TryGetValue(diagnosticId, out DiagnosticInfo diagnosticInfo);
			return diagnosticInfo;
		}

		internal static async Task<(bool IsError, string ErrorMessage, int? ErrorCode)> GetErrorMessageAsync(this HttpResponseMessage message)
		{
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

				Sometimes, the JSON string contains additional info like this example:
				{
					"code":300,
					"message":"Validation Failed.",
					"errors":[
						{
							"field":"settings.jbh_time",
							"message":"Invalid parameter: jbh_time."
						}
					]
				}
			*/

			var responseContent = await message.Content.ReadAsStringAsync(null).ConfigureAwait(false);

			if (!string.IsNullOrEmpty(responseContent))
			{
				try
				{
					var rootJsonElement = JsonDocument.Parse(responseContent).RootElement;

					if (rootJsonElement.ValueKind == JsonValueKind.Object)
					{
						errorCode = rootJsonElement.TryGetProperty("code", out JsonElement jsonErrorCode) ? (int?)jsonErrorCode.GetInt32() : (int?)null;
						errorMessage = rootJsonElement.TryGetProperty("message", out JsonElement jsonErrorMessage) ? jsonErrorMessage.GetString() : (errorCode.HasValue ? $"Error code: {errorCode}" : errorMessage);
						if (rootJsonElement.TryGetProperty("errors", out JsonElement jsonErrorDetails))
						{
							var errorDetails = string.Join(
								" ",
								jsonErrorDetails
									.EnumerateArray()
									.Select(jsonErrorDetail =>
						{
							var errorDetail = jsonErrorDetail.TryGetProperty("message", out JsonElement jsonErrorMessage) ? jsonErrorMessage.GetString() : string.Empty;
							return errorDetail;
						})
									.Where(message => !string.IsNullOrEmpty(message)));

							if (!string.IsNullOrEmpty(errorDetails)) errorMessage += $" {errorDetails}";
						}

						return (errorCode.HasValue, errorMessage, errorCode);
					}
				}
				catch
				{
					// Intentionally ignore parsing errors
				}
			}

			return (!message.IsSuccessStatusCode, errorMessage, errorCode);
		}

		internal static async Task<Stream> CompressAsync(this Stream source)
		{
			var compressedStream = new MemoryStream();
			using (var gzip = new GZipStream(compressedStream, CompressionMode.Compress, true))
			{
				await source.CopyToAsync(gzip).ConfigureAwait(false);
			}

			compressedStream.Position = 0;
			return compressedStream;
		}

		internal static async Task<Stream> DecompressAsync(this Stream source)
		{
			var decompressedStream = new MemoryStream();
			using (var gzip = new GZipStream(source, CompressionMode.Decompress, true))
			{
				await gzip.CopyToAsync(decompressedStream).ConfigureAwait(false);
			}

			decompressedStream.Position = 0;
			return decompressedStream;
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
			return JsonSerializer.Deserialize<T>(element.GetRawText(), options ?? JsonFormatter.DeserializerOptions);
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

		internal static string ToHexString(this byte[] bytes)
		{
			var result = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i++)
				result.Append(bytes[i].ToString("x2"));
			return result.ToString();
		}

		internal static string ToExactLength(this string source, int totalWidth, string postfix = "...", char paddingChar = ' ')
		{
			if (string.IsNullOrEmpty(source)) return new string(paddingChar, totalWidth);
			if (source.Length <= totalWidth) return source.PadRight(totalWidth, paddingChar);
			var result = $"{source.Substring(0, totalWidth - (postfix?.Length ?? 0))}{postfix ?? string.Empty}";
			return result;
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
				return JsonSerializer.Deserialize<T>(responseContent, options ?? JsonFormatter.DeserializerOptions);
			}

			var jsonDoc = JsonDocument.Parse(responseContent, (JsonDocumentOptions)default);
			if (jsonDoc.RootElement.TryGetProperty(propertyName, out JsonElement property))
			{
				return property.ToObject<T>(options);
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
			if (!jsonProperty.HasValue && totalRecords is > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponse<T>()
			{
				PageCount = pageCount,
				PageNumber = pageNumber,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? jsonProperty.Value.ToObject<T[]>(options) : Array.Empty<T>()
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
			if (!jsonProperty.HasValue && totalRecords is > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithToken<T>()
			{
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? jsonProperty.Value.ToObject<T[]>(options) : Array.Empty<T>()
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
			if (!jsonProperty.HasValue && totalRecords is > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithTokenAndDateRange<T>()
			{
				From = from,
				To = to,
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jsonProperty.HasValue ? jsonProperty.Value.ToObject<T[]>(options) : Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		private static T GetPropertyValue<T>(this JsonElement element, string[] names, T defaultValue, bool throwIfMissing)
		{
			JsonElement? property = null;

			foreach (var name in names)
			{
				property = element.GetProperty(name, false);
				if (property.HasValue) break;
			}

			if (!property.HasValue) return defaultValue;

			var typeOfT = typeof(T);

			if (typeOfT.IsEnum)
			{
				return property.Value.ValueKind switch
				{
					JsonValueKind.String => (T)Enum.Parse(typeof(T), property.Value.GetString()),
					JsonValueKind.Number => (T)Enum.ToObject(typeof(T), property.Value.GetInt16()),
					_ => throw new ArgumentException($"Unable to convert a {property.Value.ValueKind} into a {typeof(T).FullName}", nameof(T)),
				};
			}

			if (typeOfT.IsGenericType && typeOfT.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				var underlyingType = Nullable.GetUnderlyingType(typeOfT);
				var getElementValue = typeof(Internal)
					.GetMethod(nameof(Internal.GetElementValue), BindingFlags.Static | BindingFlags.NonPublic)
					.MakeGenericMethod(underlyingType);

				return (T)getElementValue.Invoke(null, new object[] { property.Value });
			}

			if (typeOfT.IsArray)
			{
				var elementType = typeOfT.GetElementType();
				var getElementValue = typeof(Internal)
					.GetMethod(nameof(Internal.GetElementValue), BindingFlags.Static | BindingFlags.NonPublic)
					.MakeGenericMethod(elementType);

				var arrayList = new ArrayList(property.Value.GetArrayLength());
				foreach (var arrayElement in property.Value.EnumerateArray())
				{
					var elementValue = getElementValue.Invoke(null, new object[] { arrayElement });
					arrayList.Add(elementValue);
				}

				return (T)Convert.ChangeType(arrayList.ToArray(elementType), typeof(T));
			}

			return property.Value.GetElementValue<T>();
		}

		private static T GetElementValue<T>(this JsonElement element)
		{
			var typeOfT = typeof(T);

			return typeOfT switch
			{
				Type boolType when boolType == typeof(bool) => (T)(object)element.GetBoolean(),
				Type strType when strType == typeof(string) => (T)(object)element.GetString(),
				Type bytesType when bytesType == typeof(byte[]) => (T)(object)element.GetBytesFromBase64(),
				Type sbyteType when sbyteType == typeof(sbyte) => (T)(object)element.GetSByte(),
				Type byteType when byteType == typeof(byte) => (T)(object)element.GetByte(),
				Type shortType when shortType == typeof(short) => (T)(object)element.GetInt16(),
				Type ushortType when ushortType == typeof(ushort) => (T)(object)element.GetUInt16(),
				Type intType when intType == typeof(int) => (T)(object)element.GetInt32(),
				Type uintType when uintType == typeof(uint) => (T)(object)element.GetUInt32(),
				Type longType when longType == typeof(long) => (T)(object)element.GetInt64(),
				Type ulongType when ulongType == typeof(ulong) => (T)(object)element.GetUInt64(),
				Type doubleType when doubleType == typeof(double) => (T)(object)element.GetDouble(),
				Type floatType when floatType == typeof(float) => (T)(object)element.GetSingle(),
				Type decimalType when decimalType == typeof(decimal) => (T)(object)element.GetDecimal(),
				Type datetimeType when datetimeType == typeof(DateTime) => (T)(object)element.GetDateTime(),
				Type offsetType when offsetType == typeof(DateTimeOffset) => (T)(object)element.GetDateTimeOffset(),
				Type guidType when guidType == typeof(Guid) => (T)(object)element.GetGuid(),
				_ => throw new ArgumentException($"Unsable to map {typeof(T).FullName} to a corresponding JSON type", nameof(T)),
			};
		}
	}
}
