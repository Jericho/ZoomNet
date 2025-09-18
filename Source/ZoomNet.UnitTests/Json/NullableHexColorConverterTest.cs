using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class NullableHexColorConverterTests
	{
		public static IEnumerable<object[]> HexColorConverterTestData
		{
			get
			{
				yield return new object[] { "\"#FFF0FFFF\"", Color.Azure }; // Long form with leading #
				yield return new object[] { "\"FFF0FFFF\"", Color.Azure }; // Long form without leading #
				yield return new object[] { "\"F0FFFF\"", Color.Azure }; // Short form
			}
		}

		[Theory]
		[InlineData("\"\"")] // Empty string
		[InlineData("null")] // Null value
		[InlineData("\"QWERTY\"")] // Not a valid value
		public void Read_null(string value)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new NullableHexColorConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeNull();
		}

		[Theory]
		[MemberData(nameof(HexColorConverterTestData))]
		public void Read(string value, Color expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value.ToString());
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new NullableHexColorConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Value.ToArgb().ShouldBe(expected.ToArgb());
		}

		[Fact]
		public void Write_null()
		{
			// Arrange
			var value = (Color?)null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableHexColorConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("null");
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = Color.Brown;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableHexColorConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("\"#A52A2A\"");
		}
	}
}
