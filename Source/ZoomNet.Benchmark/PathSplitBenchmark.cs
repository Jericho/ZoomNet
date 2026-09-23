using BenchmarkDotNet.Attributes;
using System.Text.Json;
using ZoomNet;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class PathSplitBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private JsonElement _element;
	private string _path = "object/participant/id";

	[GlobalSetup]
	public void Setup()
	{
		var json = "{ \"object\": { \"participant\": { \"id\": 42 } } }";
		using var doc = JsonDocument.Parse(json);
		_element = doc.RootElement.Clone();
	}

	[Benchmark(Baseline = true)]
	public void Current_GetProperty()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var v = _element.GetProperty(_path, false);
			_ = v.HasValue;
		}
	}

	[Benchmark]
	public void PreSplit_Traverse()
	{
		var parts = _path.Split('/');
		for (int i = 0; i < Iterations; i++)
		{
			var property = _element;
			bool ok = true;
			foreach (var part in parts)
			{
				if (!property.TryGetProperty(part, out property)) { ok = false; break; }
			}
			_ = ok;
		}
	}
}
