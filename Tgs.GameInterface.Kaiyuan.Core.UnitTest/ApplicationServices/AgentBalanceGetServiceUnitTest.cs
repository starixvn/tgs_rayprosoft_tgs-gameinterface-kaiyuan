using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;

public class AgentBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_ProcessAsync()
	{
		var query = new AgentBalanceQuery(
			ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
			Deskey: "E2F3A416006F84F8",
			MD5key: "B563DF2A3DE58DFB",
			AgentId: "72412");

		var fakeClient = Substitute.For<IGameApiClient>();

		var credit = 10000000.00m;
		_ = fakeClient
			.AgentBalanceAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.Deskey),
			Arg.Is(query.MD5key),
			Arg.Is(query.AgentId),
			Arg.Any<CancellationToken>())
			.Returns(credit);

		var service = new AgentBalanceGetService(
			fakeClient);

		var actual = await service.GetAsync(
			query: query);

		Assert.NotNull(actual);
		Assert.Equal(10000000.00m, actual.Credit);
	}
}
