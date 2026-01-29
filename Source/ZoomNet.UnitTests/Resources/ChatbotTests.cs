using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models.ChatbotMessage;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class ChatbotTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public ChatbotTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region DeleteMessageAsync Tests

		[Fact]
		public async Task DeleteMessageAsync()
		{
			// Arrange
			var messageId = "msg123";
			var accountId = "account123";
			var userJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.WithQueryString("account_id", accountId)
				.WithQueryString("user_jid", userJId)
				.WithQueryString("robot_jid", robotJId)
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__DELETE);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.DeleteMessageAsync(messageId, accountId, userJId, robotJId, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.MessageId.ShouldBe("201910tryyRFjM_main");
			result.RobotJId.ShouldBe("v1pky3tyBBB5pl8q@xmpp.zoom.us");
			result.ToJId.ShouldBe("xghfd@shj.zoom.us");
			result.SentTime.ShouldBe("2019-10-17 01:40:24 +0000");
		}

		[Fact]
		public async Task DeleteMessageAsync_WithSpecialCharactersInJIds()
		{
			// Arrange
			var messageId = "msg456";
			var accountId = "account456";
			var userJId = "user+test@xmpp.zoom.us";
			var robotJId = "robot-bot@xmpp.zoom.us";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.WithQueryString("account_id", accountId)
				.WithQueryString("user_jid", userJId)
				.WithQueryString("robot_jid", robotJId)
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__DELETE);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.DeleteMessageAsync(messageId, accountId, userJId, robotJId, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region SendMessageAsync Tests

		[Fact]
		public async Task SendMessageAsync_WithSimpleTextMessage()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Hello, this is a test message")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.MessageId.ShouldBe("DWQ2A82E-9220-4600-AFB2-A028852E377C");
		}

		[Fact]
		public async Task SendMessageAsync_WithMarkdownSupport()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("**Bold** and *italic* text")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, true, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithFormFields()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Message with fields"),
				Body = new List<IChatbotBody>
				{
					new ChatbotFormFields
					{
						Items = new List<ChatbotFormField>
						{
							new ChatbotFormField
							{
								Key = "Status",
								Value = "Active",
								Short = true,
								Editable = false
							},
							new ChatbotFormField
							{
								Key = "Priority",
								Value = "High",
								Short = true,
								Editable = false
							}
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithAttachments()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Message with attachment"),
				Body = new List<IChatbotBody>
				{
					new ChatbotAttachment
					{
						ResourceUrl = "https://example.com/document.pdf",
						ImageUrl = "https://example.com/thumbnail.png",
						Information = new ChatbotAttachmentInformation
						{
							Title = new ChatbotMessageText("Document Title"),
							Description = new ChatbotMessageText("Click to view the document")
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithButtons()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Choose an option"),
				Body = new List<IChatbotBody>
				{
					new ChatbotActions
					{
						Items = new List<ChatbotAction>
						{
							new ChatbotAction
							{
								Text = "Option 1",
								Value = "option1",
								Style = ChatbotActionStyle.Primary
							},
							new ChatbotAction
							{
								Text = "Option 2",
								Value = "option2",
								Style = ChatbotActionStyle.Update
							}
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithDropdownList()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Select from dropdown"),
				Body = new List<IChatbotBody>
				{
					new ChatbotDropdownList
					{
						Text = "Choose a category",
						StaticSource = ChatbotDropdownStaticSource.Channels
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithHeaderSubtitle()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader
				{
					Text = "Main Title",
					SubHeader = new ChatbotMessageText("Subtitle information")
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithSection()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Section message"),
				Body = new List<IChatbotBody>
				{
					new ChatbotSection
					{
						SidebarColor = System.Drawing.Color.Blue,
						Sections = new List<IChatbotSection>
						{
							new ChatbotMessageLine("Section content")
						},
						Footer = "Footer text",
						FooterIcon = "https://example.com/icon.png"
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region EditMessageAsync Tests

		[Fact]
		public async Task EditMessageAsync()
		{
			// Arrange
			var messageId = "msg123";
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Updated message text")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__PUT);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.EditMessageAsync(messageId, accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.MessageId.ShouldBe("201910tryyRFjM_main");
		}

		[Fact]
		public async Task EditMessageAsync_WithMarkdownSupport()
		{
			// Arrange
			var messageId = "msg456";
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("**Updated** message with *markdown*")
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__PUT);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.EditMessageAsync(messageId, accountId, toJId, robotJId, content, true, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task EditMessageAsync_WithUpdatedFields()
		{
			// Arrange
			var messageId = "msg789";
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Updated fields"),
				Body = new List<IChatbotBody>
				{
					new ChatbotFormFields
					{
						Items = new List<ChatbotFormField>
						{
							new ChatbotFormField
							{
								Key = "Status",
								Value = "Completed",
								Short = true,
								Editable = false
							}
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__PUT);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.EditMessageAsync(messageId, accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task EditMessageAsync_ChangingButtonActions()
		{
			// Arrange
			var messageId = "msg999";
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Updated actions"),
				Body = new List<IChatbotBody>
				{
					new ChatbotActions
					{
						Items = new List<ChatbotAction>
						{
							new ChatbotAction
							{
								Text = "New Action",
								Value = "new_action",
								Style = ChatbotActionStyle.Delete
							}
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("im", "chat", "messages", messageId))
				.Respond("application/json", EndpointsResource.im_chat_messages__message_id__PUT);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.EditMessageAsync(messageId, accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region Complex Content Tests

		[Fact]
		public async Task SendMessageAsync_WithComplexMixedContent()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader
				{
					Text = "Complex Message",
					SubHeader = new ChatbotMessageText("With multiple components")
				},
				Body = new List<IChatbotBody>
				{
					new ChatbotFormFields
					{
						Items = new List<ChatbotFormField>
						{
							new ChatbotFormField { Key = "Field1", Value = "Value1", Editable = false },
							new ChatbotFormField { Key = "Field2", Value = "Value2", Editable = false }
						}
					},
					new ChatbotAttachment
					{
						ResourceUrl = "https://example.com",
						ImageUrl = "https://example.com/image.png",
						Information = new ChatbotAttachmentInformation
						{
							Title = new ChatbotMessageText("Attachment"),
							Description = new ChatbotMessageText("Description")
						}
					},
					new ChatbotActions
					{
						Items = new List<ChatbotAction>
						{
							new ChatbotAction { Text = "Action", Value = "act", Style = ChatbotActionStyle.Primary }
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithMultipleActionsInSameGroup()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Multiple actions"),
				Body = new List<IChatbotBody>
				{
					new ChatbotActions
					{
						Items = new List<ChatbotAction>
						{
							new ChatbotAction { Text = "Approve", Value = "approve", Style = ChatbotActionStyle.Primary },
							new ChatbotAction { Text = "Reject", Value = "reject", Style = ChatbotActionStyle.Delete },
							new ChatbotAction { Text = "Later", Value = "later", Style = ChatbotActionStyle.Disabled }
						}
					}
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task SendMessageAsync_WithMessageLine()
		{
			// Arrange
			var accountId = "account123";
			var toJId = "user@xmpp.zoom.us";
			var robotJId = "robot@xmpp.zoom.us";
			var content = new ChatbotContent
			{
				Head = new ChatbotHeader("Message with message line"),
				Body = new List<IChatbotBody>
				{
					new ChatbotMessageLine("This is a message line")
				}
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("im", "chat", "messages"))
				.Respond("application/json", EndpointsResource.im_chat_messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chatbot = new Chatbot(client);

			// Act
			var result = await chatbot.SendMessageAsync(accountId, toJId, robotJId, content, false, TestContext.Current.CancellationToken).ConfigureAwait(true);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion
	}
}
