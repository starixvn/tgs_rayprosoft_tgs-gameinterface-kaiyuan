namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string Deskey,
    string MD5key,
    string AgentId,
    string PlayerCode);
