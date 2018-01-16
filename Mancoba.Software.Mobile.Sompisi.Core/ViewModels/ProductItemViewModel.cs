using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class ProductItemViewModel : MvxViewModel
	{
		public ProductItemViewModel()
		{
		}


		#region Properties
		 
		public bool IsHeading { get; set;}

        public string Id { get; set; }

        private string _name;
		public string Name {
			get { 
				if (IsHeading)
					return "Name";

				return _name; 
			}

			set {

                _name = value;
				RaisePropertyChanged (() => Name);}
		}


		private string _description;
		public string Description {

			get { 
				if(IsHeading)
					return   "Description";
				return _description; }
			set {
                _description = value;
				RaisePropertyChanged (() => Description);}
		}

		private string _price;
		public string Price {

			get { 
				if (IsHeading)
					return "Price";
				return _price; }
			set {
                _price = value;
				RaisePropertyChanged (() => Price);
			}
		}

		 private string _drawable = "icon_wire_f1";
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

