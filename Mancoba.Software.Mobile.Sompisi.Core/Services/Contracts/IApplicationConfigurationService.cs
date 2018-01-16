using Mancoba.Sompisi.Utils.Enums;

namespace Mancoba.Sompisi.Core.Services.Contracts
{
	public interface IApplicationConfigurationService
	{
		PlatformTypeEnum GetPlatformType();
		DeviceTypeEnum GetDeviceType();
		string GetDeviceKey();
	}
}

