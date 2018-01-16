using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Utils.Language;
using FluentValidation;

namespace Bluescore.DStv.Core.Validators
{
	public class AddressAddValidator : AbstractValidator<AddressAddViewModel>
	{
		public AddressAddValidator()
		{			
			//prov
			RuleFor(r => r.Province).NotEmpty().WithMessage(LanguageResolver.CustAddNoProvinceError);
			RuleFor(r => r.Province.Length).GreaterThanOrEqualTo(6).WithMessage(LanguageResolver.CustAddNoProvinceError);

			//town
			RuleFor(r => r.Town).NotEmpty().WithMessage(LanguageResolver.CustAddNoTownError);
			RuleFor(r => r.Town.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.CustAddNoTownError);

			//suburb
			RuleFor(r => r.Suburb).NotEmpty().WithMessage(LanguageResolver.CustAddNoSuburbError);
			RuleFor(r => r.Suburb.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.CustAddNoSuburbError);
				
			//house
			RuleFor(r => r.AddLocation).NotEmpty().WithMessage(LanguageResolver.AddressAddNoLocationError);			
		}
	}
}

