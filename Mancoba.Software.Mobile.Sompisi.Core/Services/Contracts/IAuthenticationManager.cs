using System.Threading.Tasks;

namespace Bluescore.DStv.Core.Services.Contracts
{
	public interface IAuthenticationManager
	{
		Task<bool> Ping();
		Task<bool> Login(string username, string password, string accessCode);
		Task<bool> TryAutoLogin();
	}
}

