namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Marketplace : IMarketplace
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Marketplace" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Marketplace(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}
	}
}
