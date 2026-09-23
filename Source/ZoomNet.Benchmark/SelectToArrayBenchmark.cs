using BenchmarkDotNet.Attributes;
using System.Linq;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class SelectToArrayBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private int[] _values = null!;

	[GlobalSetup]
	public void Setup()
	{
		_values = Enumerable.Range(0, 100).ToArray();
	}

	[Benchmark(Baseline = true)]
	public void Linq_Select_ToArray()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var arr = _values.Select(x => x * 2).ToArray();
			_ = arr.Length;
		}
	}

	[Benchmark]
	public void Manual_Loop_Preallocated()
	{
		for (int i = 0; i < Iterations; i++)
		{
			var arr = new int[_values.Length];
			for (int j = 0; j < _values.Length; j++) arr[j] = _values[j] * 2;
			_ = arr.Length;
		}
	}
}
