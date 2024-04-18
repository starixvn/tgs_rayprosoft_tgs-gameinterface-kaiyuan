namespace Tgs.GameInterface.Kaiyuan.Core.Models;

public record BalanceInfo(
	bool IsPlayerExist,
	decimal TotalMoney,
	decimal FreeMoney);
