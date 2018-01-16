using AutoMapper;
using Mancoba.Sompisi.Core.Services;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Core.Settings;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Data;
using Mancoba.Sompisi.Data.Models;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Sqlite;

namespace Mancoba.Sompisi.Core
{
	public class App : MvxApplication
	{
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
		{
			Mvx.ConstructAndRegisterSingleton<IUserSettings, UserSettings>();

			PluginLoader.Instance.EnsureLoaded();
			Mvx.ConstructAndRegisterSingleton<IMancobaMobileDataApi, MancobaMobileDataApi>();
			Mvx.RegisterSingleton<IMobileDataService>(MobileDataService.DataServiceInstance);

			EntityMapping();
			//Navigation.SetUp ();

			RegisterAppStart(new CustomApplicationStart());
		}

        /// <summary>
        /// Entities the mapping.
        /// </summary>
        static void EntityMapping()
		{
            Mapper.CreateMap<ModelApplication, DraftItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))                
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.SuburbName, opt => opt.MapFrom(src => src.Suburb))
                .ForMember(dest => dest.TownName, opt => opt.MapFrom(src => src.Town))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode));

            Mapper.CreateMap<ModelApplication, SentItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.SuburbName, opt => opt.MapFrom(src => src.Suburb))
                .ForMember(dest => dest.TownName, opt => opt.MapFrom(src => src.Town))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode));
        }
    }
}
