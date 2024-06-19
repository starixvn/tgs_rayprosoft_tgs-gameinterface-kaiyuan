namespace Tgs.GameInterface.Kaiyuan.Core.UnitTest;

public class HashCryptographyUnitTest
{
	[Fact]
	public void Hash()
	{
		var hashCryptography = new HashCryptography();
		var agent = 72412;
		var timestamp = 1690340035435;
		var md5Key = "B563DF2A3DE58DFB";
		var data = $"{agent}{timestamp}{md5Key}";
		var actual = hashCryptography.GetHash(data: data);

		Assert.NotNull(actual);
		Assert.Equal("2a9a46e4cb2ef009ce1ca3732227ec4f", actual);
	}

	[Fact]
	public async Task AesEncrypt_Balance()
	{
		var hashCryptography = new HashCryptography();
		var secretKey = "E2F3A416006F84F8";
		var data = $"s={(int)SubTypes.Balance}&account=4ucmqplxb029";
		var actual = await hashCryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey);

		Assert.NotNull(actual);
		Assert.Equal("TVvTX3VbPPF8uoTUB9GG382q5wLB1kCJjq8gYecYEyc=", actual);
	}

	[Fact]
	public async Task AesEncrypt_Deposit()
	{
		var hashCryptography = new HashCryptography();
		var secretKey = "E2F3A416006F84F8";
		var data = $"s={(int)SubTypes.Deposit}&account=4ucmqplxb029&money=1.0&orderid=72412202307260300148304ucmqplxb029";
		var actual = await hashCryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey);

		Assert.NotNull(actual);
		Assert.Equal("ho4lHF9xbBKSBKSI47dt9MeLG54H1Gp7fhoksdXiGpleMvtAAFzHCFIFakM5X0PuRThHCQl215so3waG6y28qwyxTQU/BUCgrpXya23E4Bs=", actual);
	}

	[Fact]
	public async Task AesEncrypt_Withdrawal()
	{
		var hashCryptography = new HashCryptography();
		var secretKey = "E2F3A416006F84F8";
		var data = $"s={(int)SubTypes.Withdrawal}&account=4ucmqplxb029&orderid=72412202307171354537634ucmqplxb029&money=1";
		var actual = await hashCryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey);

		Assert.NotNull(actual);
		Assert.Equal("NnlQBQWvhxyimD+GgoNvmkhlBh/yO2eWHoZJ8WoZjOnOixnMbJEPPRaovuGwVNMS4WffbgEyu9pT4LniPlYkaRQ4dlrDtSCertOuxvQXVjY=", actual);
	}

	[Fact]
	public async Task AesEncrypt_Login()
	{
		var hashCryptography = new HashCryptography();
		var secretKey = "E2F3A416006F84F8";
		var data = $"s={(int)SubTypes.Login}&account=4ucmqplxb029&money=0&orderid=72412202307260309201174ucmqplxb029&ip=127.0.0.1&lineCode=GPI&KindID=0";
		var actual = await hashCryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey);

		Assert.NotNull(actual);
		Assert.Equal("AN/kR9Tl/hgbe5UMClo83jPXBmho0mIUfC6GszYjs2H0ivBWXSRAVMPHW6206xIKykfufyQrf55SqtNh32TsilT7ne+yxFaQuFLdaURgJ5Op0nsjfy9JyrM5VLFeUJMbM2xicRz+TSoPz1x8Y5myMw==", actual);
	}

	[Fact]
	public async Task AesEncrypt_GameRecord()
	{
		var hashCryptography = new HashCryptography();
		var secretKey = "E2F3A416006F84F8";
		var data = $"s={(int)SubTypes.GameRecord}&startTime=1689573304660&endTime=1689573604660";
		var actual = await hashCryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey);

		Assert.NotNull(actual);
		Assert.Equal("H5kLMUYwNlqbCtfaD/H4q33IUwwYr7Owu3a3QJi9Jj1lCR48Wnkqm/Nl9Z3UEdXuVHZOgPcUHBwewVlMD0cssg==", actual);
	}
}
