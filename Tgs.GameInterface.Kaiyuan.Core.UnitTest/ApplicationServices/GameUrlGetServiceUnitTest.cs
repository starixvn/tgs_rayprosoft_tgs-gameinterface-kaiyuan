using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            BrandCode: "GPI",
            PlayerCode: "4ucmqplxb029",
            IpAddress: "127.0.0.1",
            ExternalGameId: "0");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new GameUrlGetService(
            fakeClient);

        _ = await service
            .GetAsync(
            query: query);

        _ = fakeClient
            .Received(1)
            .LoginAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.IpAddress),
            Arg.Is(query.BrandCode),
            Arg.Is(query.ExternalGameId),
            Arg.Any<CancellationToken>());
    }
}
