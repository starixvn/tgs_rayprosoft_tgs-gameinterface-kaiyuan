using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Options;
using Tgs.GameInterface.Kaiyuan.Client;
using Tgs.GameInterface.Kaiyuan.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceKaiyuanClient(
        this IServiceCollection services,
        Action<GameInterfaceKaiyuanClientOptions, IServiceProvider> configure)
    {
        var isDns = false;
        _ = services
            .AddOptions<GameInterfaceKaiyuanClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceKaiyuan.GameInterfaceKaiyuanClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceKaiyuanClientOptions>>().Value;
                isDns = options.BaseUrl.Scheme.Equals("dns", StringComparison.OrdinalIgnoreCase);
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
                configureChannel.Credentials = isDns
                ? ChannelCredentials.Insecure
                : null;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceKaiyuanClient, GameInterfaceKaiyuanClient>();
        if (isDns)
        {
            //負載平衡時，效能很重要。 藉由快取位址，解析位址的延遲會從 gRPC 呼叫中排除。 進行第一個 gRPC 呼叫時，將會叫用解析程式，而後續的呼叫會使用快取。
            _ = services.AddSingleton<ResolverFactory>(sp => new DnsResolverFactory(refreshInterval: TimeSpan.FromSeconds(30)));
        }
        return services;
    }
}
