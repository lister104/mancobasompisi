﻿using Android.App;
using Android.Content.Res;
using Android.Telephony;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Utils.Enums;

namespace Mancoba.Sompisi.Droid.Classes.Platform
{
	public class AndroidConfigurationService : IApplicationConfigurationService
	{
        /// <summary>
        /// Gets the type of the platform.
        /// </summary>
        /// <returns></returns>
        public PlatformTypeEnum GetPlatformType()
		{
			return PlatformTypeEnum.Android;
		}

        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <returns></returns>
        public DeviceTypeEnum GetDeviceType()
		{
			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			return (Application.Context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) >= ScreenLayout.SizeLarge
				                                                                                                     ? DeviceTypeEnum.Tablet
					                                                                                                     : DeviceTypeEnum.Phone;
		}

        /// <summary>
        /// Gets the device key.
        /// </summary>
        /// <returns></returns>
        public string GetDeviceKey()
		{
			TelephonyManager tm = (TelephonyManager) Application.Context.GetSystemService(Android.Content.Context.TelephonyService);
			return  tm.DeviceId;
		}
	}
}

