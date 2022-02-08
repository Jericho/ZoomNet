using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Threading;
using System.Threading.Tasks;
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
		internal static async Task<string> ReadAsStringAsync(this HttpContent httpContent, Encoding encoding)
		{
			var content = string.Empty;

			if (httpContent != null)
			{
				var contentStream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);

				if (encoding == null) encoding = httpContent.GetEncoding(Encoding.UTF8);

				// This is important: we must make a copy of the response stream otherwise we would get an
				// exception on subsequent attempts to read the content of the stream
				using (var ms = new MemoryStream())
				{
					await contentStream.CopyToAsync(ms).ConfigureAwait(false);
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
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<T> AsObject<T>(this IResponse response, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonConverter jsonConverter = null)
		{
			return response.Message.Content.AsObject<T>(propertyName, throwIfPropertyIsMissing, jsonConverter);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to an object of the desired type.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<T> AsObject<T>(this IRequest request, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonConverter jsonConverter = null)
		{
			var response = await request.AsMessage().ConfigureAwait(false);
			return await response.Content.AsObject<T>(propertyName, throwIfPropertyIsMissing, jsonConverter).ConfigureAwait(false);
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<JObject> AsRawJsonObject(this IResponse response, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			return response.Message.Content.AsRawJsonObject(propertyName, throwIfPropertyIsMissing);
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<JObject> AsRawJsonObject(this IRequest request, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			var response = await request.AsMessage().ConfigureAwait(false);
			return await response.Content.AsRawJsonObject(propertyName, throwIfPropertyIsMissing).ConfigureAwait(false);
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<JArray> AsRawJsonArray(this IResponse response, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			return response.Message.Content.AsRawJsonArray(propertyName, throwIfPropertyIsMissing);
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<JArray> AsRawJsonArray(this IRequest request, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			var response = await request.AsMessage().ConfigureAwait(false);
			return await response.Content.AsRawJsonArray(propertyName, throwIfPropertyIsMissing).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this IResponse response, string propertyName, JsonConverter jsonConverter = null)
		{
			return response.Message.Content.AsPaginatedResponse<T>(propertyName, jsonConverter);
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this IRequest request, string propertyName, JsonConverter jsonConverter = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponse<T>(propertyName, jsonConverter).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this IResponse response, string propertyName, JsonConverter jsonConverter = null)
		{
			return response.Message.Content.AsPaginatedResponseWithToken<T>(propertyName, jsonConverter);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this IRequest request, string propertyName, JsonConverter jsonConverter = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponseWithToken<T>(propertyName, jsonConverter).ConfigureAwait(false);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="response">The response.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this IResponse response, string propertyName, JsonConverter jsonConverter = null)
		{
			return response.Message.Content.AsPaginatedResponseWithTokenAndDateRange<T>(propertyName, jsonConverter);
		}

		/// <summary>Asynchronously retrieve the JSON encoded response body and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="request">The request.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the paginated response.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		internal static async Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this IRequest request, string propertyName, JsonConverter jsonConverter = null)
		{
			var response = await request.AsResponse().ConfigureAwait(false);
			return await response.AsPaginatedResponseWithTokenAndDateRange<T>(propertyName, jsonConverter).ConfigureAwait(false);
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

		internal static void AddPropertyIfValue(this JObject jsonObject, string propertyName, string value)
		{
			if (string.IsNullOrEmpty(value)) return;
			jsonObject.AddDeepProperty(propertyName, value);
		}

		internal static void AddPropertyIfValue<T>(this JObject jsonObject, string propertyName, T value, JsonConverter converter = null)
		{
			if (EqualityComparer<T>.Default.Equals(value, default)) return;

			var jsonSerializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };
			if (converter != null)
			{
				jsonSerializer.Converters.Add(converter);
			}

			jsonObject.AddDeepProperty(propertyName, JToken.FromObject(value, jsonSerializer));
		}

		internal static void AddPropertyIfValue<T>(this JObject jsonObject, string propertyName, IEnumerable<T> value, JsonConverter converter = null)
		{
			if (value == null || !value.Any()) return;

			var jsonSerializer = new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore };
			if (converter != null)
			{
				jsonSerializer.Converters.Add(converter);
			}

			jsonObject.AddDeepProperty(propertyName, JArray.FromObject(value.ToArray(), jsonSerializer));
		}

		internal static void AddPropertyIfEnumValue<T>(this JObject jsonObject, string propertyName, T value, JsonConverter converter = null)
		{
			var serializerSettings = new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Ignore
			};
			if (converter != null) serializerSettings.Converters.Add(converter);

			AddPropertyIfValue(jsonObject, propertyName, value, v => JToken.Parse(JsonConvert.SerializeObject(v, serializerSettings)).ToString());
		}

		internal static void AddPropertyIfValue<T>(this JObject jsonObject, string propertyName, T value, Func<T, JToken> convertValueToJsonToken)
		{
			if (convertValueToJsonToken == null) throw new ArgumentNullException(nameof(convertValueToJsonToken));

			if (EqualityComparer<T>.Default.Equals(value, default)) return;

			jsonObject.AddDeepProperty(propertyName, convertValueToJsonToken(value));
		}

		internal static void AddDeepProperty(this JObject jsonObject, string propertyName, JToken value)
		{
			var separatorLocation = propertyName.IndexOf('/');

			if (separatorLocation == -1)
			{
				jsonObject.Add(propertyName, value);
			}
			else
			{
				var name = propertyName.Substring(0, separatorLocation);
				var childrenName = propertyName.Substring(separatorLocation + 1);

				var property = jsonObject.Value<JObject>(name);
				if (property == null)
				{
					property = new JObject();
					jsonObject.Add(name, property);
				}

				property.AddDeepProperty(childrenName, value);
			}
		}

		internal static JToken GetProperty(this JToken item, string name, bool throwIfMissing = true)
		{
			var parts = name.Split('/');
			var property = item[parts[0]];
			if (property == null)
			{
				if (throwIfMissing) throw new ArgumentException($"Unable to find '{name}'", nameof(name));
				else return null;
			}

			foreach (var part in parts.Skip(1))
			{
				property = property[part];
				if (property == null)
				{
					if (throwIfMissing) throw new ArgumentException($"Unable to find '{name}'", nameof(name));
					else return null;
				}
			}

			return property;
		}

		internal static T GetPropertyValue<T>(this JToken item, string name, T defaultValue)
		{
			var property = item.GetProperty(name, false);
			if (property == null) return defaultValue;
			return property.Value<T>();
		}

		internal static T GetPropertyValue<T>(this JToken item, string name)
		{
			var property = item.GetProperty(name, true);
			return property.Value<T>();
		}

		internal static async Task<TResult[]> ForEachAsync<T, TResult>(this IEnumerable<T> items, Func<T, Task<TResult>> action, int maxDegreeOfParalellism)
		{
			var allTasks = new List<Task<TResult>>();
			var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParalellism);
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

		internal static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> action, int maxDegreeOfParalellism)
		{
			var allTasks = new List<Task>();
			var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParalellism);
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

		internal static async Task<(bool, string, int?)> GetErrorMessage(this HttpResponseMessage message)
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
					var jObject = JObject.Parse(responseContent);

					errorCode = jObject.GetPropertyValue<int?>("code", null);
					errorMessage = jObject.GetPropertyValue<string>("message", errorCode.HasValue ? $"Error code: {errorCode}" : null);

					isError = errorCode.HasValue || !string.IsNullOrEmpty(errorMessage);
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
			var enumMemberAttribute = enumValue.GetAttributeOfType<EnumMemberAttribute>();
			if (enumMemberAttribute != null) return enumMemberAttribute.Value;

			var descriptionAttribute = enumValue.GetAttributeOfType<DescriptionAttribute>();
			if (descriptionAttribute != null) return descriptionAttribute.Description;

			return enumValue.ToString();
		}

		/// <summary>Parses a string into its corresponding enum value.</summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="str">The string value.</param>
		/// <returns>The enum representation of the string value.</returns>
		/// <remarks>Inspired by: https://stackoverflow.com/questions/10418651/using-enummemberattribute-and-doing-automatic-string-conversions .</remarks>
		internal static T ToEnum<T>(this string str)
			where T : Enum
		{
			var enumType = typeof(T);
			foreach (var name in Enum.GetNames(enumType))
			{
				var customAttributes = enumType.GetField(name).GetCustomAttributes(true);

				// See if there's a matching 'EnumMember' attribute
				if (customAttributes.OfType<EnumMemberAttribute>().Any(attribute => string.Equals(attribute.Value, str, StringComparison.OrdinalIgnoreCase))) return (T)Enum.Parse(enumType, name);

				// See if there's a matching 'Description' attribute
				if (customAttributes.OfType<DescriptionAttribute>().Any(attribute => string.Equals(attribute.Description, str, StringComparison.OrdinalIgnoreCase))) return (T)Enum.Parse(enumType, name);

				// See if the value matches the name
				if (string.Equals(name, str, StringComparison.OrdinalIgnoreCase)) return (T)Enum.Parse(enumType, name);
			}

			throw new ArgumentException($"There is no value in the {enumType.Name} enum that corresponds to '{str}'.");
		}

		/// <summary>Asynchronously converts the JSON encoded content and convert it to an object of the desired type.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the strongly typed object.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<T> AsObject<T>(this HttpContent httpContent, string propertyName = null, bool throwIfPropertyIsMissing = true, JsonConverter jsonConverter = null)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);

			var serializer = new JsonSerializer();
			if (jsonConverter != null) serializer.Converters.Add(jsonConverter);

			if (!string.IsNullOrEmpty(propertyName))
			{
				var jObject = JObject.Parse(responseContent);
				var jProperty = jObject.Property(propertyName);
				if (jProperty == null)
				{
					if (throwIfPropertyIsMissing)
					{
						throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
					}
					else
					{
						return default;
					}
				}

				return jProperty.Value.ToObject<T>(serializer);
			}
			else if (typeof(T).IsArray)
			{
				return JArray.Parse(responseContent).ToObject<T>(serializer);
			}
			else
			{
				return JObject.Parse(responseContent).ToObject<T>(serializer);
			}
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<JObject> AsRawJsonObject(this HttpContent httpContent, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);

			if (!string.IsNullOrEmpty(propertyName))
			{
				var jObject = JObject.Parse(responseContent);
				var jProperty = jObject.Property(propertyName);
				if (jProperty == null)
				{
					if (throwIfPropertyIsMissing)
					{
						throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
					}
					else
					{
						return default;
					}
				}

				return (JObject)jProperty.Value;
			}
			else
			{
				return JObject.Parse(responseContent);
			}
		}

		/// <summary>Get a raw JSON object representation of the response, which can also be accessed as a <c>dynamic</c> value.</summary>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="throwIfPropertyIsMissing">Indicates if an exception should be thrown when the specified JSON property is missing from the response.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<JArray> AsRawJsonArray(this HttpContent httpContent, string propertyName = null, bool throwIfPropertyIsMissing = true)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);

			if (!string.IsNullOrEmpty(propertyName))
			{
				var jObject = JObject.Parse(responseContent);
				var jProperty = jObject.Property(propertyName);
				if (jProperty == null)
				{
					if (throwIfPropertyIsMissing)
					{
						throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
					}
					else
					{
						return default;
					}
				}

				return (JArray)jProperty.Value;
			}
			else
			{
				return JArray.Parse(responseContent);
			}
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponse' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this HttpContent httpContent, string propertyName, JsonConverter jsonConverter = null)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);
			var jObject = JObject.Parse(responseContent);

			var serializer = new JsonSerializer();
			if (jsonConverter != null) serializer.Converters.Add(jsonConverter);

			var pageCount = jObject.GetPropertyValue<int>("page_count", 0);
			var pageNumber = jObject.GetPropertyValue<int>("page_number", 0);
			var pageSize = jObject.GetPropertyValue<int>("page_size", 0);
			var totalRecords = jObject.GetPropertyValue<int?>("total_records", null);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			var jProperty = jObject.Property(propertyName);
			if (jProperty == null && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponse<T>()
			{
				PageCount = pageCount,
				PageNumber = pageNumber,
				PageSize = pageSize,
				Records = jProperty?.Value.ToObject<T[]>(serializer) ?? Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponseWithToken<T>> AsPaginatedResponseWithToken<T>(this HttpContent httpContent, string propertyName, JsonConverter jsonConverter = null)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);
			var jObject = JObject.Parse(responseContent);

			var serializer = new JsonSerializer();
			if (jsonConverter != null) serializer.Converters.Add(jsonConverter);

			var nextPageToken = jObject.GetPropertyValue<string>("next_page_token", string.Empty);
			var pageSize = jObject.GetPropertyValue<int>("page_size", 0);
			var totalRecords = jObject.GetPropertyValue<int?>("total_records", null);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			var jProperty = jObject.Property(propertyName);
			if (jProperty == null && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithToken<T>()
			{
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jProperty?.Value.ToObject<T[]>(serializer) ?? Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		/// <summary>Asynchronously retrieve the JSON encoded content and convert it to a 'PaginatedResponseWithToken' object.</summary>
		/// <typeparam name="T">The response model to deserialize into.</typeparam>
		/// <param name="httpContent">The content.</param>
		/// <param name="propertyName">The name of the JSON property (or null if not applicable) where the desired data is stored.</param>
		/// <param name="jsonConverter">Converter that will be used during deserialization.</param>
		/// <returns>Returns the response body, or <c>null</c> if the response has no body.</returns>
		/// <exception cref="ApiException">An error occurred processing the response.</exception>
		private static async Task<PaginatedResponseWithTokenAndDateRange<T>> AsPaginatedResponseWithTokenAndDateRange<T>(this HttpContent httpContent, string propertyName, JsonConverter jsonConverter = null)
		{
			var responseContent = await httpContent.ReadAsStringAsync(null).ConfigureAwait(false);
			var jObject = JObject.Parse(responseContent);

			var serializer = new JsonSerializer();
			if (jsonConverter != null) serializer.Converters.Add(jsonConverter);

			var from = DateTime.ParseExact(jObject.GetPropertyValue<string>("from"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var to = DateTime.ParseExact(jObject.GetPropertyValue<string>("to"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
			var nextPageToken = jObject.GetPropertyValue<string>("next_page_token", string.Empty);
			var pageSize = jObject.GetPropertyValue<int>("page_size", 0);
			var totalRecords = jObject.GetPropertyValue<int?>("total_records", null);

			// Make sure the desired property is present. It's ok if the property is missing when there are no records.
			var jProperty = jObject.Property(propertyName);
			if (jProperty == null && pageSize > 0)
			{
				throw new ArgumentException($"The response does not contain a field called '{propertyName}'", nameof(propertyName));
			}

			var result = new PaginatedResponseWithTokenAndDateRange<T>()
			{
				From = from,
				To = to,
				NextPageToken = nextPageToken,
				PageSize = pageSize,
				Records = jProperty?.Value.ToObject<T[]>(serializer) ?? Array.Empty<T>()
			};
			if (totalRecords.HasValue) result.TotalRecords = totalRecords.Value;

			return result;
		}

		/// <summary>
		/// Casts the result type of the input task as if it were covariant.
		/// </summary>
		/// <typeparam name="T">The original result type of the task.</typeparam>
		/// <typeparam name="TResult">The covariant type to return.</typeparam>
		/// <param name="task">The target task to cast.</param>
		internal static async Task<TResult> AsTask<T, TResult>(this Task<T> task)
			where T : TResult
			where TResult : class
		{
			return await task;
		}
	}
}
