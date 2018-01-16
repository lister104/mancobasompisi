using Mancoba.Sompisi.Utils.Helpers.Settings;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Platform;

namespace Mancoba.Sompisi.Core.Settings
{
	public class UserSettings : IUserSettings
	{
		public const string UsernameKey = "Username";
		public const string UsernameDefault = "";

		public const string PasswordKey = "Password";
		public const string PasswordDefault = "";

		public const string AccessCodeKey = "AccessCode";
		public const string AccessCodeDefault = "";

        public const string TablesCreatedKey = "TablesCreated";
        public const bool TablesCreatedDefault = false;

        public const string Permissions = "MobilePermissions";
		public const string PermissionsDefault = "";
		public const string Profile = "UserProfile";
		public const string ProfileDefault = "";

		private ISettings _settings;

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
		{
			Username = UsernameDefault;
			Password = PasswordDefault;
			AccessCode = AccessCodeDefault;
		}
        	
		public ISettings AppSettings => _settings ?? (_settings = Mvx.GetSingleton<ISettings>());

		public string Username
		{
			get
			{
				return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue (UsernameKey, value);				
			}
		}

		public string Password
		{
			get
			{
				return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
			}
			set
			{			
				AppSettings.AddOrUpdateValue(PasswordKey, value);				
			}
		}

		public string AccessCode
		{
			get
			{
				return AppSettings.GetValueOrDefault(AccessCodeKey, AccessCodeDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue (AccessCodeKey, value);				
			}
		}

        public bool HasTablesCreated
        {
            get
            {
                return AppSettings.GetValueOrDefault(TablesCreatedKey, TablesCreatedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TablesCreatedKey, value);
            }
        }
    }
}

