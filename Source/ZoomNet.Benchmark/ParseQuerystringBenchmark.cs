using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using ZoomNet;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class ParseQuerystringBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private Uri _uri;

	[GlobalSetup]
	public void Setup()
	{
		// Create a query string with many items
		var pairs = Enumerable.Range(0, 20).Select(i => $"key{i}=value{i}");
		var qs = string.Join("&", pairs);
		_uri = new Uri($"https://example.com/path?{qs}");
	}

	[Benchmark(Baseline = true)]
	public void Current_SplitBased()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var result = _uri.ParseQuerystring();
			// iterate to ensure work is done
			foreach (var kvp in result) { _ = kvp.Key; }
		}
	}

	[Benchmark]
	public void Manual_IndexOf()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var q = _uri.Query;
			if (q.Length == 0) continue;
			var s = q.AsSpan().TrimStart('?');
			var j = 0;
			var list = new List<KeyValuePair<string, string>>();
			while (j < s.Length)
			{
				var amp = s[j..].IndexOf('&');
				var segment = amp == -1 ? s[j..] : s.Slice(j, amp);
				var eq = segment.IndexOf('=');
				if (eq == -1)
				{
					list.Add(new KeyValuePair<string, string>(segment.ToString().Trim(), null));
				}
				else
				{
					var key = segment.Slice(0, eq).ToString().Trim();
					var value = segment.Slice(eq + 1).ToString().Trim();
					list.Add(new KeyValuePair<string, string>(key, value));
				}

				if (amp == -1) break;
				j += amp + 1;
			}
		}
	}
}
