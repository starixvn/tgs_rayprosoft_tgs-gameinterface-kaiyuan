using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Web;

namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest;
public class OnlineStatusUnitTest
{
	[Fact]
	public async void Normal_Process()
	{
		var policySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			DepositRetryCount = 1,
			DepositRetryWaitSec = 1,
			GameRecordRetryCount = 1,
			GameRecordRetryWaitSec = 1
		});

		Uri apiUrl = new Uri("https://wc1-api.hddv1.com/");
		var deskey = "E2F3A416006F84F8";
		var md5key = "B563DF2A3DE58DFB";
		var agentId = "72412";
		var account = "4ucmqplxb029";

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1690340035435);
		_ = fakeSystemClock.CurrentTime.Returns(currentTime);

		var data = "s=5&account=4ucmqplxb029";
		var param = "TVvTX3VbPPF8uoTUB9GG382q5wLB1kCJjq8gYecYEyc=";
		_ = fakeCryptography.GetAesEncryptAsync(
			data: Arg.Is(data),
			secretKey: Arg.Is(deskey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(param);

		var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
		var key = "2a9a46e4cb2ef009ce1ca3732227ec4f";
		_ = fakeCryptography.GetHash(
			data: Arg.Is(dataMD5))
			.Returns(key);

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["agent"] = agentId;
		query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
		query["param"] = param;
		query["key"] = key;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["m"] = "/channelHandle",
			["s"] = 105,
			["d"] = new JsonObject
			{
				["code"] = 0,
				["status"] = 1
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			policySettingsOptions,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.OnlineStatusAsync(
			apiUrl: apiUrl,
			deskey: deskey,
			md5key: md5key,
			agentId: agentId,
			account: account);

		Assert.True(actual);
	}

	[Fact]
	public async void Data_non_existence()
	{
		var policySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			DepositRetryCount = 1,
			DepositRetryWaitSec = 1,
			GameRecordRetryCount = 1,
			GameRecordRetryWaitSec = 1
		});

		Uri apiUrl = new Uri("https://wc1-api.hddv1.com/");
		var deskey = "E2F3A416006F84F8";
		var md5key = "B563DF2A3DE58DFB";
		var agentId = "72412";
		var account = "4ucmqplxb029";

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1690340035435);
		_ = fakeSystemClock.CurrentTime.Returns(currentTime);

		var data = "s=5&account=4ucmqplxb029";
		var param = "TVvTX3VbPPF8uoTUB9GG382q5wLB1kCJjq8gYecYEyc=";
		_ = fakeCryptography.GetAesEncryptAsync(
			data: Arg.Is(data),
			secretKey: Arg.Is(deskey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(param);

		var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
		var key = "2a9a46e4cb2ef009ce1ca3732227ec4f";
		_ = fakeCryptography.GetHash(
			data: Arg.Is(dataMD5))
			.Returns(key);

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["agent"] = agentId;
		query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
		query["param"] = param;
		query["key"] = key;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["m"] = "/channelHandle",
			["s"] = 105,
			["d"] = new JsonObject
			{
				["code"] = 16,
				["status"] = 1
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			policySettingsOptions,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.OnlineStatusAsync(
			apiUrl: apiUrl,
			deskey: deskey,
			md5key: md5key,
			agentId: agentId,
			account: account);

		Assert.False(actual);
	}
}
