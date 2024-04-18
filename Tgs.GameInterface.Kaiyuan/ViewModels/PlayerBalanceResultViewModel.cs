namespace Tgs.GameInterface.Kaiyuan.ViewModels;

public record PlayerBalanceResultViewModel(
	bool IsPlayerExist,
	decimal Balance,
	decimal FreeBalance,
	decimal OutstandingBalance);
