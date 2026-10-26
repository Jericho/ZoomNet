using BenchmarkDotNet.Attributes;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class ParseQuerystringBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private Uri _uri = new Uri("https://example.com");

	[GlobalSetup]
	public void Setup()
	{
		// Create a query string with many items
		var pairs = Enumerable.Range(0, 20).Select(i => $"key{i}=value{i}");
		var qs = string.Join("&", pairs);
		_uri = new Uri($"https://example.com/path?{qs}");
	}

	[Benchmark(Baseline = true)]
	public void Legacy()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var result = _uri
				.Query.TrimStart('?')
				.Split(['&'], StringSplitOptions.RemoveEmptyEntries)
				.Select(value => value.Split(['='], StringSplitOptions.RemoveEmptyEntries))
				.Select(splitValue =>
				{
					var key = splitValue[0].Trim();
					var value = splitValue.Length > 1 ? splitValue[1].Trim() : null;
					return new KeyValuePair<string, string?>(key, value);
				});

			// iterate to ensure work is done
			foreach (var kvp in result) { _ = kvp.Key; }
		}
	}

	[Benchmark]
	public void Current()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var result = _uri.ParseQuerystring();

			// iterate to ensure work is done
			foreach (var kvp in result) { _ = kvp.Key; }
		}
	}
}
