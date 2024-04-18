namespace Tgs.GameInterface.Kaiyuan.Core;

public interface IHashCryptography
{
	ValueTask<string> GetAesEncryptAsync(
		string data,
		string secretKey,
		CancellationToken cancellationToken = default);

	string GetHash(string data);
}
