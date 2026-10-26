using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	internal sealed class EnumConverterFactory : JsonConverterFactory
	{
		public EnumConverterFactory()
		{
		}

		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsEnum;
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			return (JsonConverter)Activator.CreateInstance(GetEnumConverterType(typeToConvert));
		}

		private static Type GetEnumConverterType(Type enumType) => typeof(StringEnumConverter<>).MakeGenericType(enumType);
	}
}
