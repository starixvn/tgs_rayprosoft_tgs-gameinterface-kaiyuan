using Tgs.GameInterface.Kaiyuan.Core.Models;

namespace Tgs.GameInterface.Kaiyuan.Core;

public interface IGameRecordApiClient
{
    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string deskey, string md5key, string agentId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
}
