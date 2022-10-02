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

		public override bool CanConvert(Type type)
		{
			return type.IsEnum;
		}

		public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
		{
			return (JsonConverter)Activator.CreateInstance(GetEnumConverterType(type));
		}

		private static Type GetEnumConverterType(Type enumType) => typeof(StringEnumConverter<>).MakeGenericType(enumType);
	}
}
