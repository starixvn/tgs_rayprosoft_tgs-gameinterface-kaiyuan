namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record FundOutQuery(
	Uri ApiUrl,
	string Deskey,
	string MD5key,
	string AgentId,
	string PlayerCode,
	string OrderId,
	decimal Amount);
