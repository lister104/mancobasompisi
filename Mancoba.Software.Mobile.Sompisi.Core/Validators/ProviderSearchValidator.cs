using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Utils.Language;
using FluentValidation;

namespace Mancoba.Sompisi.Core.Validators
{	
	public class ProviderSearchValidator : AbstractValidator<ProviderSearchViewModel>
	{
		public ProviderSearchValidator()
		{
			RuleFor(r => r.SearchText).NotEmpty().WithMessage(LanguageResolver.SearchNoTextError);
			RuleFor(r => r.SearchText.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.SearchNoTextError);
		}
	}
}
