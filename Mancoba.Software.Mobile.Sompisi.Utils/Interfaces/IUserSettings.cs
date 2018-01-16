using Mancoba.Sompisi.Utils.Helpers.Settings;

namespace Mancoba.Sompisi.Utils.Interfaces
{
	public interface IUserSettings
	{
		ISettings AppSettings { get; }
		string Username { get; set; }
		string Password { get; set; }
		string AccessCode { get; set; }
        bool HasTablesCreated { get; set; }
        void Clear();
	}
}

