using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new FundOutQuery(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            PlayerCode: "4ucmqplxb029",
            Amount: 1.0m);

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new FundOutGetService(
            fakeClient);

        _ = await service
            .GetAsync(
            query: query)
            .ConfigureAwait(false);

        _ = fakeClient
            .Received(1)
            .DepositAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.Amount),
            Arg.Any<CancellationToken>());
    }
}
