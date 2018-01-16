namespace Mancoba.Sompisi.Utils.Interfaces
{
	public interface IPlatformCapabilities
	{
		bool HasNetworkConnection();
		bool UseLocalCaching();
		bool IsMapVisible { get; set; }
	}
}
