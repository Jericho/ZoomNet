using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ZoomNet.UnitTests.Extensions
{
	public class ArgumentNullExceptionExtensionsTests
	{
#if NETFRAMEWORK || NETSTANDARD
		public class ThrowIfNull
		{
			[Fact]
			public void DoesNotThrow_WhenArgumentIsNotNull()
			{
				// Arrange
				var validObject = new object();

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNull(validObject));
			}

			[Fact]
			public void Throws_WhenArgumentIsNull()
			{
				// Arrange
				object nullObject = null;

				// Act
				void ThrowIfNullCall() => ArgumentNullException.ThrowIfNull(nullObject);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullCall)
					.ParamName.ShouldBe("nullObject");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenArgumentIsNull()
			{
				// Arrange
				object nullObject = null;
				var customMessage = "Custom error message";

				// Act
				void ThrowIfNullCall() => ArgumentNullException.ThrowIfNull(nullObject, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentNullException>(ThrowIfNullCall);
				exception.ParamName.ShouldBe("nullObject");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomParamName_WhenArgumentIsNull()
			{
				// Arrange
				object nullObject = null;
				var customParamName = "myParameter";

				// Act
				void ThrowIfNullCall() => ArgumentNullException.ThrowIfNull(nullObject, customParamName);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullCall)
					.ParamName.ShouldBe(customParamName);
			}
		}

		public class ThrowIfNullOrEmpty_String
		{
			[Fact]
			public void DoesNotThrow_WhenStringIsValid()
			{
				// Arrange
				var validString = "valid string";

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(validString));
			}

			[Fact]
			public void Throws_ArgumentNullException_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(nullString);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("nullString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsEmpty()
			{
				// Arrange
				var emptyString = string.Empty;

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyString);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("emptyString");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;
				var customMessage = "String cannot be null";

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(nullString, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentNullException>(ThrowIfNullOrEmptyCall);
				exception.ParamName.ShouldBe("nullString");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsEmpty()
			{
				// Arrange
				var emptyString = string.Empty;
				var customMessage = "String cannot be empty";

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyString, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall);
				exception.ParamName.ShouldBe("emptyString");
				exception.Message.ShouldContain(customMessage);
			}
		}

		public class ThrowIfNullOrWhiteSpace
		{
			[Fact]
			public void DoesNotThrow_WhenStringIsValid()
			{
				// Arrange
				var validString = "valid string";

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(validString));
			}

			[Fact]
			public void Throws_ArgumentNullException_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;

				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(nullString);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullOrWhiteSpaceCall)
					.ParamName.ShouldBe("nullString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsEmpty()
			{
				// Arrange
				var emptyString = string.Empty;

				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(emptyString);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrWhiteSpaceCall)
					.ParamName.ShouldBe("emptyString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsWhiteSpace()
			{
				// Arrange
				var whiteSpaceString = "   ";

				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrWhiteSpaceCall)
					.ParamName.ShouldBe("whiteSpaceString");
			}

			[Theory]
			[InlineData(" ")]
			[InlineData("  ")]
			[InlineData("\t")]
			[InlineData("\n")]
			[InlineData("\r\n")]
			[InlineData(" \t \n ")]
			public void Throws_ArgumentException_WhenStringContainsOnlyWhiteSpaceCharacters(string whiteSpaceString)
			{
				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrWhiteSpaceCall);
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;
				var customMessage = "String cannot be null";

				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(nullString, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentNullException>(ThrowIfNullOrWhiteSpaceCall);
				exception.ParamName.ShouldBe("nullString");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsWhiteSpace()
			{
				// Arrange
				var whiteSpaceString = "   ";
				var customMessage = "String cannot be whitespace";

				// Act
				void ThrowIfNullOrWhiteSpaceCall() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentException>(ThrowIfNullOrWhiteSpaceCall);
				exception.ParamName.ShouldBe("whiteSpaceString");
				exception.Message.ShouldContain(customMessage);
			}
		}
#endif

		public class ThrowIfNullOrEmpty_Collection
		{
			[Fact]
			public void DoesNotThrow_WhenCollectionHasElements()
			{
				// Arrange
				var validCollection = new List<int> { 1, 2, 3 };

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(validCollection));
			}

			[Fact]
			public void DoesNotThrow_WhenCollectionHasSingleElement()
			{
				// Arrange
				var validCollection = new List<string> { "single item" };

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(validCollection));
			}

			[Fact]
			public void Throws_ArgumentNullException_WhenCollectionIsNull()
			{
				// Arrange
				IEnumerable<int> nullCollection = null;

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("nullCollection");
			}

			[Fact]
			public void Throws_ArgumentException_WhenCollectionIsEmpty()
			{
				// Arrange
				var emptyCollection = Enumerable.Empty<int>();

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyCollection);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("emptyCollection");
			}

			[Fact]
			public void Throws_ArgumentException_WhenListIsEmpty()
			{
				// Arrange
				var emptyList = new List<string>();

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyList);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("emptyList");
			}

			[Fact]
			public void Throws_ArgumentException_WhenArrayIsEmpty()
			{
				// Arrange
				var emptyArray = Array.Empty<int>();

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyArray);

				// Assert
				Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe("emptyArray");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenCollectionIsNull()
			{
				// Arrange
				IEnumerable<int> nullCollection = null;
				var customMessage = "Collection cannot be null";

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentNullException>(ThrowIfNullOrEmptyCall);
				exception.ParamName.ShouldBe("nullCollection");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenCollectionIsEmpty()
			{
				// Arrange
				var emptyCollection = Enumerable.Empty<string>();
				var customMessage = "Collection cannot be empty";

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(emptyCollection, message: customMessage);

				// Assert
				var exception = Should.Throw<ArgumentException>(ThrowIfNullOrEmptyCall);
				exception.ParamName.ShouldBe("emptyCollection");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomParamName_WhenCollectionIsNull()
			{
				// Arrange
				IEnumerable<int> nullCollection = null;
				var customParamName = "myCollection";

				// Act
				void ThrowIfNullOrEmptyCall() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection, customParamName);

				// Assert
				Should.Throw<ArgumentNullException>(ThrowIfNullOrEmptyCall)
					.ParamName.ShouldBe(customParamName);
			}

			[Fact]
			public void WorksWithDifferentCollectionTypes()
			{
				// Arrange
				var array = new[] { 1, 2, 3 };
				var list = new List<int> { 1, 2, 3 };
				var hashSet = new HashSet<int> { 1, 2, 3 };

				// Act & Assert
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(array));
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(list));
				Should.NotThrow(() => ArgumentNullException.ThrowIfNullOrEmpty(hashSet));
			}
		}
	}
}
