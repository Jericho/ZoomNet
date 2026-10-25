using BenchmarkDotNet.Attributes;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Benchmark;

[MemoryDiagnoser]
public class ModelRecordBenchmark
{
	[Params(100_000)]
	public int Iterations;

	private RegistrantInfo _registrant = null!;
	private RegistrantInfo _registrantCopy = null!;
	private string _registrantJson = null!;

	private CallQueue _callQueue = null!;
	private string _callQueueJson = null!;

	[GlobalSetup]
	public void Setup()
	{
		_registrant = new RegistrantInfo
		{
			EventId = 987654321,
			Id = "r_12345",
			JoinUrl = "https://example.zoom.us/j/987654321?pwd=abcdef",
			StartTime = new DateTime(2026, 1, 1, 13, 30, 0, DateTimeKind.Utc)
		};

		_registrantCopy = _registrant with { };
		_registrantJson = JsonSerializer.Serialize(_registrant);

		_callQueue = new CallQueue
		{
			Id = "cq_001",
			Name = "Support Queue",
			ExtensionId = "1001",
			ExtensionNumber = 1001,
			Status = CallQueueStatus.Active,
			Site = new Site
			{
				Id = "site_01",
				Name = "Headquarters"
			},
			PhoneNumbers =
			[
				new CallQueuePhoneNumber
				{
					Id = "pn_1",
					Number = "+1-555-0100",
					Source = CallQueueNumberSource.Internal
				},
				new CallQueuePhoneNumber
				{
					Id = "pn_2",
					Number = "+1-555-0101",
					Source = CallQueueNumberSource.External
				}
			]
		};

		_callQueueJson = JsonSerializer.Serialize(_callQueue);
	}

	[Benchmark]
	public int Serialize_RegistrantInfo()
	{
		var totalLength = 0;
		for (var i = 0; i < Iterations; i++)
		{
			totalLength += JsonSerializer.Serialize(_registrant).Length;
		}

		return totalLength;
	}

	[Benchmark]
	public long Deserialize_RegistrantInfo()
	{
		long totalEventId = 0;
		for (var i = 0; i < Iterations; i++)
		{
			var model = JsonSerializer.Deserialize<RegistrantInfo>(_registrantJson)!;
			totalEventId += model.EventId;
		}

		return totalEventId;
	}

	[Benchmark]
	public int Serialize_CallQueue()
	{
		var totalLength = 0;
		for (var i = 0; i < Iterations; i++)
		{
			totalLength += JsonSerializer.Serialize(_callQueue).Length;
		}

		return totalLength;
	}

	[Benchmark]
	public int Deserialize_CallQueue()
	{
		var totalPhoneNumbers = 0;
		for (var i = 0; i < Iterations; i++)
		{
			var model = JsonSerializer.Deserialize<CallQueue>(_callQueueJson)!;
			totalPhoneNumbers += model.PhoneNumbers.Count;
		}

		return totalPhoneNumbers;
	}

	[Benchmark]
	public int Equality_RegistrantInfo()
	{
		var matches = 0;
		for (var i = 0; i < Iterations; i++)
		{
			if (_registrant == _registrantCopy) matches++;
		}

		return matches;
	}

	[Benchmark]
	public int HashCode_RegistrantInfo()
	{
		var hash = 0;
		for (var i = 0; i < Iterations; i++)
		{
			hash ^= _registrant.GetHashCode();
		}

		return hash;
	}
}
