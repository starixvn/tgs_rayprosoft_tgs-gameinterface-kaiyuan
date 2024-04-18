using System.Diagnostics;

namespace Tgs.GameInterface.Kaiyuan;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Kaiyuan.DiagnosticsInterceptor");
}
