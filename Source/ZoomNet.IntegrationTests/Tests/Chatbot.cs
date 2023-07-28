using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.ChatbotMessage;

namespace ZoomNet.IntegrationTests.Tests;

public class Chatbot : IIntegrationTest
{
	/// <inheritdoc />
	public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log,
		CancellationToken cancellationToken)
	{
		var accountId = myUser?.AccountId ?? "{accountId}";
		var robotJId = "{robotId}@xmpp.zoom.us";
		var toJId = "{userId}@xmpp.zoom.us"; // User
		//var toJId = "{channelId}@conference.xmpp.zoom.us"; // Channel
		var response = await client.Chatbot.SendMessageAsync(accountId, toJId, robotJId, "Test message", false, cancellationToken);
		await log.WriteLineAsync(response.MessageId);
		await Task.Delay(1000, cancellationToken);
		response = await client.Chatbot.EditMessageAsync(response.MessageId, accountId, toJId, robotJId, "*Updated test message*", true, cancellationToken);
		await Task.Delay(1000, cancellationToken);
		response = await client.Chatbot.DeleteMessageAsync(response.MessageId, accountId, toJId, robotJId, cancellationToken);
		await Task.Delay(1000, cancellationToken);
		response = await client.Chatbot.SendMessageAsync(accountId, toJId, robotJId,
			new ChatbotContent()
			{
				Head = new ChatbotHeader("A message header"),
				Body = new List<IChatbotBody>()
				{
					new ChatbotSection()
					{
						SidebarColor = Color.Red,
						Sections = new List<IChatbotSection>()
						{
							new ChatbotMessageLine("Section 1 Message")
						},
						Footer = "I am a footer",
						FooterIcon = "https://d24cgw3uvb9a9h.cloudfront.net/static/93516/image/new/ZoomLogo.png",
						TimeStampFromDateTime = DateTime.Now.AddSeconds(120)
					},
					new ChatbotSection()
					{
						SidebarColor = Color.Green,
						Sections = new List<IChatbotSection>()
						{
							new ChatbotMessageLine("Section 2 Message")
						}
					},
					new ChatbotAttachment()
					{
						ResourceUrl = "https://zoom.us",
						ImageUrl = "https://d24cgw3uvb9a9h.cloudfront.net/static/93516/image/new/ZoomLogo.png",
						Information = new ChatbotAttachmentInformation()
						{
							Title = new ChatbotMessageText("Text"),
							Description = new ChatbotMessageText("Description")
						}
					},
					new ChatbotMessageLine("Non-section Message"),
					new ChatbotDropdownList()
					{
						Text = "Channels: ",
						StaticSource = ChatbotDropdownStaticSource.Channels,
					},
					new ChatbotDropdownList()
					{
						Text = "Members: ",
						StaticSource = ChatbotDropdownStaticSource.Members,
					},
					new ChatbotFormFields()
					{
						Items = new List<ChatbotFormField>()
						{
							new ChatbotFormField()
							{
								Key = "field1",
								Value = " ",
								Editable = false,
								Short = true
							},
							new ChatbotFormField()
							{
								Key = "field2",
								Value = "Test",
								Editable = false,
								Short = true
							}
						}
					},
					new ChatbotActions()
					{
						Items = new List<ChatbotAction>()
						{
							new ChatbotAction()
							{
								Text = "Button 1",
								Value = "button1",
								Style = ChatbotActionStyle.Primary
							},
							new ChatbotAction()
							{
								Text = "Button 2",
								Value = "button2",
								Style = ChatbotActionStyle.Update
							},
							new ChatbotAction()
							{
								Text = "Button 3",
								Value = "button3",
								Style = ChatbotActionStyle.Delete
							},
							new ChatbotAction()
							{
								Text = "Button 4",
								Value = "button4",
								Style = ChatbotActionStyle.Disabled
							}
						}
					}
				}
			}, true, cancellationToken);
		await log.WriteLineAsync(response.MessageId);
		await Task.Delay(1000, cancellationToken);
		response = await client.Chatbot.EditMessageAsync(response.MessageId, accountId, toJId, robotJId,
			new ChatbotContent()
			{
				Head = new ChatbotHeader("A message header")
				{
					SubHeader = new ChatbotMessageText("Added sub-header.")
				},
				Body = new List<IChatbotBody>()
				{
					new ChatbotSection()
					{
						SidebarColor = Color.Red,
						Sections = new List<IChatbotSection>()
						{
							new ChatbotMessageLine("Section 1 Message")
						},
						Footer = "I am a footer",
						FooterIcon = "https://d24cgw3uvb9a9h.cloudfront.net/static/93516/image/new/ZoomLogo.png",
						TimeStampFromDateTime = DateTime.Now.AddSeconds(120)
					},
					new ChatbotSection()
					{
						SidebarColor = Color.Green,
						Sections = new List<IChatbotSection>()
						{
							new ChatbotMessageLine("Section 2 Message")
						}
					},
					new ChatbotAttachment()
					{
						ResourceUrl = "https://zoom.us",
						ImageUrl = "https://d24cgw3uvb9a9h.cloudfront.net/static/93516/image/new/ZoomLogo.png",
						Information = new ChatbotAttachmentInformation()
						{
							Title = new ChatbotMessageText("Text"),
							Description = new ChatbotMessageText("Description")
						}
					},
					new ChatbotMessageLine("Non-section Message"),
					new ChatbotDropdownList()
					{
						Text = "Channels: ",
						StaticSource = ChatbotDropdownStaticSource.Channels,
					},
					new ChatbotDropdownList()
					{
						Text = "Members: ",
						StaticSource = ChatbotDropdownStaticSource.Members,
					},
					new ChatbotFormFields()
					{
						Items = new List<ChatbotFormField>()
						{
							new ChatbotFormField()
							{
								Key = "field1",
								Value = " ",
								Editable = false,
								Short = true
							},
							new ChatbotFormField()
							{
								Key = "field2",
								Value = "Test",
								Editable = false,
								Short = true
							}
						}
					},
					new ChatbotActions()
					{
						Items = new List<ChatbotAction>()
						{
							new ChatbotAction()
							{
								Text = "Button 1",
								Value = "button1",
								Style = ChatbotActionStyle.Primary
							},
							new ChatbotAction()
							{
								Text = "Button 2",
								Value = "button2",
								Style = ChatbotActionStyle.Update
							},
							new ChatbotAction()
							{
								Text = "Button 3",
								Value = "button3",
								Style = ChatbotActionStyle.Delete
							},
							new ChatbotAction()
							{
								Text = "Button 4",
								Value = "button4",
								Style = ChatbotActionStyle.Disabled
							}
						}
					}
				}
			}, true, cancellationToken);
		await Task.Delay(1000, cancellationToken);
		response = await client.Chatbot.DeleteMessageAsync(response.MessageId, accountId, toJId, robotJId,
			cancellationToken);
	}
}
