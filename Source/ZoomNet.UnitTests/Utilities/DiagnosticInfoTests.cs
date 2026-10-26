using Pathoschild.Http.Client;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class DiagnosticInfoTests
	{
		public static TheoryData<bool, bool, bool, bool> ParameterCombinations()
		{
			var data = new TheoryData<bool, bool, bool, bool>();
			for (int a = 0; a < 2; a++)
				for (int b = 0; b < 2; b++)
					for (int c = 0; c < 2; c++)
						for (int d = 0; d < 2; d++)
							data.Add(a == 1, b == 1, c == 1, d == 1);

			return data;
		}

		private static DiagnosticInfo CreateDiagnostic(out HttpRequestMessage request, out HttpResponseMessage response, long elapsedTicks = TimeSpan.TicksPerMillisecond * 123)
		{
			request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://example.com/path"))
			{
				Version = new Version(1, 1),
				Content = new StringContent("request-body", Encoding.UTF8)
			};
			request.Headers.Add("Authorization", "secret");
			request.Headers.Add("X-Test", "value");

			response = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Version = new Version(2, 0),
				ReasonPhrase = "OK",
				Content = new StringContent("response-body", Encoding.UTF8),
				RequestMessage = request
			};
			response.Headers.Add("Y-Header", "yvalue");

			return new DiagnosticInfo(new WeakReference<HttpRequestMessage>(request), 0, new WeakReference<HttpResponseMessage>(response), elapsedTicks, new RequestOptions());
		}

		[Theory]
		[MemberData(nameof(ParameterCombinations))]
		public void GetLoggingTemplate_Structured(bool logReqHdrs, bool logReqContent, bool logResHdrs, bool logResContent)
		{
			// Arrange
			var diag = CreateDiagnostic(out var request, out var response);

			// Act
			var template = diag.GetLoggingTemplate(true, logReqHdrs, logReqContent, logResHdrs, logResContent);
			var parameters = diag.GetLoggingParameters(logReqHdrs, logReqContent, logResHdrs, logResContent);

			// Extract named tokens from structured template
			var tokenMatches = Regex.Matches(template, "\\{([A-Za-z0-9_\\-]+)(?:[:][^}]*)?\\}");
			var tokens = tokenMatches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();

			// Build expected tokens in the same order as DiagnosticInfo
			var expectedTokens = new List<string>();
			if (request != null)
			{
				expectedTokens.Add("Request_HttpMethod");
				expectedTokens.Add("Request_Uri");
				expectedTokens.Add("Request_HttpVersion");

				if (logReqHdrs)
				{
					var requestHeaders = request.Headers.AsEnumerable();
					if (!requestHeaders.Any(kvp => string.Equals(kvp.Key, "Content-Length", StringComparison.OrdinalIgnoreCase)))
					{
						requestHeaders = requestHeaders.Append(new KeyValuePair<string, IEnumerable<string>>("Content-Length", ["For the purpose of this unit test, no need to calculate the actual content length"]));
					}

					foreach (var header in requestHeaders.OrderBy(kvp => kvp.Key))
					{
						expectedTokens.Add("Request_Header_" + header.Key);
					}
				}

				if (logReqContent) expectedTokens.Add("Request_Content");
			}

			if (response != null)
			{
				expectedTokens.Add("Response_HttpVersion");
				expectedTokens.Add("Response_StatusCode");
				expectedTokens.Add("Response_ReasonPhrase");

				if (logResHdrs)
				{
					var responseHeaders = response.Headers.AsEnumerable();
					if (!responseHeaders.Any(kvp => string.Equals(kvp.Key, "Content-Length", StringComparison.OrdinalIgnoreCase)))
					{
						responseHeaders = responseHeaders.Append(new KeyValuePair<string, IEnumerable<string>>("Content-Length", ["For the purpose of this unit test, no need to calculate the actual content length"]));
					}

					foreach (var header in responseHeaders.OrderBy(kvp => kvp.Key))
					{
						expectedTokens.Add("Response_Header_" + header.Key);
					}
				}

				if (logResContent) expectedTokens.Add("Response_Content");
			}

			expectedTokens.Add("Diagnostic_Elapsed");

			// Assert tokens match expected sequence
			tokens.ShouldBe(expectedTokens);

			// Ensure token count equals parameters length
			tokens.Count.ShouldBe(parameters.Length);

			// Spot-check token -> parameter mappings
			var elapsedMs = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 123).TotalMilliseconds;
			if (tokens.Contains("Request_HttpMethod")) parameters[tokens.IndexOf("Request_HttpMethod")].ShouldBe(request.Method.Method);
			if (tokens.Contains("Request_Uri")) parameters[tokens.IndexOf("Request_Uri")].ShouldBe(request.RequestUri);
			if (tokens.Contains("Request_Header_Authorization")) parameters[tokens.IndexOf("Request_Header_Authorization")].ShouldBe("... omitted for security reasons ...");
			if (tokens.Contains("Diagnostic_Elapsed")) parameters[tokens.IndexOf("Diagnostic_Elapsed")].ShouldBe(elapsedMs);
		}

		[Theory]
		[MemberData(nameof(ParameterCombinations))]
		public void GetLoggingTemplate_NonStructured(bool logReqHdrs, bool logReqContent, bool logResHdrs, bool logResContent)
		{
			// Arrange
			var diag = CreateDiagnostic(out var request, out var response);

			// Act
			var template = diag.GetLoggingTemplate(false, logReqHdrs, logReqContent, logResHdrs, logResContent);
			var parameters = diag.GetLoggingParameters(logReqHdrs, logReqContent, logResHdrs, logResContent);

			// Ensure the template numeric placeholders correspond to the parameters
			var matches = Regex.Matches(template, "\\{(\\d+)(?:[:][^}]*)?\\}");
			var indices = matches.Cast<Match>().Select(m => int.Parse(m.Groups[1].Value)).Distinct().OrderBy(i => i).ToArray();
			indices.Length.ShouldBe(parameters.Length);
			if (indices.Length > 0) indices.Max().ShouldBe(parameters.Length - 1);

			var formatted = string.Format(template, parameters);
			formatted.ShouldContain("DIAGNOSTIC");

			// Ensure sections are only present when the corresponding flags are enabled
			if (logReqHdrs) template.ShouldContain("REQUEST HEADERS:"); else template.ShouldNotContain("REQUEST HEADERS:");
			if (logReqContent) template.ShouldContain("REQUEST: {"); else template.ShouldNotContain("REQUEST: {");
		}

		[Theory]
		[MemberData(nameof(ParameterCombinations))]
		public void GetLoggingParameters_AllCombinations_ReturnsExpectedLengthAndOrder(bool logReqHdrs, bool logReqContent, bool logResHdrs, bool logResContent)
		{
			// Arrange
			var diag = CreateDiagnostic(out var request, out var response, TimeSpan.TicksPerMillisecond * 123);
			var elapsedMs = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 123).TotalMilliseconds;

			// Act
			var parameters = diag.GetLoggingParameters(logReqHdrs, logReqContent, logResHdrs, logResContent);

			// Calculate expected count
			var expected = 0;
			if (request != null)
			{
				expected += 3; // method, uri, version
				if (logReqHdrs) expected += 3; // Authorization, Content-Length (added), X-Test
				if (logReqContent) expected += 1;
			}
			if (response != null)
			{
				expected += 3; // version, status, reason
				if (logResHdrs) expected += 2; // Content-Length (added), Y-Header
				if (logResContent) expected += 1;
			}
			expected += 1; // elapsed

			// Assert length
			parameters.Length.ShouldBe(expected);

			// Spot checks when request/response present
			if (request != null)
			{
				parameters[0].ShouldBe(request.Method.Method);
				parameters[1].ShouldBe(request.RequestUri);
			}

			parameters.Last().ShouldBe(elapsedMs);
		}
	}
}
