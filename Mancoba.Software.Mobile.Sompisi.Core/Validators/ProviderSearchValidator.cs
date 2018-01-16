using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Utils.Language;
using FluentValidation;

namespace Mancoba.Sompisi.Core.Validators
{	
	public class ProviderSearchValidator : AbstractValidator<ProviderSearchViewModel>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderSearchValidator"/> class.
        /// </summary>
        public ProviderSearchValidator()
		{
			RuleFor(r => r.SearchText).NotEmpty().WithMessage(LanguageResolver.SearchNoTextError);
			RuleFor(r => r.SearchText.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.SearchNoTextError);
		}
	}
}
