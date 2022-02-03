using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Text;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace StrongGrid.UnitTests.Utilities
{
	public class ParticipantDeviceConverterTests
	{
		[Fact]
		public void Properties()
		{
			// Act
			var converter = new ParticipantDeviceConverter();

			// Assert
			converter.CanRead.ShouldBeTrue();
			converter.CanWrite.ShouldBeTrue();
		}

		[Fact]
		public void CanConvert()
		{
			// Act
			var converter = new ParticipantDeviceConverter();

			// Assert
			converter.CanConvert(typeof(string)).ShouldBeTrue();
		}

		[Fact]
		public void Write_single()
		{
			// Arrange
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			var writer = new JsonTextWriter(sw);

			var value = new[]
			{
				ParticipantDevice.Windows
			};

			var serializer = new JsonSerializer();

			var converter = new ParticipantDeviceConverter();

			// Act
			converter.WriteJson(writer, value, serializer);
			var result = sb.ToString();

			// Assert
			result.ShouldBe("\"Windows\"");
		}
		[Fact]
		public void Write_multiple()
		{
			// Arrange
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			var writer = new JsonTextWriter(sw);

			var value = new[]
			{
				ParticipantDevice.Unknown,
				ParticipantDevice.Phone
			};

			var serializer = new JsonSerializer();

			var converter = new ParticipantDeviceConverter();

			// Act
			converter.WriteJson(writer, value, serializer);
			var result = sb.ToString();

			// Assert
			result.ShouldBe("\"Unknown + Phone\"");
		}

		[Fact]
		public void Read_single()
		{
			// Arrange
			var json = "'Phone'";

			var textReader = new StringReader(json);
			var jsonReader = new JsonTextReader(textReader);
			var objectType = (Type)null;
			var existingValue = (object)null;
			var serializer = new JsonSerializer();

			var converter = new ParticipantDeviceConverter();

			// Act
			jsonReader.Read();
			var result = converter.ReadJson(jsonReader, objectType, existingValue, serializer);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ParticipantDevice[]>();

			var resultAsArray = (ParticipantDevice[])result;
			resultAsArray.Length.ShouldBe(1);
			resultAsArray[0].ShouldBe(ParticipantDevice.Phone);
		}

		[Fact]
		public void Read_multiple()
		{
			// Arrange
			var json = "'Unknown + Phone'";

			var textReader = new StringReader(json);
			var jsonReader = new JsonTextReader(textReader);
			var objectType = (Type)null;
			var existingValue = (object)null;
			var serializer = new JsonSerializer();

			var converter = new ParticipantDeviceConverter();

			// Act
			jsonReader.Read();
			var result = converter.ReadJson(jsonReader, objectType, existingValue, serializer);

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
