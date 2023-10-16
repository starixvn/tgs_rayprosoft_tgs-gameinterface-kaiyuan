using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest;

public class LoginUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var apiUrl = new Uri("https://wc1-api.hddv1.com/");
        var deskey = "E2F3A416006F84F8";
        var md5key = "B563DF2A3DE58DFB";
        var agentId = "72412";
        var account = "4ucmqplxb029";
        var ipAddress = "127.0.0.1";
        var lineCode = "GPI";
        var kindId = "0";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.Parse("2023-07-26T03:09:20.117+00:00");
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=0&account=4ucmqplxb029&money=0&orderid=72412202307260309201174ucmqplxb029&ip=127.0.0.1&lineCode=GPI&KindID=0";
        var param = "AN/kR9Tl/hgbe5UMClo83jPXBmho0mIUfC6GszYjs2H0ivBWXSRAVMPHW6206xIKykfufyQrf55SqtNh32TsilT7ne+yxFaQuFLdaURgJ5Op0nsjfy9JyrM5VLFeUJMbM2xicRz+TSoPz1x8Y5myMw==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "ccc6e8f38adaa92f3323f320ebe0781b";
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
            ["s"] = 100,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["url"] = "https://wc1-g-h5.uaphl791.com/",
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
            .LoginAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            account: account,
            ipAddress: ipAddress,
            lineCode: lineCode,
            kindId: kindId);

        Assert.NotNull(actual);
        Assert.Equal("https://wc1-g-h5.uaphl791.com/", actual);
    }
}
