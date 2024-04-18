namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string Deskey,
	string MD5key,
	string AgentId,
	string PlayerCode);
