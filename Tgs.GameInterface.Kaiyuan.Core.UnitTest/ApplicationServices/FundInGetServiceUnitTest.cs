using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;
using Tgs.GameInterface.Kaiyuan.Core.Models;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new FundInQuery(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            PlayerCode: "4ucmqplxb029",
            OrderId: "72412202310060331234064ucmqplxb029",
            Amount: 1.0m);

        var fakeClient = Substitute.For<IGameApiClient>();

        _ = fakeClient
            .WithdrawAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.OrderId),
            Arg.Is(query.Amount),
            Arg.Any<CancellationToken>())
            .Returns(new ValueTask<WithdrawResp>(
                new WithdrawResp(Resp: "{\"m\":\"/channelHandle\",\"s\":103,\"d\":{\"code\":0,\"money\":\"0.00\"}}")));

        var service = new FundInGetService(
            fakeClient);

        var actual = await service.GetAsync(query: query);

        Assert.Equal("{\"m\":\"/channelHandle\",\"s\":103,\"d\":{\"code\":0,\"money\":\"0.00\"}}", actual.Resp);
    }
}
