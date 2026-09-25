using BenchmarkDotNet.Attributes;
using System.Text.Json;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class PathSplitBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private JsonElement _element;
	private readonly string _path = "object/participant/id";

	[GlobalSetup]
	public void Setup()
	{
		var json = "{ \"object\": { \"participant\": { \"id\": 42 } } }";
		using var doc = JsonDocument.Parse(json);
		_element = doc.RootElement.Clone();
	}

	[Benchmark]
	public void New_GetProperty()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var v = NewGetProperty(_element, _path, false);
			_ = v.HasValue;
		}
	}

	[Benchmark(Baseline = true)]
	public void Legacy_GetProperty()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var v = LegacyGetProperty(_element, _path, false);
			_ = v.HasValue;
		}
	}

	private static JsonElement? LegacyGetProperty(JsonElement element, string path, bool throwIfMissing = true, char splitChar = '/')
	{
		var parts = path.Split(splitChar);
		var property = element;

		foreach (var part in parts)
		{
			if (!property.TryGetProperty(part, out property))
			{
				if (throwIfMissing) throw new ArgumentException($"Unable to find '{path}'", nameof(path));
				else return null;
			}
		}

		return property;
	}

	private static JsonElement? NewGetProperty(JsonElement element, string path, bool throwIfMissing = true, char splitChar = '/')
	{
		ArgumentNullException.ThrowIfNull(path);

		var property = element;
		var span = path.AsSpan();
		int start = 0;
		while (start <= span.Length)
		{
			int idx = start >= span.Length ? -1 : span.Slice(start).IndexOf(splitChar);
			ReadOnlySpan<char> partSpan = idx == -1 ? span.Slice(start) : span.Slice(start, idx);
			string part = partSpan.ToString();
			if (!property.TryGetProperty(part, out property))
			{
				if (throwIfMissing) throw new ArgumentException($"Unable to find '{path}'", nameof(path));
				return null;
			}

			if (idx == -1) break;
			start += idx + 1;
		}

		return property;
	}
}
