using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources;

public class PhoneUserExtensionTests
{
    [Theory]
    [InlineData("26000026001", 26000026001L)]
    [InlineData("999999999999999", 999999999999999L)]
    public async Task ListPhoneUsersPreservesExtensionsBeyondInt32(string extension, long expected)
    {
        using var http = new MockHttpMessageHandler();
        http.Expect(HttpMethod.Get, Utils.GetZoomApiUri("phone", "users"))
            .Respond("application/json", "{\"users\":[{\"name\":\"Staff\",\"extension_number\":" + extension + "}]}");
        var phone = new Phone(Utils.GetFluentClient(http));
        var users = await phone.ListPhoneUsersAsync(cancellationToken: TestContext.Current.CancellationToken);
        users.Records.ShouldHaveSingleItem();
        users.Records[0].ExtensionNumber.ShouldBe(expected);
        http.VerifyNoOutstandingExpectation();
    }
}
