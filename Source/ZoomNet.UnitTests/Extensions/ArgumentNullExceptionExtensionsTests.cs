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

				// Act & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(nullObject))
					.ParamName.ShouldBe("nullObject");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenArgumentIsNull()
			{
				// Arrange
				object nullObject = null;
				var customMessage = "Custom error message";

				// Act
				var exception = Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(nullObject, message: customMessage));

				// Assert
				exception.ParamName.ShouldBe("nullObject");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomParamName_WhenArgumentIsNull()
			{
				// Arrange
				object nullObject = null;
				var customParamName = "myParameter";

				// Act & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(nullObject, customParamName));
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

				// Act & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrEmpty(nullString))
					.ParamName.ShouldBe("nullString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsEmpty()
			{
				// Arrange
				var emptyString = string.Empty;

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyString))
					.ParamName.ShouldBe("emptyString");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;
				var customMessage = "String cannot be null";

				// Act
				var exception = Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrEmpty(nullString, message: customMessage));

				// Assert
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
				var exception = Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyString, message: customMessage));

				// Assert
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

				// Assert & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(nullString))
					.ParamName.ShouldBe("nullString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsEmpty()
			{
				// Arrange
				var emptyString = string.Empty;

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(emptyString))
					.ParamName.ShouldBe("emptyString");
			}

			[Fact]
			public void Throws_ArgumentException_WhenStringIsWhiteSpace()
			{
				// Arrange
				var whiteSpaceString = "   ";

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString))
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
				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString));
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenStringIsNull()
			{
				// Arrange
				string nullString = null;
				var customMessage = "String cannot be null";

				// Act
				var exception = Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(nullString, message: customMessage));

				// Assert
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
				var exception = Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrWhiteSpace(whiteSpaceString, message: customMessage));

				// Assert
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

				// Act & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection))
					.ParamName.ShouldBe("nullCollection");
			}

			[Fact]
			public void Throws_ArgumentException_WhenCollectionIsEmpty()
			{
				// Arrange
				var emptyCollection = Enumerable.Empty<int>();

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyCollection))
					.ParamName.ShouldBe("emptyCollection");
			}

			[Fact]
			public void Throws_ArgumentException_WhenListIsEmpty()
			{
				// Arrange
				var emptyList = new List<string>();

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyList))
					.ParamName.ShouldBe("emptyList");
			}

			[Fact]
			public void Throws_ArgumentException_WhenArrayIsEmpty()
			{
				// Arrange
				var emptyArray = Array.Empty<int>();

				// Act & Assert
				Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyArray))
					.ParamName.ShouldBe("emptyArray");
			}

			[Fact]
			public void Throws_WithCustomMessage_WhenCollectionIsNull()
			{
				// Arrange
				IEnumerable<int> nullCollection = null;
				var customMessage = "Collection cannot be null";

				// Act
				var exception = Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection, message: customMessage));

				// Assert
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
				var exception = Should.Throw<ArgumentException>(() => ArgumentNullException.ThrowIfNullOrEmpty(emptyCollection, message: customMessage));

				// Assert
				exception.ParamName.ShouldBe("emptyCollection");
				exception.Message.ShouldContain(customMessage);
			}

			[Fact]
			public void Throws_WithCustomParamName_WhenCollectionIsNull()
			{
				// Arrange
				IEnumerable<int> nullCollection = null;
				var customParamName = "myCollection";

				// Act & Assert
				Should.Throw<ArgumentNullException>(() => ArgumentNullException.ThrowIfNullOrEmpty(nullCollection, customParamName))
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
