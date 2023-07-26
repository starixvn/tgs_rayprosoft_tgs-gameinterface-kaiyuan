using Tgs.GameInterface.Kaiyuan.Protos;

namespace Tgs.GameInterface.Kaiyuan.Client;

public interface IGameInterfaceKaiyuanClient
{
    ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default);

    ValueTask<string> FundInAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<string> FundOutAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string orderId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string brandCode,
        string playerCode,
        string ipAddress,
        string gameExternalID,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default);
}
