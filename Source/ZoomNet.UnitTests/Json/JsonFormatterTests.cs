using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Json
{
	public class JsonFormatterTests
	{
		#region Deserialize Tests

		[Fact]
		public void Deserialize_SimpleObject_DeserializesCorrectly()
		{
			// Arrange
			var json = @"{
				""id"": 123456789,
				""topic"": ""Test Meeting"",
				""type"": 2,
				""duration"": 60
			}";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
			var formatter = new JsonFormatter();

			// Act
			var result = formatter.Deserialize(typeof(ScheduledMeeting), stream, null, null);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledMeeting>();
			var meeting = (ScheduledMeeting)result;
			meeting.Id.ShouldBe(123456789);
			meeting.Topic.ShouldBe("Test Meeting");
			meeting.Type.ShouldBe(MeetingType.Scheduled);
			meeting.Duration.ShouldBe(60);
		}

		[Fact]
		public void Deserialize_EmptyStream_ThrowsException()
		{
			// Arrange
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act & Assert
			Should.Throw<JsonException>(() => formatter.Deserialize(typeof(ScheduledMeeting), stream, null, null));
		}

		[Fact]
		public void Deserialize_LeavesStreamOpen()
		{
			// Arrange
			var json = @"{""id"": 123, ""topic"": ""Test"", ""type"": 2}";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
			var formatter = new JsonFormatter();

			// Act
			formatter.Deserialize(typeof(ScheduledMeeting), stream, null, null);

			// Assert
			stream.CanRead.ShouldBeTrue();
			stream.Position.ShouldBeGreaterThan(0);
		}

		#endregion

		#region Serialize Tests

		[Fact]
		public void Serialize_SimpleObject_SerializesCorrectly()
		{
			// Arrange
			var meeting = new ScheduledMeeting
			{
				Id = 123456789,
				Topic = "Test Meeting",
				Type = MeetingType.Scheduled,
				Duration = 60
			};
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act
			formatter.Serialize(typeof(ScheduledMeeting), meeting, stream, null, null);
			stream.Position = 0;
			var result = new StreamReader(stream).ReadToEnd();

			// Assert
			result.ShouldNotBeNullOrEmpty();
			result.ShouldContain("\"id\":123456789");
			result.ShouldContain("\"topic\":\"Test Meeting\"");
		}

		[Fact]
		public void Serialize_WithNullProperties_OmitsNullValues()
		{
			// Arrange
			var meeting = new ScheduledMeeting
			{
				Id = 123,
				Topic = "Test",
				Type = MeetingType.Scheduled,
				Password = null,
				Uuid = null
			};
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act
			formatter.Serialize(typeof(ScheduledMeeting), meeting, stream, null, null);
			stream.Position = 0;
			var result = new StreamReader(stream).ReadToEnd();

			// Assert
			result.ShouldNotContain("\"password\"");
			result.ShouldNotContain("\"uuid\"");
		}

		#endregion

		#region CleanJsonObject Direct Tests

		[Fact]
		public void CleanJsonObject_RemovesNullProperties()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = "value1",
				["property2"] = null,
				["property3"] = "value3",
				["property4"] = null
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(2);
			jsonObject.ContainsKey("property1").ShouldBeTrue();
			jsonObject.ContainsKey("property3").ShouldBeTrue();
			jsonObject.ContainsKey("property2").ShouldBeFalse();
			jsonObject.ContainsKey("property4").ShouldBeFalse();
			jsonObject["property1"].ToString().ShouldBe("value1");
			jsonObject["property3"].ToString().ShouldBe("value3");
		}

		[Fact]
		public void CleanJsonObject_WithNestedJsonObject_RemovesNullPropertiesRecursively()
		{
			// Arrange
			var nestedObject = new System.Text.Json.Nodes.JsonObject
			{
				["nestedProp1"] = "nestedValue1",
				["nestedProp2"] = null,
				["nestedProp3"] = "nestedValue3"
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["topProp1"] = "topValue1",
				["topProp2"] = null,
				["nested"] = nestedObject,
				["topProp3"] = "topValue3"
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert - Top level
			jsonObject.Count.ShouldBe(3);
			jsonObject.ContainsKey("topProp1").ShouldBeTrue();
			jsonObject.ContainsKey("topProp3").ShouldBeTrue();
			jsonObject.ContainsKey("topProp2").ShouldBeFalse();

			// Assert - Nested level
			var nested = jsonObject["nested"].AsObject();
			nested.Count.ShouldBe(2);
			nested.ContainsKey("nestedProp1").ShouldBeTrue();
			nested.ContainsKey("nestedProp3").ShouldBeTrue();
			nested.ContainsKey("nestedProp2").ShouldBeFalse();
		}

		[Fact]
		public void CleanJsonObject_RemovesEmptyNestedJsonObjects()
		{
			// Arrange
			var emptyNested = new System.Text.Json.Nodes.JsonObject
			{
				["prop1"] = null,
				["prop2"] = null
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = "value1",
				["emptyObject"] = emptyNested,
				["property2"] = "value2"
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(2);
			jsonObject.ContainsKey("property1").ShouldBeTrue();
			jsonObject.ContainsKey("property2").ShouldBeTrue();
			jsonObject.ContainsKey("emptyObject").ShouldBeFalse();
		}

		[Fact]
		public void CleanJsonObject_PreservesZeroAndFalseValues()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["stringProp"] = "text",
				["nullString"] = null,
				["numberProp"] = 42,
				["zeroValue"] = 0,
				["boolProp"] = true,
				["falseValue"] = false,
				["emptyString"] = ""
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(6);
			jsonObject.ContainsKey("nullString").ShouldBeFalse();
			jsonObject["zeroValue"].GetValue<int>().ShouldBe(0);
			jsonObject["falseValue"].GetValue<bool>().ShouldBeFalse();
			jsonObject["emptyString"].ToString().ShouldBe("");
		}

		[Fact]
		public void CleanJsonObject_WithMultipleNestedLevels_CleansAllLevels()
		{
			// Arrange
			var level3 = new System.Text.Json.Nodes.JsonObject
			{
				["level3Keep"] = "value3",
				["level3Remove"] = null
			};

			var level2 = new System.Text.Json.Nodes.JsonObject
			{
				["level2Keep"] = "value2",
				["level2Remove"] = null,
				["level3Object"] = level3
			};

			var level1 = new System.Text.Json.Nodes.JsonObject
			{
				["level1Keep"] = "value1",
				["level1Remove"] = null,
				["level2Object"] = level2
			};

			// Act
			JsonFormatter.CleanJsonObject(level1);

			// Assert - Level 1
			level1.Count.ShouldBe(2);
			level1.ContainsKey("level1Remove").ShouldBeFalse();

			// Assert - Level 2
			var level2Result = level1["level2Object"].AsObject();
			level2Result.Count.ShouldBe(2);
			level2Result.ContainsKey("level2Remove").ShouldBeFalse();

			// Assert - Level 3
			var level3Result = level2Result["level3Object"].AsObject();
			level3Result.Count.ShouldBe(1);
			level3Result.ContainsKey("level3Remove").ShouldBeFalse();
		}

		[Fact]
		public void CleanJsonObject_WithAllNullProperties_BecomesEmpty()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = null,
				["property2"] = null,
				["property3"] = null
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(0);
		}

		[Fact]
		public void CleanJsonObject_EmptyObject_RemainsEmpty()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject();

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(0);
		}

		[Fact]
		public void CleanJsonObject_ComplexScenario_CleansCorrectly()
		{
			// Arrange
			var emptyNestedObject = new System.Text.Json.Nodes.JsonObject
			{
				["shouldBeRemoved1"] = null,
				["shouldBeRemoved2"] = null
			};

			var nonEmptyNestedObject = new System.Text.Json.Nodes.JsonObject
			{
				["keepThis"] = "important",
				["removeThis"] = null
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["topLevelString"] = "keep",
				["topLevelNull"] = null,
				["emptyNested"] = emptyNestedObject,
				["nonEmptyNested"] = nonEmptyNestedObject,
				["topLevelNumber"] = 42
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(3);
			jsonObject.ContainsKey("topLevelString").ShouldBeTrue();
			jsonObject.ContainsKey("topLevelNumber").ShouldBeTrue();
			jsonObject.ContainsKey("nonEmptyNested").ShouldBeTrue();
			jsonObject.ContainsKey("topLevelNull").ShouldBeFalse();
			jsonObject.ContainsKey("emptyNested").ShouldBeFalse();

			var nested = jsonObject["nonEmptyNested"].AsObject();
			nested.Count.ShouldBe(1);
			nested.ContainsKey("keepThis").ShouldBeTrue();
			nested.ContainsKey("removeThis").ShouldBeFalse();
		}

		[Fact]
		public void CleanJsonObject_MutatesOriginalObject()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["keep"] = "value",
				["remove"] = null
			};
			var originalCount = jsonObject.Count;

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			originalCount.ShouldBe(2);
			jsonObject.Count.ShouldBe(1);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(10)]
		public void CleanJsonObject_WithVariousNullPropertyCounts_RemovesAllNulls(int nullPropertyCount)
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject();
			for (int i = 0; i < nullPropertyCount; i++)
			{
				jsonObject[$"null{i}"] = null;
			}
			jsonObject["keepMe"] = "value";

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(1);
			jsonObject.ContainsKey("keepMe").ShouldBeTrue();
			for (int i = 0; i < nullPropertyCount; i++)
			{
				jsonObject.ContainsKey($"null{i}").ShouldBeFalse();
			}
		}

		[Fact]
		public void CleanJsonObject_WithNoNullProperties_LeavesUnchanged()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = "value1",
				["property2"] = "value2",
				["property3"] = 123
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(3);
			jsonObject["property1"].ToString().ShouldBe("value1");
			jsonObject["property2"].ToString().ShouldBe("value2");
			jsonObject["property3"].GetValue<int>().ShouldBe(123);
		}

		[Fact]
		public void CleanJsonObject_WithNonEmptyNestedObject_KeepsNestedObject()
		{
			// Arrange
			var nonEmptyNested = new System.Text.Json.Nodes.JsonObject
			{
				["nestedProp"] = "nestedValue"
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = "value1",
				["nested"] = nonEmptyNested
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			jsonObject.Count.ShouldBe(2);
			jsonObject.ContainsKey("property1").ShouldBeTrue();
			jsonObject.ContainsKey("nested").ShouldBeTrue();

			var nested = jsonObject["nested"].AsObject();
			nested.Count.ShouldBe(1);
			nested.ContainsKey("nestedProp").ShouldBeTrue();
		}

		[Fact]
		public void CleanJsonObject_StepByStep_VerifiesProcessing()
		{
			// Arrange - Test that demonstrates all 4 steps
			var deeplyNestedEmpty = new System.Text.Json.Nodes.JsonObject
			{
				["willBeRemoved"] = null
			};

			var midLevelObject = new System.Text.Json.Nodes.JsonObject
			{
				["validProp"] = "value",
				["nullProp"] = null,
				["deepNested"] = deeplyNestedEmpty
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["topLevel"] = "keep",
				["topNull"] = null,
				["midLevel"] = midLevelObject
			};

			// Act
			JsonFormatter.CleanJsonObject(jsonObject);

			// Assert
			// STEP 1: Null properties removed at all levels
			jsonObject.ContainsKey("topNull").ShouldBeFalse();

			// STEP 2: Nested objects processed recursively
			var midLevel = jsonObject["midLevel"].AsObject();
			midLevel.ContainsKey("nullProp").ShouldBeFalse();

			// STEP 3: Deeply nested objects also processed
			// The deepNested object should have been removed because it became empty
			midLevel.ContainsKey("deepNested").ShouldBeFalse();

			// STEP 4: Empty objects removed
			// midLevel is not empty (has "validProp"), so it stays
			jsonObject.ContainsKey("midLevel").ShouldBeTrue();
			midLevel.ContainsKey("validProp").ShouldBeTrue();
		}

		#endregion

		#region CleanJsonObject Integration Tests (via Serialize)

		[Fact]
		public void Serialize_JsonObject_RemovesNullProperties_Integration()
		{
			// Arrange
			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["property1"] = "value1",
				["property2"] = null,
				["property3"] = "value3"
			};
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act
			formatter.Serialize(typeof(System.Text.Json.Nodes.JsonObject), jsonObject, stream, null, null);
			stream.Position = 0;
			var result = new StreamReader(stream).ReadToEnd();

			// Assert
			result.ShouldContain("\"property1\":\"value1\"");
			result.ShouldContain("\"property3\":\"value3\"");
			result.ShouldNotContain("property2");
		}

		[Fact]
		public void Serialize_JsonObjectArray_RemovesNullProperties_Integration()
		{
			// Arrange
			var jsonObjects = new[]
			{
				new System.Text.Json.Nodes.JsonObject
				{
					["prop1"] = "value1",
					["prop2"] = null
				},
				new System.Text.Json.Nodes.JsonObject
				{
					["prop3"] = "value3",
					["prop4"] = null
				}
			};
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act
			formatter.Serialize(typeof(System.Text.Json.Nodes.JsonObject[]), jsonObjects, stream, null, null);
			stream.Position = 0;
			var result = new StreamReader(stream).ReadToEnd();

			// Assert
			result.ShouldContain("\"prop1\":\"value1\"");
			result.ShouldContain("\"prop3\":\"value3\"");
			result.ShouldNotContain("prop2");
			result.ShouldNotContain("prop4");
		}

		[Fact]
		public void Serialize_JsonObject_ComplexNesting_Integration()
		{
			// Arrange
			var level3 = new System.Text.Json.Nodes.JsonObject
			{
				["level3Value"] = "deep",
				["level3Null"] = null
			};

			var level2 = new System.Text.Json.Nodes.JsonObject
			{
				["level2Value"] = "middle",
				["level2Null"] = null,
				["level3"] = level3
			};

			var jsonObject = new System.Text.Json.Nodes.JsonObject
			{
				["level1Value"] = "top",
				["level1Null"] = null,
				["level2"] = level2
			};
			var stream = new MemoryStream();
			var formatter = new JsonFormatter();

			// Act
			formatter.Serialize(typeof(System.Text.Json.Nodes.JsonObject), jsonObject, stream, null, null);
			stream.Position = 0;
			var result = new StreamReader(stream).ReadToEnd();

			// Assert
			result.ShouldContain("\"level1Value\":\"top\"");
			result.ShouldContain("\"level2Value\":\"middle\"");
			result.ShouldContain("\"level3Value\":\"deep\"");
			result.ShouldNotContain("level1Null");
			result.ShouldNotContain("level2Null");
			result.ShouldNotContain("level3Null");
		}

		#endregion
	}
}

