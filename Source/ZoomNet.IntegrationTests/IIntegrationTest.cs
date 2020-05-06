using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests
{
	public interface IIntegrationTest
	{
		Task RunAsync(string userId, IClient client, TextWriter log, CancellationToken cancellationToken);
	}
}
