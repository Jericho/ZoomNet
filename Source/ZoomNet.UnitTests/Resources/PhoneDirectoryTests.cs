using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources;

public class PhoneDirectoryTests
{
    [Theory]
    [InlineData("26000026001")]
    public async Task AutoReceptionistListingPreservesLargeExtensionsAndPageTokens(string extension)
    {
        using var http = new MockHttpMessageHandler();
        http.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "auto_receptionists"))
            .WithQueryString("page_size", "100").WithQueryString("next_page_token", "a/b+")
            .Respond("application/json", "{\"auto_receptionists\":[{\"id\":\"reception\",\"name\":\"Reception\",\"extension_number\":" + extension + "}],\"next_page_token\":\"next\"}");
        var phone = new Phone(Utils.GetFluentClient(http));
        var reception = await phone.ListAutoReceptionistsAsync(100, "a/b+", TestContext.Current.CancellationToken);
        reception.Records.ShouldHaveSingleItem();
        reception.Records[0].Id.ShouldBe("reception");
        reception.Records[0].Name.ShouldBe("Reception");
        reception.Records[0].ExtensionNumber.ShouldBe(26000026001L);
        reception.NextPageToken.ShouldBe("next");
        http.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task AutoReceptionistPageSizeIsValidated(int size)
    {
        using var http = new MockHttpMessageHandler();
        var phone = new Phone(Utils.GetFluentClient(http));
        await Should.ThrowAsync<ArgumentOutOfRangeException>(() => phone.ListAutoReceptionistsAsync(size, cancellationToken: TestContext.Current.CancellationToken));
        http.GetMatchCount(http.When("*")).ShouldBe(0);
    }

    [Fact]
    public async Task AutoReceptionistRequestHonorsCancellation()
    {
        using var http = new MockHttpMessageHandler();
        var phone = new Phone(Utils.GetFluentClient(http));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Should.ThrowAsync<OperationCanceledException>(() => phone.ListAutoReceptionistsAsync(cancellationToken: cancellation.Token));
    }
}
