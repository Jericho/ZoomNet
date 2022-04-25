using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Utilities
{
	public class ParticipantDeviceConverterTests
	{
		[Fact]
		public void Write_single()
		{
			// Arrange
			var value = new[]
			{
				ParticipantDevice.Windows
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new ParticipantDeviceConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("\"Windows\"");
		}

		[Fact]
		public void Write_multiple()
		{
			// Arrange
			var value = new[]
			{
				ParticipantDevice.Unknown,
				ParticipantDevice.Phone
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new ParticipantDeviceConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("\"Unknown \\u002B Phone\"");
		}

		[Theory]
		[InlineData("", ParticipantDevice.Unknown)]
		[InlineData("Unknown", ParticipantDevice.Unknown)]
		[InlineData("Android", ParticipantDevice.Android)]
		[InlineData("Phone", ParticipantDevice.Phone)]
		[InlineData("iOs", ParticipantDevice.IOS)]
		[InlineData("H.323/SIP", ParticipantDevice.Sip)]
		[InlineData("Windows", ParticipantDevice.Windows)]
		public void Read_single(string value, ParticipantDevice expectedValue)
		{
			// Arrange
			var json = $"\"{value}\"";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;

			var converter = new ParticipantDeviceConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, ZoomNetJsonFormatter.DeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ParticipantDevice[]>();

			var resultAsArray = (ParticipantDevice[])result;
			resultAsArray.Length.ShouldBe(1);
			resultAsArray[0].ShouldBe(expectedValue);
		}

		[Fact]
		public void Read_multiple()
		{
			// Arrange
			var json = "\"Unknown + Phone\"";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;

			var converter = new ParticipantDeviceConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, ZoomNetJsonFormatter.DeserializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ParticipantDevice[]>();

			var resultAsArray = (ParticipantDevice[])result;
			resultAsArray.Length.ShouldBe(2);
			resultAsArray[0].ShouldBe(ParticipantDevice.Unknown);
			resultAsArray[1].ShouldBe(ParticipantDevice.Phone);
		}
	}
}
