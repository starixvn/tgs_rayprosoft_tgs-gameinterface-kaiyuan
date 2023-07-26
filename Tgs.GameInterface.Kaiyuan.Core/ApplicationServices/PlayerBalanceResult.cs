namespace Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;

public record PlayerBalanceResult(
    decimal LockBalance,
    decimal FreeBalance,
    decimal Balance);
