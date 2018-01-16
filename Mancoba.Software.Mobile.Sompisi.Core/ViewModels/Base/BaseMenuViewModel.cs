using MvvmCross.Core.ViewModels;

namespace Mancoba.Sompisi.Core.ViewModels.Base
{
	public abstract class BaseMenuViewModel : MvxViewModel
	{
		private bool _isLoading;
		private string _modelName;
		private string _title;
		private long _id;

		public string ModelName
		{
			get
			{
				return _modelName;
			}

			set
			{
				_modelName = value;
				RaisePropertyChanged(() => ModelName);
			}
		}

		public long Id
		{
			get { return _id; }
			set { _id = value; RaisePropertyChanged(() => Id); }
		}

		public string Title
		{
			get { return _title; }
			set { 
				_title = (value != null) ? value : string.Empty; 
				RaisePropertyChanged(() => Title); 
			}
		}

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				RaisePropertyChanged(() => IsLoading);
			}
		}

		public void ReportError(string error)
		{
			// GetService<IErrorReporter>().ReportError(error);
		}
	}
}

