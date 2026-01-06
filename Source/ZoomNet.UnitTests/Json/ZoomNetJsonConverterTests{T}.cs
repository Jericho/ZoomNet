using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public partial class ZoomNetJsonConverterTests
	{
		#region Test Classes

		// Test model class without any special attributes
		internal class SimpleTestModel
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("name")]
			public string Name { get; set; }

			[JsonPropertyName("value")]
			public int Value { get; set; }
		}

		// Test model with JsonIgnore attribute
		internal class ModelWithIgnoredProperty
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("public_name")]
			public string PublicName { get; set; }

			[JsonIgnore]
			public string InternalSecret { get; set; }
		}

		// Test model with custom converter on a property
		internal class ModelWithCustomConverter
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("created_date")]
			[JsonConverter(typeof(DateTimeConverter))]
			public DateTime CreatedDate { get; set; }
		}

		// Test model with null properties
		internal class ModelWithNullableProperties
		{
			[JsonPropertyName("required_field")]
			public string RequiredField { get; set; }

			[JsonPropertyName("optional_field")]
			public string OptionalField { get; set; }

			[JsonPropertyName("optional_number")]
			public int? OptionalNumber { get; set; }
		}

		// Concrete implementation of ZoomNetJsonConverter for testing
		internal class TestConverter : ZoomNetJsonConverter<SimpleTestModel>
		{
			// Uses the base class implementation
		}

		// Concrete implementation for models with ignored properties
		internal class ModelWithIgnoredPropertyConverter : ZoomNetJsonConverter<ModelWithIgnoredProperty>
		{
		}

		// Concrete implementation for models with custom converters
		internal class ModelWithCustomConverterConverter : ZoomNetJsonConverter<ModelWithCustomConverter>
		{
		}

		// Concrete implementation for models with nullable properties
		internal class ModelWithNullablePropertiesConverter : ZoomNetJsonConverter<ModelWithNullableProperties>
		{
		}

		// Concrete implementation for array conversion
		internal class SimpleTestModelArrayConverter : ZoomNetJsonConverter<SimpleTestModel[]>
		{
		}

		[JsonSerializable(typeof(SimpleTestModel))]
		[JsonSerializable(typeof(ModelWithIgnoredProperty))]
		[JsonSerializable(typeof(ModelWithCustomConverter))]
		[JsonSerializable(typeof(ModelWithNullableProperties))]
		[JsonSerializable(typeof(SimpleTestModel[]))]
		[JsonSerializable(typeof(ModelWithIgnoredProperty[]))]
		[JsonSerializable(typeof(ModelWithCustomConverter[]))]
		[JsonSerializable(typeof(ModelWithNullableProperties[]))]
		internal partial class UnitTestingJsonSerializerContext : JsonSerializerContext
		{

		}

		#endregion

		#region Read Method Tests

		[Fact]
		public void Read_ValidJson_DeserializesCorrectly()
		{
			// Arrange
			var json = "{\"id\":\"test123\",\"name\":\"Test Name\",\"value\":42}";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new TestConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, typeof(SimpleTestModel), options);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("test123");
			result.Name.ShouldBe("Test Name");
			result.Value.ShouldBe(42);
		}

		[Fact]
		public void Read_EmptyJson_ReturnsDefault()
		{
			// Arrange
			var json = "{}";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new TestConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, typeof(SimpleTestModel), options);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBeNull();
			result.Name.ShouldBeNull();
			result.Value.ShouldBe(0);
		}

		[Fact]
		public void Read_NullJson_ReturnsNull()
		{
			// Arrange
			var json = "null";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new TestConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, typeof(SimpleTestModel), options);

			// Assert
			result.ShouldBeNull();
		}

		[Fact]
		public void Read_PartialJson_DeserializesAvailableProperties()
		{
			// Arrange
			var json = "{\"id\":\"partial123\",\"name\":\"Partial Name\"}";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new TestConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, typeof(SimpleTestModel), options);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe("partial123");
			result.Name.ShouldBe("Partial Name");
			result.Value.ShouldBe(0);
		}

		#endregion

		#region Write Method Tests

		[Fact]
		public void Write_ValidObject_SerializesCorrectly()
		{
			// Arrange
			var model = new SimpleTestModel
			{
				Id = "write123",
				Name = "Write Test",
				Value = 99
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new TestConverter();

			// Act
			converter.Write(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"id\":\"write123\"");
			json.ShouldContain("\"name\":\"Write Test\"");
			json.ShouldContain("\"value\":99");
		}

		[Fact]
		public void Write_NullObject_SerializesAsNull()
		{
			// Arrange
			SimpleTestModel nullModel = null;
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new TestConverter();

			// Act
			converter.Write(writer, nullModel, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("null");
		}

		[Fact]
		public void Write_ObjectWithNullProperties_IgnoresNullValues()
		{
			// Arrange
			var model = new ModelWithNullableProperties
			{
				RequiredField = "Required",
				OptionalField = null,
				OptionalNumber = null
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new ModelWithNullablePropertiesConverter();

			// Act
			converter.Write(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"required_field\":\"Required\"");
			json.ShouldNotContain("optional_field");
			json.ShouldNotContain("optional_number");
		}

		[Fact]
		public void Write_ObjectWithIgnoredProperty_ExcludesIgnoredProperty()
		{
			// Arrange
			var model = new ModelWithIgnoredProperty
			{
				Id = "ignored123",
				PublicName = "Public",
				InternalSecret = "Secret123"
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new ModelWithIgnoredPropertyConverter();

			// Act
			converter.Write(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"id\":\"ignored123\"");
			json.ShouldContain("\"public_name\":\"Public\"");
			json.ShouldNotContain("InternalSecret");
			json.ShouldNotContain("Secret123");
		}

		[Fact]
		public void Write_Array_SerializesAsJsonArray()
		{
			// Arrange
			var models = new[]
			{
				new SimpleTestModel { Id = "arr1", Name = "First", Value = 1 },
				new SimpleTestModel { Id = "arr2", Name = "Second", Value = 2 }
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new SimpleTestModelArrayConverter();

			// Act
			converter.Write(writer, models, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldStartWith("[");
			json.ShouldEndWith("]");
			json.ShouldContain("\"id\":\"arr1\"");
			json.ShouldContain("\"id\":\"arr2\"");
		}

		[Fact]
		public void Write_NullArray_SerializesAsNull()
		{
			// Arrange
			SimpleTestModel[] nullArray = null;
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new SimpleTestModelArrayConverter();

			// Act
			converter.Write(writer, nullArray, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("null");
		}

		[Fact]
		public void Write_EmptyArray_SerializesAsEmptyJsonArray()
		{
			// Arrange
			var emptyArray = new SimpleTestModel[0];
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new SimpleTestModelArrayConverter();

			// Act
			converter.Write(writer, emptyArray, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("[]");
		}

		#endregion

		#region SerializeArray Method Tests

		[Fact]
		public void SerializeArray_ValidArray_CreatesJsonArray()
		{
			// Arrange
			var models = new[]
			{
				new SimpleTestModel { Id = "sa1", Name = "SA First", Value = 10 },
				new SimpleTestModel { Id = "sa2", Name = "SA Second", Value = 20 },
				new SimpleTestModel { Id = "sa3", Name = "SA Third", Value = 30 }
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };

			// Act
			ZoomNetJsonConverter<SimpleTestModel[]>.SerializeArray(writer, models, typeof(SimpleTestModel[]), options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldStartWith("[");
			json.ShouldEndWith("]");
			json.ShouldContain("\"id\":\"sa1\"");
			json.ShouldContain("\"id\":\"sa2\"");
			json.ShouldContain("\"id\":\"sa3\"");
			json.ShouldContain("\"value\":10");
			json.ShouldContain("\"value\":20");
			json.ShouldContain("\"value\":30");
		}

		[Fact]
		public void SerializeArray_NullArray_WritesNullValue()
		{
			// Arrange
			SimpleTestModel[] nullArray = null;
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			// Act
			ZoomNetJsonConverter<SimpleTestModel[]>.SerializeArray(writer, nullArray, typeof(SimpleTestModel[]), options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("null");
		}

		#endregion

		#region Serialize Method Tests

		[Fact]
		public void Serialize_ValidObject_CreatesJsonObject()
		{
			// Arrange
			var model = new SimpleTestModel
			{
				Id = "ser123",
				Name = "Serialize Test",
				Value = 777
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.Serialize(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldStartWith("{");
			json.ShouldEndWith("}");
			json.ShouldContain("\"id\":\"ser123\"");
			json.ShouldContain("\"name\":\"Serialize Test\"");
			json.ShouldContain("\"value\":777");
		}

		[Fact]
		public void Serialize_NullObject_WritesNullValue()
		{
			// Arrange
			SimpleTestModel nullModel = null;
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.Serialize(writer, nullModel, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("null");
		}

		[Fact]
		public void Serialize_WithCustomPropertySerializer_UsesCustomSerializer()
		{
			// Arrange
			var model = new SimpleTestModel
			{
				Id = "custom123",
				Name = "Custom",
				Value = 555
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			var customPropertySerializerCalled = false;
			Action<string, object, Type, JsonSerializerOptions, JsonConverterAttribute> customSerializer =
				(name, value, type, opts, attr) =>
				{
					customPropertySerializerCalled = true;
					ZoomNetJsonConverter<SimpleTestModel>.WriteJsonProperty(writer, name, value, type, opts, attr);
				};

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.Serialize(writer, model, typeof(SimpleTestModel), options, customSerializer);
			writer.Flush();

			// Assert
			customPropertySerializerCalled.ShouldBeTrue();
		}

		#endregion

		#region WriteJsonProperty Method Tests

		[Fact]
		public void WriteJsonProperty_SimpleProperty_SerializesCorrectly()
		{
			// Arrange
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			writer.WriteStartObject();

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.WriteJsonProperty(writer, "test_property", "test_value", typeof(string), options, null);

			writer.WriteEndObject();
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"test_property\":\"test_value\"");
		}

		[Fact]
		public void WriteJsonProperty_NumericProperty_SerializesCorrectly()
		{
			// Arrange
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			writer.WriteStartObject();

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.WriteJsonProperty(writer, "numeric_prop", 12345, typeof(int), options, null);

			writer.WriteEndObject();
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"numeric_prop\":12345");
		}

		[Fact]
		public void WriteJsonProperty_BooleanProperty_SerializesCorrectly()
		{
			// Arrange
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;

			writer.WriteStartObject();

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.WriteJsonProperty(writer, "bool_prop", true, typeof(bool), options, null);

			writer.WriteEndObject();
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"bool_prop\":true");
		}

		#endregion

		#region RoundTrip Tests

		[Fact]
		public void RoundTrip_SimpleModel_PreservesAllData()
		{
			// Arrange
			var original = new SimpleTestModel
			{
				Id = "roundtrip123",
				Name = "RoundTrip Test Model",
				Value = 888
			};

			var converter = new TestConverter();
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserialized = converter.Read(ref jsonReader, typeof(SimpleTestModel), options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.Id.ShouldBe(original.Id);
			deserialized.Name.ShouldBe(original.Name);
			deserialized.Value.ShouldBe(original.Value);
		}

		[Fact]
		public void RoundTrip_ModelWithNullProperties_PreservesNonNullData()
		{
			// Arrange
			var original = new ModelWithNullableProperties
			{
				RequiredField = "Must Have",
				OptionalField = null,
				OptionalNumber = 42
			};

			var converter = new ModelWithNullablePropertiesConverter();
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserialized = converter.Read(ref jsonReader, typeof(ModelWithNullableProperties), options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.RequiredField.ShouldBe(original.RequiredField);
			deserialized.OptionalField.ShouldBeNull();
			deserialized.OptionalNumber.ShouldBe(original.OptionalNumber);
		}

		[Fact]
		public void RoundTrip_Array_PreservesAllElements()
		{
			// Arrange
			var original = new[]
			{
				new SimpleTestModel { Id = "rt1", Name = "First", Value = 100 },
				new SimpleTestModel { Id = "rt2", Name = "Second", Value = 200 },
				new SimpleTestModel { Id = "rt3", Name = "Third", Value = 300 }
			};

			var converter = new SimpleTestModelArrayConverter();
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var deserialized = JsonSerializer.Deserialize<SimpleTestModel[]>(json, options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.Length.ShouldBe(3);
			deserialized[0].Id.ShouldBe("rt1");
			deserialized[1].Id.ShouldBe("rt2");
			deserialized[2].Id.ShouldBe("rt3");
			deserialized[0].Value.ShouldBe(100);
			deserialized[1].Value.ShouldBe(200);
			deserialized[2].Value.ShouldBe(300);
		}

		#endregion

		#region Edge Cases

		[Fact]
		public void Write_ObjectWithAllNullProperties_WritesEmptyObject()
		{
			// Arrange
			var model = new ModelWithNullableProperties
			{
				RequiredField = null,
				OptionalField = null,
				OptionalNumber = null
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new ModelWithNullablePropertiesConverter();

			// Act
			converter.Write(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("{}");
		}

		[Fact]
		public void Write_ArrayWithNullElements_SerializesNullElements()
		{
			// Arrange
			var models = new[]
			{
				new SimpleTestModel { Id = "elem1", Name = "First", Value = 1 },
				null,
				new SimpleTestModel { Id = "elem3", Name = "Third", Value = 3 }
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };
			var converter = new SimpleTestModelArrayConverter();

			// Act
			converter.Write(writer, models, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("null");
			json.ShouldContain("\"id\":\"elem1\"");
			json.ShouldContain("\"id\":\"elem3\"");
		}

		[Fact]
		public void Serialize_ObjectWithComplexNestedStructure_HandlesCorrectly()
		{
			// Arrange
			var model = new SimpleTestModel
			{
				Id = "nested123",
				Name = "Test with special chars: \"quotes\", \\backslash\\, \nnewline",
				Value = 999
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = new JsonSerializerOptions { TypeInfoResolver = new UnitTestingJsonSerializerContext() };

			// Act
			ZoomNetJsonConverter<SimpleTestModel>.Serialize(writer, model, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("nested123");
			json.ShouldNotContain("\"quotes\"");
			json.ShouldContain("\\u0022quotes\\u0022");
		}

		#endregion
	}
}
