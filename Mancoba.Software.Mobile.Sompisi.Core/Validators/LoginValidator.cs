using Mancoba.Sompisi.Core.ViewModels;
using Mancoba.Sompisi.Utils.Language;
using FluentValidation;

namespace Mancoba.Sompisi.Core.Validators
{
	public class LoginValidator : AbstractValidator<LoginViewModel>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginValidator"/> class.
        /// </summary>
        public LoginValidator()
		{		
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(LanguageResolver.LoginInvalidEmailError).EmailAddress().WithMessage(LanguageResolver.LoginInvalidEmailError);
            RuleFor(x => x.Password).NotEmpty().WithMessage(LanguageResolver.LoginNoPasswordError);			
		}
	}
}

