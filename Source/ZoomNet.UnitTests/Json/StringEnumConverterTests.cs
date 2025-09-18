using Shouldly;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class StringEnumConverterTests
	{
		public enum EnumWithEnumMemberAttribute
		{
			[EnumMember(Value = "")]
			FirstValue = 1,

			SecondValue = 2, // <-- intentionally left without an attribute

			[EnumMember(Value = "third")]
			ThirdValue = 3,
		}

		public enum EnumWithDefaultJsonProperty
		{
			[JsonPropertyName("")]
			DefaultValue,
		}

		public enum EnumWithDefaultDescriptionAttribute
		{
			[DescriptionAttribute("")]
			DefaultValue,
		}

		public enum EnumWithoutDefault
		{
			[EnumMember(Value = "first")]
			DefaultFirstValue,
		}

		[Theory]
		[InlineData("null", EnumWithEnumMemberAttribute.FirstValue)]
		[InlineData("\"\"", EnumWithEnumMemberAttribute.FirstValue)]
		[InlineData("2", EnumWithEnumMemberAttribute.SecondValue)]
		[InlineData("\"third\"", EnumWithEnumMemberAttribute.ThirdValue)]
		public void Read_EnumMember(string value, EnumWithEnumMemberAttribute expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithEnumMemberAttribute>();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData("null", EnumWithDefaultJsonProperty.DefaultValue)]
		[InlineData("\"\"", EnumWithDefaultJsonProperty.DefaultValue)]
		public void Read_JsonPropertyAttribute(string value, EnumWithDefaultJsonProperty expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithDefaultJsonProperty>();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData("null", EnumWithDefaultDescriptionAttribute.DefaultValue)]
		[InlineData("\"\"", EnumWithDefaultDescriptionAttribute.DefaultValue)]
		public void Read_DefaultescriptionAttribute(string value, EnumWithDefaultDescriptionAttribute expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithDefaultDescriptionAttribute>();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Fact]
		public void Throws_when_enum_does_not_have_default_value_and_JSON_value_is_null()
		{
			// Arrange
			var value = "null";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = typeof(EnumWithoutDefault);
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithoutDefault>();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe("Unable to convert a null value into a EnumWithoutDefault enum.");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Fact]
		public void Throws_when_unexpected_JSON_token()
		{
			// Arrange
			var value = "[]"; // <-- an array is not a valid token for an enum
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = typeof(EnumWithoutDefault);
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithoutDefault>();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe("Unexpected token StartArray when parsing an enum.");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Theory]
		[InlineData(EnumWithEnumMemberAttribute.FirstValue, "\"\"")]
		[InlineData(EnumWithEnumMemberAttribute.SecondValue, "2")]
		[InlineData(EnumWithEnumMemberAttribute.ThirdValue, "\"third\"")]
		public void Write(EnumWithEnumMemberAttribute value, string expected)
		{
			// Arrange
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new StringEnumConverter<EnumWithEnumMemberAttribute>();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe(expected);
		}
	}
}
