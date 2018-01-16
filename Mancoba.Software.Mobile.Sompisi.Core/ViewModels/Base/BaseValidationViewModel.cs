using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using MvvmCross.Plugins.Messenger;

namespace Mancoba.Sompisi.Core.ViewModels.Base
{
	public abstract class BaseValidationViewModel : MessengerBaseViewModel
	{
		#region Private Variables

		private bool _isModelValid = true;
		private IList<ValidationFailure> _validationErrors;

		#endregion

		protected BaseValidationViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
		}

		#region Public Properties

		public IList<ValidationFailure> ValidationErrors
		{
			get { return _validationErrors; }
			set
			{
				_validationErrors = value;
				RaisePropertyChanged(() => ValidationErrors);
			}
		}

		public bool IsModelValid
		{
			get { return _isModelValid; }
			set
			{
				_isModelValid = value;
				RaisePropertyChanged(() => IsModelValid);
			}
		}

		#endregion

		#region Public Methods

		public virtual void ValidateModel<T>(AbstractValidator<T> validator, T model)
		{
			var results = validator.Validate(model);
			ValidationErrors = results.Errors;
			IsModelValid = results.IsValid;
		}

		#endregion
	}
}

