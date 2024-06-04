using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public class PlayerOnlineGetService(
	IGameApiClient client) : IGetService<PlayerOnlineQuery, bool>
{
	public async ValueTask<bool> GetAsync(PlayerOnlineQuery query, CancellationToken cancellationToken = default)
		=> await client.OnlineStatusAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			account: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
}
