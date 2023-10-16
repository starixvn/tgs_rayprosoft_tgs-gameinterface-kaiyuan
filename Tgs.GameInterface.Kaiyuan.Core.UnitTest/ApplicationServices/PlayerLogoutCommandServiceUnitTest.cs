using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var command = new PlayerLogoutCommand(
            ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
            Deskey: "E2F3A416006F84F8",
            MD5key: "B563DF2A3DE58DFB",
            AgentId: "72412",
            PlayerCode: "4ucmqplxb029");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new PlayerLogoutCommandService(
            fakeClient);

        await service
            .ExecuteAsync(
            command: command);

        _ = fakeClient
            .Received(1)
            .LogoutAsync(
            Arg.Is(command.ApiUrl),
            Arg.Is(command.Deskey),
            Arg.Is(command.MD5key),
            Arg.Is(command.AgentId),
            Arg.Is(command.PlayerCode),
            Arg.Any<CancellationToken>());
    }
}
