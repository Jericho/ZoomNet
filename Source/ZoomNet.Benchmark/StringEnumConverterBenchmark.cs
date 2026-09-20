using BenchmarkDotNet.Attributes;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ZoomNet.Json;
using ZoomNet.Utilities;

namespace ZoomNet.Benchmark
{
	[MemoryDiagnoser]
	public class StringEnumConverterBenchmark
	{
		public enum PerfEnum
		{
			[MultipleValuesEnumMember(DefaultValue = "One", OtherValues = new[] { "Uno" })]
			One = 1,

			[EnumMember(Value = "Two")]
			Two = 2,

			[JsonPropertyName("Three")]
			Three = 3,

			[Description("Four")]
			Four = 4,

			Five = 5
		}

		[Params(100_000)]
		public int Iterations;

		private PerfEnum[] _values = null!;
		private string[] _strings = null!;

		[GlobalSetup]
		public void Setup()
		{
			_values = new PerfEnum[] { PerfEnum.One, PerfEnum.Two, PerfEnum.Three, PerfEnum.Four, PerfEnum.Five };
			_strings = new[] { "One", "Two", "Three", "Four", "Five", "Uno" };
		}

		[Benchmark]
		public void New_Serialize()
		{
			for (int i = 0; i < Iterations; i++)
			{
				var v = _values[i % _values.Length];
				StringEnumConverter<PerfEnum>.TryConvert(v, out string? _, false);
			}
		}

		[Benchmark]
		public void Legacy_Serialize()
		{
			for (int i = 0; i < Iterations; i++)
			{
				var v = _values[i % _values.Length];
				LegacyConvert(v);
			}
		}

		[Benchmark]
		public void New_Deserialize()
		{
			for (int i = 0; i < Iterations; i++)
			{
				var s = _strings[i % _strings.Length];
				StringEnumConverter<PerfEnum>.TryConvert(s, out PerfEnum _);
			}
		}

		[Benchmark]
		public void Legacy_Deserialize()
		{
			for (int i = 0; i < Iterations; i++)
			{
				var s = _strings[i % _strings.Length];
				LegacyTryConvert(s, out PerfEnum _);
			}
		}

		// Legacy per-call reflection implementation that mirrors the previous Internal.TryToEnum/TryToEnumString behavior
		private static string LegacyConvert(PerfEnum enumValue)
		{
			var typeOfT = typeof(PerfEnum);
			if (!Enum.IsDefined(typeOfT, enumValue))
			{
				throw new ArgumentException($"{enumValue} is not a valid value for {typeOfT.Name}", nameof(enumValue));
			}

			var member = typeOfT.GetField(enumValue.ToString())!;
			var customAttributes = member.GetCustomAttributes(true);

			var multiple = customAttributes.OfType<MultipleValuesEnumMemberAttribute>().FirstOrDefault();
			if (multiple != null)
			{
				return multiple.DefaultValue;
			}

			var enumMember = customAttributes.OfType<EnumMemberAttribute>().FirstOrDefault();
			if (enumMember != null)
			{
				return enumMember.Value;
			}

			var jsonProp = customAttributes.OfType<JsonPropertyNameAttribute>().FirstOrDefault();
			if (jsonProp != null)
			{
				return jsonProp.Name;
			}

			var desc = customAttributes.OfType<DescriptionAttribute>().FirstOrDefault();
			if (desc != null)
			{
				return desc.Description;
			}

			return enumValue.ToString();
		}

		private static bool LegacyTryConvert(string str, out PerfEnum enumValue)
		{
			var enumType = typeof(PerfEnum);
			foreach (var name in Enum.GetNames(enumType))
			{
				var customAttributes = enumType.GetField(name)!.GetCustomAttributes(true);

				// MultipleValuesEnumMember
				if (customAttributes.OfType<MultipleValuesEnumMemberAttribute>().Any(attribute => string.Equals(attribute.DefaultValue, str, StringComparison.OrdinalIgnoreCase) ||
					(attribute.OtherValues ?? Array.Empty<string>()).Any(otherValue => string.Equals(otherValue, str, StringComparison.OrdinalIgnoreCase))))
				{
					enumValue = (PerfEnum)Enum.Parse(enumType, name);
					return true;
				}

				// EnumMember
				if (customAttributes.OfType<EnumMemberAttribute>().Any(attribute => string.Equals(attribute.Value, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (PerfEnum)Enum.Parse(enumType, name);
					return true;
				}

				// JsonPropertyName
				if (customAttributes.OfType<JsonPropertyNameAttribute>().Any(attribute => string.Equals(attribute.Name, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (PerfEnum)Enum.Parse(enumType, name);
					return true;
				}

				// Description
				if (customAttributes.OfType<DescriptionAttribute>().Any(attribute => string.Equals(attribute.Description, str, StringComparison.OrdinalIgnoreCase)))
				{
					enumValue = (PerfEnum)Enum.Parse(enumType, name);
					return true;
				}

				// Name
				if (string.Equals(name, str, StringComparison.OrdinalIgnoreCase))
				{
					enumValue = (PerfEnum)Enum.Parse(enumType, name);
					return true;
				}

				// Numeric
				if (int.TryParse(str, out int numberValue))
				{
					enumValue = (PerfEnum)Enum.ToObject(enumType, numberValue);
					return true;
				}
			}

			enumValue = default;
			return false;
		}
	}
}
