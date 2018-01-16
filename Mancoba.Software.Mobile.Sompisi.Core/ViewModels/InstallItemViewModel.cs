using System;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
namespace Mancoba.Sompisi.Core.ViewModels
{
	public class InstallItemViewModel : MvxViewModel
	{
		public InstallItemViewModel ()
		{
		}


		#region Properties
		 
		public bool IsHeading { get; set;}


		private string _wire;
		public string Wire {
			get { 
				if (IsHeading)
					return "Wire";
				return string.Empty; 
			}

			set {
				
				_wire = value;

				RaisePropertyChanged (() => Wire);}
		}


		private string _line;
		public string Line {

			get { 
				if(IsHeading)
					return "Line";
				return _line; }
			set {
				_line = value;
				RaisePropertyChanged (() => Line);}
		}

		private string _connection;
		public string Connection {

			get { 
				if (IsHeading)
					return "Connection";
				return _connection; }
			set {
				_connection = value;
				RaisePropertyChanged (() => Connection);
			}
		}

		 private string _drawable;
		 public string WireDrawable {
			get { return _drawable; }
			set {


				switch (value) {

				case "icon-wire-f1":
					_drawable = "icon_wire_f1";
					break;

				case "icon-wire-f2":

					_drawable = "icon_wire_f2";
					break;
				case "icon-wire-f3":

					_drawable = "icon_wire_f3";
					break;

				default:

					_drawable = "icon_wire_f3";
					break;

				}

				RaisePropertyChanged (() => WireDrawable);
			}
		}
		 

		#endregion
	}
}

