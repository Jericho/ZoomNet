using System.Text.Json.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A part of the message body.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ChatbotActions), "actions")]
[JsonDerivedType(typeof(ChatbotAttachment), "attachments")]
[JsonDerivedType(typeof(ChatbotDropdownList), "select")]
[JsonDerivedType(typeof(ChatbotFormFields), "fields")]
[JsonDerivedType(typeof(ChatbotMessageLine), "message")]
[JsonDerivedType(typeof(ChatbotSection), "section")]
public interface IChatbotBody
{
}
