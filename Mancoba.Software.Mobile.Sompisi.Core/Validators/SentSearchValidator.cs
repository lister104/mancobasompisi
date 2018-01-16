using FluentValidation;
using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Utils.Language;

namespace Mancoba.Sompisi.Core.Validators
{
    public class SentSearchValidator : AbstractValidator<SentSearchViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SentSearchValidator"/> class.
        /// </summary>
        public SentSearchValidator()
        {
            RuleFor(r => r.SearchText).NotEmpty().WithMessage(LanguageResolver.SearchNoTextError);
            RuleFor(r => r.SearchText.Length).GreaterThanOrEqualTo(3).WithMessage(LanguageResolver.SearchNoTextError);
        }
    }
}
