using System;

namespace Mancoba.Sompisi.Core.ViewModels.Base
{
	public class MenuViewModel : BaseMenuViewModel
	{
		private Section _section;
		public Section Section
		{
			get { return _section; }
			set
			{
				_section = value;
				Id = (int)_section;
				RaisePropertyChanged(() => Section);
			}
		}

		public Type ViewModelType;
		public bool IsOnMainMenu
		{
			get;
			set;
		}

		private int _menuIndex = -1;
		public int MenuIndex
		{
			get { return _menuIndex; }
			set { _menuIndex = value; }
		}

		public string SectionName { get { return _section.ToString(); } }

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				RaisePropertyChanged(() => IsSelected);
			}
		}

		private string _count ;
		public string Count {
			get { return _count; }
			set {
				_count =  value;
				RaisePropertyChanged (() => Count);
			}
		}
	}
}

