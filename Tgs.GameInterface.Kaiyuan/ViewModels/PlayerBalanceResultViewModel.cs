namespace Tgs.GameInterface.Kaiyuan.ViewModels;

public record PlayerBalanceResultViewModel(
    decimal Balance,
    decimal FreeBalance,
    decimal OutstandingBalance);
