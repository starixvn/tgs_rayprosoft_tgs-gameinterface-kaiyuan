namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record GameWagerQuery(
    Uri ApiUrl,
    string Deskey,
    string MD5key,
    string AgentId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
