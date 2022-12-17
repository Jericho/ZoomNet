using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	[JsonSerializable(typeof(System.Text.Json.Nodes.JsonObject))]
	[JsonSerializable(typeof(System.Text.Json.Nodes.JsonObject[]))]

	[JsonSerializable(typeof(ZoomNet.App.AppContext))]
	internal partial class ZoomNetAppJsonSerializerContext : JsonSerializerContext
	{
	}
}
