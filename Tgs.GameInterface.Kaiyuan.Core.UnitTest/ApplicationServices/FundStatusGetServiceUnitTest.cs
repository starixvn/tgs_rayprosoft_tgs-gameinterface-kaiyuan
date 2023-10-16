using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            OrderId: "72412202307260300148304ucmqplxb029");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new FundStatusGetService(
            fakeClient);

        _ = await service
            .GetAsync(
            query: query);

        _ = fakeClient
            .Received(1)
            .OrderStatusAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.OrderId),
            Arg.Any<CancellationToken>());
    }
}
