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

		#region ValidateRecordPerPage Tests - Boundary Conditions

		[Fact]
		public void ValidateRecordPerPage_AtLowerBoundary_DoesNotThrow()
		{
			// Arrange
			var lowerBoundary = 1;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(lowerBoundary, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_AtUpperBoundary_DoesNotThrow()
		{
			// Arrange
			var upperBoundary = 300;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(upperBoundary, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_JustBelowLowerBoundary_ThrowsException()
		{
			// Arrange
			var justBelow = 0;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(justBelow, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_JustAboveUpperBoundary_ThrowsException()
		{
			// Arrange
			var justAbove = 301;

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(justAbove, 1, 300));
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

		[Fact]
		public void ValidateRecordPerPage_WithCustomRange_LowerBoundary_DoesNotThrow()
		{
			// Arrange
			var customMin = 25;
			var customMax = 75;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(customMin, customMin, customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomRange_UpperBoundary_DoesNotThrow()
		{
			// Arrange
			var customMin = 25;
			var customMax = 75;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(customMax, customMin, customMax));
		}

		[Fact]
		public void ValidateRecordPerPage_WithCustomRange_MiddleValue_DoesNotThrow()
		{
			// Arrange
			var customMin = 25;
			var customMax = 75;
			var middleValue = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(middleValue, customMin, customMax));
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
		public void ValidateRecordPerPage_Exception_ContainsCorrectMessage()
		{
			// Arrange
			var invalidValue = 500;
			var min = 1;
			var max = 300;

			// Act
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(invalidValue, min, max));

			// Assert
			exception.Message.ShouldContain($"Records per page must be between {min} and {max}");
			exception.ParamName.ShouldBe("recordsPerPage");
		}

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

		#region Integration-style Tests

		[Fact]
		public void ValidateRecordPerPage_SimulatingApiCall_CloudRecordings()
		{
			// Simulate a typical API call scenario for cloud recordings (max 300)
			// Arrange
			var userProvidedValue = 50;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(userProvidedValue, 1, 300));
		}

		[Fact]
		public void ValidateRecordPerPage_SimulatingApiCall_PhoneUsers()
		{
			// Simulate a typical API call scenario for phone users (max 100)
			// Arrange
			var userProvidedValue = 75;

			// Act & Assert
			Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(userProvidedValue, 1, 100));
		}

		[Fact]
		public void ValidateRecordPerPage_SimulatingApiCall_InvalidInput()
		{
			// Simulate user providing invalid input
			// Arrange
			var userProvidedValue = 0;

			// Act & Assert
			var exception = Should.Throw<ArgumentOutOfRangeException>(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(userProvidedValue, 1, 300));
			exception.Message.ShouldContain("Records per page must be between 1 and 300");
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

		[Fact]
		public void ValidateRecordPerPage_ManyDifferentValidValues_AllSucceed()
		{
			// Arrange & Act & Assert
			for (int i = 1; i <= 300; i++)
			{
				Should.NotThrow(() => ZoomNet.Utilities.Utils.ValidateRecordPerPage(i));
			}
		}

		#endregion
	}
}
