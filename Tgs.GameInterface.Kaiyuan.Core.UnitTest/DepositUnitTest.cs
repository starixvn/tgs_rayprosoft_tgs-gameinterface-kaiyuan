using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest;

public record DepositUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        Uri apiUrl = new Uri("https://wc1-api.hddv1.com/");
        var deskey = "E2F3A416006F84F8";
        var md5key = "B563DF2A3DE58DFB";
        var agentId = "72412";
        var account = "4ucmqplxb029";
        var money = 1.0m;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.Parse("2023-07-26T03:00:14.830+00:00");
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=2&account=4ucmqplxb029&money=1.0&orderid=72412202307260300148304ucmqplxb029";
        var param = "ho4lHF9xbBKSBKSI47dt9MeLG54H1Gp7fhoksdXiGpleMvtAAFzHCFIFakM5X0PuRThHCQl215so3waG6y28qwyxTQU/BUCgrpXya23E4Bs=";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "c31da9fc5268b9eedbd4e67b7c93b974";
        _ = fakeCryptography.GetHash(
            data: Arg.Is(dataMD5))
            .Returns(key);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["agent"] = agentId;
        query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
        query["param"] = param;
        query["key"] = key;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["m"] = "/channelHandle",
            ["s"] = 102,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["money"] = "2.00"
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .DepositAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            account: account,
            money: money)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("72412202307260300148304ucmqplxb029", actual);
    }
}
