using Shouldly;
using System;
using Xunit;

namespace ZoomNet.UnitTests.Utilities
{
	public class UtilsTests
	{
		#region MemoryStreamManager Tests

		[Fact]
		public void MemoryStreamManager_IsNotNull()
		{
			// Act
			var manager = ZoomNet.Utilities.Utils.MemoryStreamManager;

			// Assert
			manager.ShouldNotBeNull();
		}

		[Fact]
		public void MemoryStreamManager_IsSingleton()
		{
			// Act
			var manager1 = ZoomNet.Utilities.Utils.MemoryStreamManager;
			var manager2 = ZoomNet.Utilities.Utils.MemoryStreamManager;

			// Assert
			manager1.ShouldBeSameAs(manager2);
		}

		[Fact]
		public void MemoryStreamManager_CanGetStream()
		{
			// Arrange
			var manager = ZoomNet.Utilities.Utils.MemoryStreamManager;

			// Act
			using var stream = manager.GetStream();

			// Assert
			stream.ShouldNotBeNull();
			stream.Length.ShouldBe(0);
			stream.Position.ShouldBe(0);
		}

		#endregion

		#region ValidateRecordPerPage Tests - Default Parameters (1-300)

		[Fact]
		public void ValidateRecordPerPage_WithValidValue_DoesNotThrow()
		{
			// Arrange
			var validValue = 150;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(validValue));
		}

		[Fact]
		public void ValidateRecordPerPage_WithMinimumValue_DoesNotThrow()
		{
			// Arrange
			var minValue = 1;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(minValue));
		}

		[Fact]
		public void ValidateRecordPerPage_WithMaximumValue_DoesNotThrow()
		{
			// Arrange
			var maxValue = 300;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(maxValue));
		}

		[Fact]
		public void ValidateRecordPerPage_WithValueBelowMinimum_ThrowsException()
		{
			// Arrange
			var belowMin = 0;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(belowMin));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain("Records per page must be between 1 and 300");
		}

		[Fact]
		public void ValidateRecordPerPage_WithNegativeValue_ThrowsException()
		{
			// Arrange
			var negativeValue = -10;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(negativeValue));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain("Records per page must be between 1 and 300");
		}

		[Fact]
		public void ValidateRecordPerPage_WithValueAboveMaximum_ThrowsException()
		{
			// Arrange
			var aboveMax = 301;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(aboveMax));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain("Records per page must be between 1 and 300");
		}

		[Fact]
		public void ValidateRecordPerPage_WithFarAboveMaximum_ThrowsException()
		{
			// Arrange
			var farAboveMax = 1000;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(farAboveMax));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain("Records per page must be between 1 and 300");
		}

		#endregion

		#region ValidateRecordPerPage Tests - Custom Min (default 1)

		[Fact]
		public void ValidateRecordPerPage_WithCustomMin_ValidValue_DoesNotThrow()
		{
			// Arrange
			var validValue = 10;
			var customMin = 5;
			var customMax = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(validValue, customMin, customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMin_ExactMinValue_DoesNotThrow()
		{
			// Arrange
			var customMin = 10;
			var customMax = 100;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(customMin, customMin, customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMin_ExactMaxValue_DoesNotThrow()
		{
			// Arrange
			var customMin = 10;
			var customMax = 100;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(customMax, customMin, customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMin_BelowMinimum_ThrowsException()
		{
			// Arrange
			var customMin = 10;
			var customMax = 100;
			var belowMin = 5;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(belowMin, customMin, customMax));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain($"Records per page must be between {customMin} and {customMax}");
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMin_AboveMaximum_ThrowsException()
		{
			// Arrange
			var customMin = 10;
			var customMax = 100;
			var aboveMax = 150;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(aboveMax, customMin, customMax));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain($"Records per page must be between {customMin} and {customMax}");
		}

		#endregion

		#region ValidateRecordPerPage Tests - Custom Max (default 300)

		[Fact]
		public void ValidateRecordPerPage_WithCustomMax_ValidValue_DoesNotThrow()
		{
			// Arrange
			var validValue = 50;
			var customMax = 100;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(validValue, max: customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMax_ExactMaxValue_DoesNotThrow()
		{
			// Arrange
			var customMax = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(customMax, max: customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomMax_AboveMaximum_ThrowsException()
		{
			// Arrange
			var customMax = 50;
			var aboveMax = 51;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(aboveMax, max: customMax));
			exception.ParamName.ShouldBe("recordsPerPage");
			exception.Message.ShouldContain($"Records per page must be between 1 and {customMax}");
		}

		#endregion

		#region ValidateRecordPerPage Tests - Real World Scenarios

		[Fact]
		public void ValidateRecordPerPage_CloudRecordings_Max300_Valid()
		{
			// Most endpoints use max 300
			// Arrange
			var value = 100;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_CloudRecordings_Max300_Invalid()
		{
			// Arrange
			var value = 500;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_Sms_Max100_Valid()
		{
			// SMS endpoint uses max 100
			// Arrange
			var value = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 100));
		}

		[Fact]
		public void ValidateRecordPerPage_Sms_Max100_Invalid()
		{
			// Arrange
			var value = 101;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 100));
		}

		[Fact]
		public void ValidateRecordPerPage_PhoneUsers_Max100_Valid()
		{
			// Phone users endpoint uses max 100
			// Arrange
			var value = 100;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 100));
		}

		[Fact]
		public void ValidateRecordPerPage_QoS_Max10_Valid()
		{
			// QoS endpoints use max 10
			// Arrange
			var value = 5;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 10));
		}

		[Fact]
		public void ValidateRecordPerPage_QoS_Max10_Invalid()
		{
			// Arrange
			var value = 11;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 10));
		}

		#endregion

		#region ValidateRecordPerPage Tests - Edge Cases

		[Fact]
		public void ValidateRecordPerPage_WithMinEqualsMax_ValidValue_DoesNotThrow()
		{
			// Arrange
			var value = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 50, 50));
		}

		[Fact]
		public void ValidateRecordPerPage_WithMinEqualsMax_InvalidValue_ThrowsException()
		{
			// Arrange
			var value = 51;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 50, 50));
		}

		[Fact]
		public void ValidateRecordPerPage_WithVeryLargeRange_ValidValue_DoesNotThrow()
		{
			// Arrange
			var value = 5000;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 10000));
		}

		[Fact]
		public void ValidateRecordPerPage_WithVeryLargeRange_InvalidValue_ThrowsException()
		{
			// Arrange
			var value = 15000;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 10000));
		}

		#endregion

		#region ValidateRecordPerPage Tests - Multiple Validations

		[Fact]
		public void ValidateRecordPerPage_MultipleValidations_WithSameValue_DoesNotThrow()
		{
			// Arrange
			var value = 30;

			// Act & Assert
			Should.NotThrow(() =>
			{
				ZoomNet.Utilities.Utils.ValidateRecordPerPage(value);
				ZoomNet.Utilities.Utils.ValidateRecordPerPage(value);
				ZoomNet.Utilities.Utils.ValidateRecordPerPage(value);
			});
		}

		[Fact]
		public void ValidateRecordPerPage_MultipleValidations_WithDifferentRanges_ThrowsAppropriately()
		{
			// Arrange
			var value = 150;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 300));
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 200));
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, 1, 100));
		}

		#endregion

		#region ValidateRecordPerPage Tests - Exception Message Verification

		[Fact]
		public void ValidateRecordPerPage_Exception_WithCustomRange_ContainsCorrectMessage()
		{
			// Arrange
			var invalidValue = 200;
			var min = 10;
			var max = 100;

			// Act
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(invalidValue, min, max));

			// Assert
			exception.Message.ShouldContain($"Records per page must be between {min} and {max}");
			exception.ParamName.ShouldBe("recordsPerPage");
		}

		#endregion

		#region ValidateRecordPerPage Tests - Common API Limits

		[Theory]
		[InlineData(1, true)]
		[InlineData(30, true)]
		[InlineData(100, true)]
		[InlineData(300, true)]
		[InlineData(0, false)]
		[InlineData(-1, false)]
		[InlineData(301, false)]
		[InlineData(500, false)]
		public void ValidateRecordPerPage_WithCommonValues_BehavesAsExpected(int value, bool shouldSucceed)
		{
			// Act & Assert
			if (shouldSucceed)
			{
				Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value));
			}
			else
			{
				Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value));
			}
		}

		[Theory]
		[InlineData(1, 1, 100, true)]
		[InlineData(50, 1, 100, true)]
		[InlineData(100, 1, 100, true)]
		[InlineData(0, 1, 100, false)]
		[InlineData(101, 1, 100, false)]
		public void ValidateRecordPerPage_WithCustomRange_BehavesAsExpected(int value, int min, int max, bool shouldSucceed)
		{
			// Act & Assert
			if (shouldSucceed)
			{
				Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, min, max));
			}
			else
			{
				Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(value, min, max));
			}
		}

		#endregion

		#region Performance/Stress Tests

		[Fact]
		public void ValidateRecordPerPage_RepeatedCalls_PerformsConsistently()
		{
			// Arrange
			var validValue = 150;
			var iterations = 10000;

			// Act & Assert
			Should.NotThrow(() =>
			{
				for (int i = 0; i < iterations; i++)
				{
					ZoomNet.Utilities.Utils.ValidateRecordPerPage(validValue);
				}
			});
		}

		#endregion

		#region EncodeUUID Tests - Null and Empty Values

		[Fact]
		public void EncodeUUID_WithNull_ReturnsNull()
		{
			// Arrange
			string uuid = null;

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBeNull();
		}

		[Fact]
		public void EncodeUUID_WithEmptyString_ReturnsEmptyString()
		{
			// Arrange
			var uuid = string.Empty;

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(string.Empty);
		}

		#endregion

		#region EncodeUUID Tests - Values Without Forward Slash

		[Fact]
		public void EncodeUUID_WithoutForwardSlash_ReturnsUnchanged()
		{
			// Arrange
			var uuid = "abc123-def456";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		[Fact]
		public void EncodeUUID_WithStandardUUID_ReturnsUnchanged()
		{
			// Arrange
			var uuid = "550e8400-e29b-41d4-a716-446655440000";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		[Fact]
		public void EncodeUUID_WithAlphanumeric_ReturnsUnchanged()
		{
			// Arrange
			var uuid = "Meeting123";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		[Fact]
		public void EncodeUUID_WithSpecialCharactersButNoSlash_ReturnsUnchanged()
		{
			// Arrange
			var uuid = "uuid-with-dashes_and_underscores";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		#endregion

		#region EncodeUUID Tests - Values With Forward Slash

		[Fact]
		public void EncodeUUID_WithForwardSlash_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc/123";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldNotBe(uuid);
			result.ShouldBe("abc%252f123");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAtStart_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "/abc123";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe("%252fabc123");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAtEnd_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc123/";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe("abc123%252f");
		}

		[Fact]
		public void EncodeUUID_WithMultipleForwardSlashes_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc/def/ghi";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe("abc%252fdef%252fghi");
		}

		[Fact]
		public void EncodeUUID_WithConsecutiveForwardSlashes_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc//def";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe("abc%252f%252fdef");
		}

		#endregion

		#region EncodeUUID Tests - Mixed Content

		[Fact]
		public void EncodeUUID_WithForwardSlashAndOtherSpecialChars_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc/def-ghi_jkl";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.ShouldBe("abc%252fdef-ghi_jkl");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAndSpaces_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "abc 123/def 456";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.ShouldContain("%2b");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAndQueryParams_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "meetingId/123?param=value";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
		}

		#endregion

		#region EncodeUUID Tests - Real-World Scenarios

		[Fact]
		public void EncodeUUID_WithZoomMeetingId_NoSlash_ReturnsUnchanged()
		{
			// Typical Zoom meeting ID without slash
			// Arrange
			var uuid = "85641287426";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		[Fact]
		public void EncodeUUID_WithZoomMeetingUUID_WithSlash_ReturnsDoubleEncoded()
		{
			// Some Zoom UUIDs contain forward slashes
			// Arrange
			var uuid = "M2IzNTY5/6DTP8aVIZ5ASVQ==";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.ShouldNotContain("/");
		}

		[Fact]
		public void EncodeUUID_WithBase64LikeUUID_WithSlash_ReturnsDoubleEncoded()
		{
			// Base64-encoded values might contain forward slashes
			// Arrange
			var uuid = "dGVzdC9kYXRh/encoded==";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
		}

		#endregion

		#region EncodeUUID Tests - Boundary Cases

		[Fact]
		public void EncodeUUID_WithOnlyForwardSlash_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "/";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe("%252f");
		}

		[Fact]
		public void EncodeUUID_WithVeryLongStringWithSlash_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = new string('a', 100) + "/" + new string('b', 100);

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.Length.ShouldBeGreaterThan(uuid.Length);
		}

		[Fact]
		public void EncodeUUID_WithWhitespace_NoSlash_ReturnsUnchanged()
		{
			// Arrange
			var uuid = "   ";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldBe(uuid);
		}

		[Fact]
		public void EncodeUUID_WithWhitespaceAndSlash_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = " / ";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
		}

		#endregion

		#region EncodeUUID Tests - Encoding Verification

		[Fact]
		public void EncodeUUID_WithForwardSlash_ResultIsDoubleEncoded()
		{
			// Verify that double encoding produces %252F (encoded %2F)
			// Arrange
			var uuid = "test/value";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			// Single encoding would produce %2F
			// Double encoding produces %252F (the % itself is encoded as %25)
			result.ShouldBe("test%252fvalue");
			result.ShouldNotContain("%2F");
			result.ShouldNotContain("/");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlash_VerifiesDoubleEncoding()
		{
			// Verify that the encoding produces the correct double-encoded format
			// Arrange
			var uuid = "meeting/123";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			// Double encoding: / -> %2F -> %252F
			result.ShouldBe("meeting%252f123");
			result.ShouldContain("%252f");
			result.ShouldNotContain("/");
		}

		#endregion

		#region EncodeUUID Tests - Multiple Calls

		[Fact]
		public void EncodeUUID_CalledMultipleTimes_WithSameValue_ReturnsConsistentResult()
		{
			// Arrange
			var uuid = "test/uuid";

			// Act
			var result1 = ZoomNet.Utilities.Utils.EncodeUUID(uuid);
			var result2 = ZoomNet.Utilities.Utils.EncodeUUID(uuid);
			var result3 = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result1.ShouldBe(result2);
			result2.ShouldBe(result3);
		}

		[Fact]
		public void EncodeUUID_WithDifferentValues_ReturnsDistinctResults()
		{
			// Arrange
			var uuid1 = "test/uuid1";
			var uuid2 = "test/uuid2";

			// Act
			var result1 = ZoomNet.Utilities.Utils.EncodeUUID(uuid1);
			var result2 = ZoomNet.Utilities.Utils.EncodeUUID(uuid2);

			// Assert
			result1.ShouldNotBe(result2);
		}

		#endregion

		#region EncodeUUID Tests - Edge Cases with Other URL-Unsafe Characters

		[Fact]
		public void EncodeUUID_WithForwardSlashAndAmpersand_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "test/value&param";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.ShouldContain("%2526");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAndPercent_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "test/100%";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
			result.ShouldContain("%2525");
		}

		[Fact]
		public void EncodeUUID_WithForwardSlashAndPlus_ReturnsDoubleEncoded()
		{
			// Arrange
			var uuid = "test/uuid+value";

			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);

			// Assert
			result.ShouldContain("%252f");
		}

		#endregion

		#region EncodeUUID Tests - Theory-Based Tests

		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData("simple", "simple")]
		[InlineData("uuid-123", "uuid-123")]
		[InlineData("UUID_456", "UUID_456")]
		public void EncodeUUID_WithoutSlash_ReturnsExpectedValue(string input, string expected)
		{
			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(input);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData("a/b", "a%252fb")]
		[InlineData("/test", "%252ftest")]
		[InlineData("test/", "test%252f")]
		[InlineData("a/b/c", "a%252fb%252fc")]
		[InlineData("//", "%252f%252f")]
		public void EncodeUUID_WithSlash_ReturnsDoubleEncodedValue(string input, string expected)
		{
			// Act
			var result = ZoomNet.Utilities.Utils.EncodeUUID(input);

			// Assert
			result.ShouldBe(expected);
		}

		#endregion

		#region EncodeUUID Tests - Performance

		[Fact]
		public void EncodeUUID_RepeatedCalls_PerformsConsistently()
		{
			// Arrange
			var uuid = "test/uuid";
			var iterations = 10000;

			// Act & Assert
			Should.NotThrow(() =>
			{
				for (int i = 0; i < iterations; i++)
				{
					var result = ZoomNet.Utilities.Utils.EncodeUUID(uuid);
				}
			});
		}

		#endregion
	}
}
