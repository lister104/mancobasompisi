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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseValidationViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        protected BaseValidationViewModel(IMvxMessenger messenger)
			: base(messenger)
		{
		}

        #region Public Properties

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
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

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="model">The model.</param>
        public virtual void ValidateModel<T>(AbstractValidator<T> validator, T model)
		{
			var results = validator.Validate(model);
			ValidationErrors = results.Errors;
			IsModelValid = results.IsValid;
		}

		#endregion
	}
}

