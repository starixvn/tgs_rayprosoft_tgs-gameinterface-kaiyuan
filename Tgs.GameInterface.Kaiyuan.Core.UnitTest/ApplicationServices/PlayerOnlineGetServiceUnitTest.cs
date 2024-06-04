using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;
using Tgs.GameInterface.Kaiyuan.Core.Models;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest.ApplicationServices;
public class PlayerOnlineGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerOnlineQuery(
			ApiUrl: new Uri("https://wc1-api.hddv1.com/"),
			Deskey: "E2F3A416006F84F8",
			MD5key: "B563DF2A3DE58DFB",
			AgentId: "72412",
			PlayerCode: "4ucmqplxb029");

		var fakeClient = Substitute.For<IGameApiClient>();

		_ = fakeClient.OnlineStatusAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.Deskey),
			Arg.Is(query.MD5key),
			Arg.Is(query.AgentId),
			Arg.Is(query.PlayerCode),
			Arg.Any<CancellationToken>())
			.Returns(true);

		var service = new PlayerOnlineGetService(
			fakeClient);

		var actual = await service.GetAsync(query: query);

		Assert.True(actual);
	}
}
