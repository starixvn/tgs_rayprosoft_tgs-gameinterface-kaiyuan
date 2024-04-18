using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;
using Tgs.GameInterface.Kaiyuan.Core.Models;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
			Deskey: "E2F3A416006F84F8",
			MD5key: "B563DF2A3DE58DFB",
			AgentId: "72412",
			PlayerCode: "4ucmqplxb029");

		var fakeClient = Substitute.For<IGameApiClient>();

		var info = new BalanceInfo(
			IsPlayerExist: true,
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
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100, actual.Balance);
		Assert.Equal(98, actual.FreeBalance);
		Assert.Equal(2, actual.OutstandingBalance);
	}

	[Fact]
	public async Task Player_does_not_exist()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
			Deskey: "E2F3A416006F84F8",
			MD5key: "B563DF2A3DE58DFB",
			AgentId: "72412",
			PlayerCode: "4ucmqplxb029");

		var fakeClient = Substitute.For<IGameApiClient>();

		var info = new BalanceInfo(
			IsPlayerExist: false,
			TotalMoney: 0.0m,
			FreeMoney: 0.0m);
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
		Assert.False(actual.IsPlayerExist);
		Assert.Equal(0.0m, actual.Balance);
		Assert.Equal(0.0m, actual.FreeBalance);
		Assert.Equal(0.0m, actual.OutstandingBalance);
	}
}
