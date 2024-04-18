using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Kaiyuan.Core.ApplicationServices;
using Tgs.GameInterface.Kaiyuan.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Kaiyuan.Controllers;

[Route("[controller]")]
[ApiController]
public class KaiyuanController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "orderId": "72412202310060331234064ucmqplxb029",
	///             "playerCode":"4ucmqplxb029",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<string> FundInAsync(
		[FromServices] IGetService<FundInQuery, string> service,
		FundViewModel source)
	{
		var query = new FundInQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "orderId": "72412202310060331279044ucmqplxb029",
	///             "playerCode":"4ucmqplxb029",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<string> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, string> service,
		FundViewModel source)
	{
		var query = new FundOutQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "orderId":"72412202307260300148304ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusViewModel source)
	{
		var query = new FundStatusQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			OrderId: source.OrderId);

		return service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Games the wager asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-record.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "startTime": "2023-07-26T11:50:00+08:00",
	///             "endTime": "2023-07-26T12:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/wager")]
	public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerAsync(
		[FromServices] IQueryService<GameWagerQuery, GameWagerResult> service,
		GameWagerViewModel source)
	{
		var query = new GameWagerQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			StartTime: source.StartTime,
			EndTime: source.EndTime);

		var results = await service
			.QueryAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		if (!results.Any())
		{
			yield break;
		}

		foreach (var result in results)
		{
			yield return new GameWagerResultViewModel(
				ExternalWagerId: result.ExternalWagerId,
				ExternalGameId: result.ExternalGameId,
				PlayerCode: result.PlayerCode,
				ExternalCreatedOn: result.ExternalCreatedOn,
				ExternalClosedOn: result.ExternalClosedOn,
				Bet: result.Bet,
				ValidBet: result.ValidBet,
				Settle: result.Settle,
				Commission: result.Commission,
				WinLoss: result.WinLoss,
				AdditionalData: result.AdditionalData);
		}
	}

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "brandCode": "GPI",
	///             "playerCode":"4ucmqplxb029",
	///             "ipAddress": "127.0.0.1",
	///             "externalGameId": "0"
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public ValueTask<string> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string> service,
		GameUrlViewModel source)
	{
		var query = new GameUrlQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			BrandCode: source.BrandCode,
			PlayerCode: source.PlayerCode,
			IpAddress: source.IpAddress,
			ExternalGameId: source.ExternalGameId);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "playerCode":"4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceViewModel source)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode);

		var result = await service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new PlayerBalanceResultViewModel(
			IsPlayerExist: result.IsPlayerExist,
			Balance: result.Balance,
			FreeBalance: result.FreeBalance,
			OutstandingBalance: result.OutstandingBalance);
	}

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412",
	///             "playerCode":"4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutViewModel source)
	{
		var command = new PlayerLogoutCommand(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode);

		return service
			.ExecuteAsync(
			command: command,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Agents the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.hddv1.com/",
	///             "deskey": "E2F3A416006F84F8",
	///             "mD5key": "B563DF2A3DE58DFB",
	///             "agentId": "72412"
	///         }
	/// </remarks>
	[HttpPost("agent/balance", Name = "AgentBalance")]
	public async ValueTask<AgentBalanceResultViewModel> AgentBalanceAsync(
		[FromServices] IGetService<AgentBalanceQuery, AgentBalanceResult> service,
		AgentBalanceViewModel source)
	{
		var query = new AgentBalanceQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId);

		var result = await service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new AgentBalanceResultViewModel(
			Credit: result.Credit);
	}
}
