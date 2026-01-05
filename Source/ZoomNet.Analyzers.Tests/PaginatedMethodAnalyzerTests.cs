using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using System.Threading.Tasks;
using Xunit;

namespace ZoomNet.Analyzers.Tests
{
	public class PaginatedMethodAnalyzerTests
	{
		[Fact]
		public async Task AnalyzerDetectsPaginatedResponseWithToken()
		{
			var test = @"
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithToken<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
        public bool MoreRecordsAvailable => !string.IsNullOrEmpty(NextPageToken);
    }
}

namespace ZoomNet.Resources
{
    public interface IMeetings
    {
        Task<ZoomNet.Models.PaginatedResponseWithToken<string>> {|#0:GetAllAsync|}(string userId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);
    }
}";

			var expected = new DiagnosticResult(PaginatedMethodAnalyzer.DiagnosticId, DiagnosticSeverity.Info)
				.WithLocation(0)
				.WithArguments("GetAllAsync", "ZoomNet.Models.PaginatedResponseWithToken<string>");

			await VerifyAnalyzer(test, expected);
		}

		[Fact]
		public async Task AnalyzerDetectsPaginatedResponseWithTokenAndDateRange()
		{
			var test = @"
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithTokenAndDateRange<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool MoreRecordsAvailable => !string.IsNullOrEmpty(NextPageToken);
    }
}

namespace ZoomNet.Resources
{
    public interface ICloudRecordings
    {
        Task<ZoomNet.Models.PaginatedResponseWithTokenAndDateRange<string>> {|#0:GetRecordingsForUserAsync|}(
            string userId, 
            bool queryTrash = false, 
            DateTime? from = null, 
            DateTime? to = null, 
            int recordsPerPage = 30, 
            string pagingToken = null, 
            CancellationToken cancellationToken = default);
    }
}";

			var expected = new DiagnosticResult(PaginatedMethodAnalyzer.DiagnosticId, DiagnosticSeverity.Info)
				.WithLocation(0)
				.WithArguments("GetRecordingsForUserAsync", "ZoomNet.Models.PaginatedResponseWithTokenAndDateRange<string>");

			await VerifyAnalyzer(test, expected);
		}

		[Fact]
		public async Task AnalyzerIgnoresObsoleteMethods()
		{
			var test = @"
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithToken<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
    }
}

namespace ZoomNet.Resources
{
    public interface IMeetings
    {
        [Obsolete]
        Task<ZoomNet.Models.PaginatedResponseWithToken<string>> GetAllAsync(string userId, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);
    }
}";

			// Should not produce any diagnostics for obsolete methods
			await VerifyAnalyzer(test);
		}

		[Fact]
		public async Task AnalyzerIgnoresPrivateMethods()
		{
			var test = @"
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithToken<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
    }
}

namespace ZoomNet.Resources
{
    public class Meetings
    {
        private Task<ZoomNet.Models.PaginatedResponseWithToken<string>> GetAllAsync(string userId)
        {
            return null;
        }
    }
}";

			// Should not produce diagnostics for private methods
			await VerifyAnalyzer(test);
		}

		[Fact]
		public async Task AnalyzerIgnoresNonInterfaceMethods()
		{
			var test = @"
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithToken<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
    }
}

namespace ZoomNet.Resources
{
    public class Meetings
    {
        public Task<ZoomNet.Models.PaginatedResponseWithToken<string>> GetAllAsync(string userId)
        {
            return null;
        }
    }
}";

			// Should not produce diagnostics for class methods
			await VerifyAnalyzer(test);
		}

		[Fact]
		public async Task AnalyzerIgnoresNonTaskReturnTypes()
		{
			var test = @"
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
    public class PaginatedResponseWithToken<T>
    {
        public T[] Records { get; set; }
        public string NextPageToken { get; set; }
    }
}

namespace ZoomNet.Resources
{
    public interface IMeetings
    {
        ZoomNet.Models.PaginatedResponseWithToken<string> GetAllAsync(string userId);
    }
}";

			// Should not produce diagnostics for non-Task return types
			await VerifyAnalyzer(test);
		}

		private static async Task VerifyAnalyzer(string source, params DiagnosticResult[] expected)
		{
			var test = new CSharpAnalyzerTest<PaginatedMethodAnalyzer, DefaultVerifier>
			{
				TestCode = source,
			};

			test.ExpectedDiagnostics.AddRange(expected);
			await test.RunAsync();
		}
	}
}
