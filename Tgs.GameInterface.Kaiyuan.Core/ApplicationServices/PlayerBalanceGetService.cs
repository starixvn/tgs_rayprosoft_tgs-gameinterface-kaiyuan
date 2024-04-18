using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public class PlayerBalanceGetService(
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(
		PlayerBalanceQuery query,
		CancellationToken cancellationToken = default)
	{
		var info = await client
			.BalanceAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			account: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult
		{
			IsPlayerExist = info.IsPlayerExist,
			Balance = info.TotalMoney,
			FreeBalance = info.FreeMoney
		};
	}
}
