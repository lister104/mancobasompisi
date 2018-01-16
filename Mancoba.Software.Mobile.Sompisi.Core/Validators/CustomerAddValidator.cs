using Bluescore.DStv.Core.ViewModels;
using Bluescore.DStv.Utils.Language;
using FluentValidation;

namespace Bluescore.DStv.Core.Validators
{
	public class CustomerAddValidator : AbstractValidator<MyDetailsViewModel>
	{
		public CustomerAddValidator()
		{
			//last name
			RuleFor(r => r.LastName).NotEmpty().WithMessage(LanguageResolver.CustAddNoLastNameError);
			RuleFor(r => r.LastName.Length).GreaterThanOrEqualTo(2).WithMessage(LanguageResolver.CustAddNoLastNameError);

			// first names
			RuleFor(r => r.FirstName).EmailAddress().WithMessage(LanguageResolver.CustAddNoFirstNamesError);
			RuleFor(r => r.FirstName.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.CustAddNoFirstNamesError);

			// title
			RuleFor(r => r.UserTitle).NotEmpty().WithMessage(LanguageResolver.CustAddNoUserTitleError);
			RuleFor(r => r.UserTitle.Length).GreaterThanOrEqualTo(2).WithMessage(LanguageResolver.CustAddNoUserTitleError);

			//id
			RuleFor(r => r.IdNumber).NotEmpty().WithMessage(LanguageResolver.CustAddNoIdNumberError);
			RuleFor(r => r.IdNumber.Length).GreaterThanOrEqualTo(5).WithMessage(LanguageResolver.CustAddNoIdNumberError);

			//prov
			RuleFor(r => r.Province).NotEmpty().WithMessage(LanguageResolver.CustAddNoProvinceError);
			RuleFor(r => r.Province.Length).GreaterThanOrEqualTo(5).WithMessage(LanguageResolver.CustAddNoProvinceError);

			//town
			RuleFor(r => r.Town).NotEmpty().WithMessage(LanguageResolver.CustAddNoTownError);
			
			//suburb
			RuleFor(r => r.Suburb).NotEmpty().WithMessage(LanguageResolver.CustAddNoSuburbError);
			
			//street
			RuleFor(r => r.Street).NotEmpty().WithMessage(LanguageResolver.CustAddNoStreetError);
			
			//house
			RuleFor(r => r.House).NotEmpty().WithMessage(LanguageResolver.CustAddNoHouseError);            
        }
    }
}

