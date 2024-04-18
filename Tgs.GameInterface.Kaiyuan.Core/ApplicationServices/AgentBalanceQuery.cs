namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record AgentBalanceQuery(
	Uri ApiUrl,
	string Deskey,
	string MD5key,
	string AgentId);
