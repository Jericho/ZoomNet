using System;
using System.Collections.Generic;
using Xunit;
using ZoomNet.Models.ChatbotMessage;

namespace ZoomNet.UnitTests.Resources
{
	public class ChatbotValidateTests
	{
		[Fact]
		public void ChatbotMessageLine_Validate_ThrowsForMarkdownAndLinks()
		{
			var token = new ChatbotMessageLine("abc")
			{
				Link = "http://link"
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Link property cannot be used with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotSectionNested_LineValidate_ThrowsForMarkdownAndLinks()
		{
			var token = new ChatbotSection()
			{
				Sections = new List<IChatbotSection>()
				{
					new ChatbotMessageLine("abc")
					{
						Link = "http://link"
					}
				}
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Link property cannot be used with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotFormField_Validate_ThrowsForMarkdownAndEditable()
		{
			var token = new ChatbotFormField()
			{
				Editable = true
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Editable property cannot be used on Form Field with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotSectionNested_FormFieldValidate_ThrowsForMarkdownAndEditable()
		{
			var token = new ChatbotSection()
			{
				Sections = new List<IChatbotSection>()
				{
					new ChatbotFormFields()
					{
						Items = new List<ChatbotFormField>()
						{
							new ChatbotFormField()
							{
								Editable = true
							}
						}
					}
				}
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Editable property cannot be used on Form Field with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotContentNested_FormFieldValidate_ThrowsForMarkdownAndEditable()
		{
			var token = new ChatbotContent()
			{
				Body = new List<IChatbotBody>()
				{
					new ChatbotFormFields()
					{
						Items = new List<ChatbotFormField>()
						{
							new ChatbotFormField()
							{
								Editable = true
							}
						}
					}
				}
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Editable property cannot be used on Form Field with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotContentNested_LineValidate_ThrowsForMarkdownAndLink()
		{
			var token = new ChatbotContent()
			{
				Body = new List<IChatbotBody>()
				{
					new ChatbotMessageLine("abc")
					{
						Link = "http://link"
					}
				}
			};

			token.Validate(false);
			var ex = Assert.Throws<InvalidOperationException>(() => token.Validate(true));

			Assert.Equal("Link property cannot be used with EnableMarkdownSupport", ex.Message);
		}

		[Fact]
		public void ChatbotContent_BodyNull_DoesNotThrow()
		{
			var token = new ChatbotContent()
			{
				Body = null
			};

			token.Validate(false);
			token.Validate(true);
		}
	}
}
