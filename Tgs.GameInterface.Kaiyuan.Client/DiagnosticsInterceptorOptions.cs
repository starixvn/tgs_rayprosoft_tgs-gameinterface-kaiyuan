using System.Diagnostics;

namespace Tgs.GameInterface.Kaiyuan.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Kaiyuan.Client.DiagnosticsInterceptor");
}
