using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;
using Tgs.GameInterface.Kaiyuan.Core.Models;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            PlayerCode: "4ucmqplxb029");

        var fakeClient = Substitute.For<IGameApiClient>();

        var info = new BalanceInfo(
            TotalMoney: 100,
            FreeMoney: 98);
        _ = fakeClient
            .BalanceAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Any<CancellationToken>())
            .Returns(info);

        var service = new PlayerBalanceGetService(
            fakeClient);

        var actual = await service
            .GetAsync(
            query: query);

        Assert.NotNull(actual);
        Assert.Equal(100, actual.Balance);
        Assert.Equal(98, actual.FreeBalance);
        Assert.Equal(2, actual.OutstandingBalance);
    }
}
