using System.Threading.Tasks;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Utils.Enums;
using Mancoba.Sompisi.Utils.Language;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Mancoba.Sompisi.Core.ViewModels
{
	public class TermsViewModel : MvxViewModel
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TermsViewModel"/> class.
        /// </summary>
        public TermsViewModel()
		{			
			CloseCommand = new MvxAsyncCommand(CloseCommandHandler);
		}
        /// <summary>
        /// Gets the terms and conditions title.
        /// </summary>
        /// <value>
        /// The terms and conditions title.
        /// </value>
        public string TermsAndConditionsTitle
		{
			get
			{
				return LanguageResolver.TermsAndConditionsTitle;
			}
		}

        /// <summary>
        /// Gets the terms and conditions text.
        /// </summary>
        /// <value>
        /// The terms and conditions text.
        /// </value>
        public string TermsAndConditionsText
		{
			get
			{

				string text = string.Empty;
				if (Mvx.Resolve<IApplicationConfigurationService>().GetDeviceType() == DeviceTypeEnum.Phone)
				{

					text = string.Format(@"<html><head><style>	html{{font-family: Helvetica;font-size: 16px;}}h2{{	font-size: 19px;}}h3{{font-size: 17px;}}</style></head><body><h2> {0}</h2> <p>{1}</p><p>{2}</p><p>{3}</p><h3>{4}</h3><p>{5}</p><p>{6}</p>
				<p>{7}</p><p>{8}</p><h3>{9}</h3><p>{10}</p>	<p>{11}</p>	<p>{12}</p>	<p>{13}</p>	<p> {14}</p>	<h3>{15}</h3>	<p> {16}</p> <p>{17}</p><p>{18}</p><h3>{19}</h3><p>{20}</p></body></html>",
						LanguageResolver.TermsAndConditionsText, LanguageResolver.TermsAndConditionsText2, LanguageResolver.TermsAndConditionsText3, LanguageResolver.TermsAndConditionsText4, LanguageResolver.TermsAndConditionsText5, LanguageResolver.TermsAndConditionsText6
				, LanguageResolver.TermsAndConditionsText7, LanguageResolver.TermsAndConditionsText8, LanguageResolver.TermsAndConditionsText9, LanguageResolver.TermsAndConditionsText10, LanguageResolver.TermsAndConditionsText11
				, LanguageResolver.TermsAndConditionsText12, LanguageResolver.TermsAndConditionsText13, LanguageResolver.TermsAndConditionsText14, LanguageResolver.TermsAndConditionsText15, LanguageResolver.TermsAndConditionsText16
				, LanguageResolver.TermsAndConditionsText17, LanguageResolver.TermsAndConditionsText18, LanguageResolver.TermsAndConditionsText19, LanguageResolver.TermsAndConditionsText20, LanguageResolver.TermsAndConditionsText21);

				}
				else
				{

					text = string.Format(@"<html><head><style>	html{{font-family: Helvetica;font-size: 20px;}}h2{{	font-size: 35px;}}h3{{font-size: 25px;}}</style></head><body><h2> {0}</h2> <p>{1}</p><p>{2}</p><p>{3}</p><h3>{4}</h3><p>{5}</p><p>{6}</p>
				<p>{7}</p><p>{8}</p><h3>{9}</h3><p>{10}</p>	<p>{11}</p>	<p>{12}</p>	<p>{13}</p>	<p> {14}</p>	<h3>{15}</h3>	<p> {16}</p> <p>{17}</p><p>{18}</p><h3>{19}</h3><p>{20}</p></body></html>",
						LanguageResolver.TermsAndConditionsText1, LanguageResolver.TermsAndConditionsText2, LanguageResolver.TermsAndConditionsText3, LanguageResolver.TermsAndConditionsText4, LanguageResolver.TermsAndConditionsText5, LanguageResolver.TermsAndConditionsText6
						, LanguageResolver.TermsAndConditionsText7, LanguageResolver.TermsAndConditionsText8, LanguageResolver.TermsAndConditionsText9, LanguageResolver.TermsAndConditionsText10, LanguageResolver.TermsAndConditionsText11
						, LanguageResolver.TermsAndConditionsText12, LanguageResolver.TermsAndConditionsText13, LanguageResolver.TermsAndConditionsText14, LanguageResolver.TermsAndConditionsText15, LanguageResolver.TermsAndConditionsText16
						, LanguageResolver.TermsAndConditionsText17, LanguageResolver.TermsAndConditionsText18, LanguageResolver.TermsAndConditionsText19, LanguageResolver.TermsAndConditionsText20, LanguageResolver.TermsAndConditionsText21);

				}

				return text;
			}
		}

        /// <summary>
        /// Gets the terms ok.
        /// </summary>
        /// <value>
        /// The terms ok.
        /// </value>
        public string TermsOk
		{
			get { return LanguageResolver.Ok; }
		}

		public IMvxAsyncCommand CloseCommand { get; }

        /// <summary>
        /// Closes the command handler.
        /// </summary>
        /// <returns></returns>
        public async Task CloseCommandHandler()
		{
			Close(this);
		}
	}	
}

