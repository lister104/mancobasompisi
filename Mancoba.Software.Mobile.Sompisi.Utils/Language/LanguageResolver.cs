using System.Reflection;
using Mancoba.Sompisi.Utils.Helpers;

namespace Mancoba.Sompisi.Utils.Language 
{
	public static class LanguageResolver
	{
		public static string GetTheString(string message)
		{
			var type = typeof(LanguageResolver);
			var fieldInfo = type.GetRuntimeField(message);
			if (fieldInfo == null) return message;
			var result = fieldInfo.GetValue(null) as string;
			return GetString(message, result);
		}

		public static string GetString(string comment, string message = null)
		{
			var translated  = Localise.GetString(comment);

			if (translated == null)
				return message; 
			return translated;

		}

		// IMPORTANT! If you add strings to this file, you must also add them to Strings.xml 

		// Connection error string
		public static string NoConnectionExceptionMessage = GetString("NoConnectionExceptionMessage", "You do not seem to have network connectivity.");

		// Generic error string
		public static string GenericErrorOccurred = GetString("GenericErrorOccurred", "Unfortunately an error has occurred.");

		// Login view strings
		public static string AppName { get { return GetString("AppName", "TECH TOOL"); } }
		public static string LoginEmailAddress = GetString("LoginEmailAddress", "Email Address");
		public static string LoginPassword = GetString("LoginPassword", "Password");
		public static string LoginButton = GetString("LoginButton", "Sign in");
		public static string LoginVersion = GetString("LoginVersion", "Version");

		public static string LoginErrorTitle = GetString("LoginErrorTitle", "You have provided invalid credentials. Please try again");
		public static string LoginConnectivityError = GetString("LoginConnectivityError", "We have encountered a login error. Please contact support.");
		public static string LoginInvalidEmailError = GetString("LoginInvalidEmailError", "Please enter a valid email address.");
		public static string LoginNoPasswordError = GetString("LoginNoPasswordError", "Please enter a password.");

		public static string AccountDisabled = GetString("AccountDisabled", "Your account has been disabled. Please contact your administrator.");
		public static string AccountInactive = GetString("AccountInactive", "Your account is inactive. Please contact your administrator.");
		public static string InvalidCredentials = GetString("InvalidCredentials", "You have provided invalid credentials. Please try again.");
		public static string AccountLocked = GetString("AccountLocked", "Your account has been locked. Please visit the web application and follow the forgot password process.");
		public static string AccountUnverified = GetString("AccountUnverified", "Your account is unverified. Please contact your administrator.");

		//ADD CUSTOMER
		public static string CustAddNoFirstNamesError = GetString("CustAddNoFirstNamesError", "Please enter valid first names.");
		public static string CustAddNoLastNameError = GetString("CustAddNoLastNameError", "Please enter a valid last name.");
		public static string CustAddNoIdNumberError = GetString("CustAddNoIdNumberError", "Please enter a valid id number.");
		public static string CustAddNoUserTitleError = GetString("CustAddNoUserTitleError", "Please select a valid title.");
		public static string CustAddNoProvinceError = GetString("CustAddNoProvinceError", "Please select a valid province.");
		public static string CustAddNoTownError = GetString("CustAddNoTownError", "Please select a valid town.");
		public static string CustAddNoSuburbError = GetString("CustAddNoSuburbError", "Please select a valid suburb.");
		public static string CustAddNoStreetError = GetString("CustAddNoStreetError", "Please select a valid street.");
		public static string CustAddNoHouseError = GetString("CustAddNoHouseError", "Please select a valid house number.");
        public static string AddressAddNoLocationError = GetString("AddressAddNoLocationError", "Please type the text you want to add.");

        // Terms and Conditions strings
        public static string TermsAndConditionsTitle = GetString("TermsAndConditionsTitle", "Terms of Use");

		public static string ForgotPasswordTitle = GetString("ForgotPasswordTitle", "Forgot Your Password?");

		// Settings view strings
		public static string AutoLogin = GetString("AutoLogin", "Remember me");		 	 
		public static string UserSettings = GetString("UserSettings", "User Settings");

		//Search View
		public static string SearchText = GetString("SearchText", "Search Text");
		public static string SearchButton = GetString("SearchButton", "Find Customer");
		public static string SearchErrorTitle = GetString("SearchErrorTitle",
			"No Customer Details related to the Search Text have been found. Please try again");
		public static string SearchNoTextError = GetString("SearchNoTextError", "Please enter the customer details to search for.");

		// General strings
		public static string FilterBox = GetString("FilterBox", "Filter");
		public static string Edit = GetString ("Edit", "Edit");
		public static string Ok = GetString("Ok", "OK");
		public static string Done = GetString("Done", "Done");
		public static string Back = GetString("Back", "Back");
        public static string Favourite = GetString("Favourite", "Favourite");
        public static string CheckIn = GetString ("CheckIn", "Check In");
		public static string CheckOut = GetString ("CheckOut", "Check Out");
		public static string Cancel = GetString ("Cancel", "Cancel");
		public static string Save = GetString("Save", "Save");
		public static string Accept = GetString ("Accept", "Accept");
		public static string Remove = GetString ("Remove", "Remove");
		public static string Next = GetString("Next", "Next");
		// Slider menu strings
		public static string MenuItemSummary = GetString("MenuItemSummary", "Summary");

		// Selection
		public static string Navigation = GetString("Navigation", "Tech Tool");
		public static string Loading = GetString("Loading", "Loading...");

        // Product
        public static string ProductName = GetString("ProductName", "Name");
        public static string ProductDescription = GetString("ProductDescription", "Description");
        public static string Price = GetString("Price", "Price");

        // Installer Item View
        public static string Name = GetString("Name", "Name");
        public static string Contact = GetString("Contact", "Contact");

        public static string CustomerSummary = GetString("CustomerSummary", "Customer Summary");
        public static string AssetSummary = GetString("AssetSummary", "Asset Summary");
        public static string AddressSummary = GetString("AddressSummary", "Address Summary");
        public static string AccountNumber = GetString("AccountNumber", "Account Number");
        public static string LastPaymentDate = GetString("LastPaymentDate", "Last Payment Date");
        public static string Status = GetString("Status", "Status");

        // MyDetails
        public static string FirstNames = GetString("FirstNames", "First Names");
		public static string UserName = GetString("UserName", "User Name");
		public static string LastName = GetString("LastName", "Last Name");
        public static string IdNumber = GetString("IdNumber", "ID Number");
        public static string UserTitle = GetString("UserTitle", "Title");

        //Settings Strings
        public static string Metric = GetString("Metric", "Metric");
		public static string Imperial = GetString("Imperial", "Imperial (UK)");
		// ReSharper disable once InconsistentNaming
		public static string USImperial = GetString("USImperial", "Imperial (US)");
		public static string SemiMetricUsGallons = GetString("SemiMetricUsGallons", "Semi-metric (US gallons)");
		public static string MetricHiRes = GetString("MetricHiRes", "Metric (High res)");
        public static string ApplicationCountLabel = GetString("ApplicationCountLabel", "Showing {0} Applications sent in ascending order");
        public static string InstallerCountLabel = GetString("InstallerCountLabel", "Showing {0} Installers in ascending order");
		public static string ProviderCountLabel = GetString("ProviderCountLabel", "Showing {0} Providers in ascending order");

		public static string TermsAndConditionsText = GetString("TermsAndConditionsText",
																"Mobile application usage terms and conditions\n\n" +
																"Welcome to the Mancoba (Pty) Ltd ('Mancoba Software') mobile application. " +
                                                                "If you continue to log in to and use this mobile application, you are agreeing to comply with and be bound by the following terms and conditions of use, which together with our privacy policy govern Mancoba Software relationship with you in relation to this mobile application.  " +
																"If you do not wish to be bound by these terms and conditions, then you may not access or use any of the content of this mobile application.\n\n" +
                                                                "Mancoba Software is the owner of the mobile application and related website; with its registered office at 232, Forest Drive Ext, Pinelands, Cape Town, 7405, South Africa; the company registration number is 2015/349945/07; place of registration is the Republic of South Africa.  " +
																"The term 'you' refers to the user or viewer of our mobile application.\n\n" +
																"The use of this mobile application is subject to the following terms of use: \n\n" +
																"Disclaimer\n\n" +
																"You agree that your use of this mobile application is for lawful purposes only. " +
																"You agree that you will not use this mobile application for any unlawful purpose, including committing a criminal offense.\n\n" +
																"The content of the pages of this mobile application is for your general information and use only. It is subject to change without notice.\n\n" +
                                                                "Mancoba Software reserves the right, in its sole discretion and without notice, to interrupt, suspend or terminate this mobile application, and to change the software and/or hardware required to gain access to the website though this application. " +
                                                                "Mancoba Software will however, where practicable, provide notice of such changes." +
                                                                "Mancoba Software cannot provide any warranty or guarantee as to the accuracy, timeliness, performance, completeness or suitability of the information and materials found or offered through this mobile application for any particular purpose. " +
                                                                "You acknowledge that such information and materials may contain inaccuracies or errors and Mancoba Software expressly exclude liability for any such inaccuracies or errors.\n\n" +
																"There is no guarantee that you will always be able to access the website through this mobile application. " +
                                                                "Mancoba Software will not be liable to you for any interruption or delay that you experience in accessing the website, whatever the cause. " +
                                                                "Mancoba Software does not warrant that the website or the server or mobile application that makes it available are free of viruses or bugs or other harmful components.\n\n" +
																"Liability\n\n" +
                                                                "Your use of any information or materials on this website is entirely at your own risk, for which Mancoba Software shall not be liable. " +
                                                                "The mobile application and all its contents are provided on an 'as is' basis, and Mancoba Software Software makes no representations or warranties of any kind, whether express or implied, to the accuracy of the contents of the mobile application. " +
																"It shall be your own responsibility to ensure that any products, services or information available through this website meet your specific requirements. " +
                                                                "Mancoba Software does not warrant that the mobile application's functions will be uninterrupted or error-free, or that the site or its server is free from viruses or other harmful components\n\n" +
                                                                "Mancoba Software, its owners, directors, employees, officials, suppliers, agents and/or representatives shall not be liable for any loss or damage, whether direct, indirect or consequential, or any expense of any nature whatsoever, which may be suffered by the user, which arises directly or indirectly from reliance of the mobile application and/or its content.\n\n" +
                                                                "Mancoba Software shall not be responsible for any direct or indirect special consequential or other damage of any kind whatsoever suffered or incurred by you related to your use of, or your inability to access or use, the content or the website or any functionality of the website or of any linked website, even where Mancoba Software is expressly advised thereof.\n\n" +
                                                                "You will indemnify Mancoba Software, its owners, members, employees, officials, agents or representatives, and keep them fully indemnified, from and against any loss or damage suffered or liability incurred in respect of any third party, which arises from your use of this website.\n\nUnauthorised use of this website may give rise to a claim for damages and/or be a criminal offense.\n\n" +
																"Privacy Policy\n\n" +
                                                                "Please read the Mancoba Software Privacy Policy for details of how we use information about you.  " +
                                                                "This policy is available on the home Mancoba Software website: http://www.mancoba.co.za/en/privacy-policy\n\n" +
																"Waiver\n\n" +
                                                                "If you breach these conditions and Mancoba Software takes no action, we are still entitled to use our rights and remedies in the future and where you continue to breach these conditions.\n\n" +
																"Enquiries\n\n" +
																"For all enquiries please contact info@mancoba.co.za");

		public static string TermsAndConditionsText1 = GetString("TermsAndConditionsText1", "Mobile application usage terms and conditions");
		public static string TermsAndConditionsText2 = GetString("TermsAndConditionsText2", "Welcome to the Mancoba (Pty) Ltd ('Mancoba Software') mobile application. If you continue to log in to and use this mobile application, you are agreeing to comply with and be bound by the following terms and conditions of use, which together with our privacy policy govern Mancoba Software' relationship with you in relation to this mobile application.If you do not wish to be bound by these terms and conditions, then you may not access or use any of the content of this mobile application.");

		public static string TermsAndConditionsText3 = GetString("TermsAndConditionsText3", "Mancoba Software is the owner of the mobile application and related website; with its registered office at 232, Forest Drive Ext, Pinelands, Cape Town, 7405, South Africa; the company registration number is 2015/349945/07; place of registration is the Republic of South Africa. The term 'you' refers to the user or viewer of our mobile application.");

		public static string TermsAndConditionsText4 = GetString("TermsAndConditionsText4", "The use of this mobile application is subject to the following terms of use:");
		public static string TermsAndConditionsText5 = GetString("TermsAndConditionsText5", "Disclaimer");
		public static string TermsAndConditionsText6 = GetString("TermsAndConditionsText6", "You agree that your use of this mobile application is for lawful purposes only. You agree that you will not use this mobile application for any unlawful purpose, including committing a criminal offense.");
		public static string TermsAndConditionsText7 = GetString("TermsAndConditionsText7", "The content of the pages of this mobile application is for your general information and use only. It is subject to change without notice.");

		public static string TermsAndConditionsText8 = GetString("TermsAndConditionsText8", "Mancoba Software reserves the right, in its sole discretion and without notice, to interrupt, suspend or terminate this mobile application, and to change the software and/or hardware required to gain access to the website though this application. Mancoba Software will however, where practicable, provide notice of such changes. Mancoba Software cannot provide any warranty or guarantee as to the accuracy, timeliness, performance, completeness or suitability of the information and materials found or offered through this mobile application for any particular purpose.You acknowledge that such information and materials may contain inaccuracies or errors and Mancoba Software expressly exclude liability for any such inaccuracies or errors. ");
		public static string TermsAndConditionsText9 = GetString("TermsAndConditionsText9", "There is no guarantee that you will always be able to access the website through this mobile application. Mancoba Software will not be liable to you for any interruption or delay that you experience in accessing the website, whatever the cause. Mancoba Software will not be liable to you for any interruption or delay that you experience in accessing the website, whatever the cause. Mancoba Software does not warrant that the website or the server or mobile application that makes it available are free of viruses or bugs or other harmful components.");

		public static string TermsAndConditionsText10 = GetString("TermsAndConditionsText10", "Liability");
		public static string TermsAndConditionsText11 = GetString("TermsAndConditionsText11", "Your use of any information or materials on this website is entirely at your own risk, for which Mancoba Software shall not be liable.  The mobile application and all its contents are provided on an 'as is' basis, and Mancoba Software makes no representations or warranties of any kind, whether express or implied, to the accuracy of the contents of the mobile application. It shall be your own responsibility to ensure that any products, services or information available through this website meet your specific requirements.  Mancoba Software does not warrant that the mobile application's functions will be uninterrupted or error-free, or that the site or its server is free from viruses or other harmful components");
		public static string TermsAndConditionsText12 = GetString("TermsAndConditionsText12", "Mancoba Software, its owners, directors, employees, officials, suppliers, agents and/or representatives shall not be liable for any loss or damage, whether direct, indirect or consequential, or any expense of any nature whatsoever, which may be suffered by the user, which arises directly or indirectly from reliance of the mobile application and/or its content.");
		public static string TermsAndConditionsText13 = GetString("TermsAndConditionsText13", "Mancoba Software shall not be responsible for any direct or indirect special consequential or other damage of any kind whatsoever suffered or incurred by you related to your use of, or your inability to access or use, the content or the website or any functionality of the website or of any linked website, even where Mancoba Software is expressly advised thereof.");
		public static string TermsAndConditionsText14 = GetString("TermsAndConditionsText14", "You will indemnify Mancoba Software, its owners, members, employees, officials, agents or representatives, and keep them fully indemnified, from and against any loss or damage suffered or liability incurred in respect of any third party, which arises from your use of this website");
		public static string TermsAndConditionsText15 = GetString("TermsAndConditionsText15", "Unauthorised use of this website may give rise to a claim for damages and/or be a criminal offense.");
		public static string TermsAndConditionsText16 = GetString("TermsAndConditionsText16", "Privacy Policy");
		public static string TermsAndConditionsText17 = GetString("TermsAndConditionsText17", "Please read the Mancoba Software Privacy Policy for details of how we use information about you. This policy is available on the home Mancoba Software website: http://www.mancoba.co.za/en/privacy-policy");
		public static string TermsAndConditionsText18 = GetString("TermsAndConditionsText18", "Waiver");
		public static string TermsAndConditionsText19 = GetString("TermsAndConditionsText19", "If you breach these conditions and Mancoba Software takes no action, we are still entitled to use our rights and remedies in the future and where you continue to breach these conditions.");
		public static string TermsAndConditionsText20 = GetString("TermsAndConditionsText20", "Enquiries");
		public static string TermsAndConditionsText21 = GetString("TermsAndConditionsText21", "For all enquiries please contact info@mancoba.co.za");
		

		/// Slide out menu 
		public static string MenuProviders = GetString("Providers", "Providers");
        public static string MenuSearchInstallers = GetString("SearchInstallers", "Search Installers");
        public static string MenuInstallers = GetString("Installers", "Installers");
        public static string MenuMyFavourites = GetString("MyFavourites", "My Favourites");
        public static string MenuMyWishList = GetString("WishList", "My Wish List");
        public static string MenuMyDetails = GetString("MyDetails", "My Details");
		public static string MenuMyPurchases = GetString("MyPurchases", "My Purchases");
        public static string MenuMyBasket = GetString("MyBasket", "My Basket");
        public static string MenuDrafts = GetString("Drafts", "Drafts");
        public static string MenuOutbox = GetString("Outbox", "Outbox");
        public static string MenuSent = GetString("Sent", "Sent");
        public static string MenuSettings = GetString ("Settings", "Settings");
		public static string MenuLogout = GetString("Logout", "Log out");

        // Menu Headings
		public static string ListOfInstallers = GetString("ListOfInstallers", "List of Installers");
        public static string ListOfProviders = GetString("ListOfProviders", "List of Providers");
		public static string MyWishList = GetString("MyWishList", "My Wish List");
        public static string MyFavourites = GetString("MyFavourites", "My Favourites");
        public static string MyBasket = GetString("MyBasket", "My Basket");
        public static string MyProfile = GetString("MyProfile", "My Profile");
        public static string MyPurchases = GetString("MyPurchases", "My Purchases");
        public static string ProviderDetails = GetString("ProviderDetails", "Provider Details");
        public static string FilterProviders = GetString("FilterProviders", "Filter Providers");
        public static string InstallerDetails = GetString("InstallerDetails", "Installer Details");
        public static string FilterInstallers = GetString("FilterInstallers", "Filter Installers");
        public static string Drafts = GetString("Drafts", "Drafts");
        public static string Outbox = GetString("Outbox", "Outbox");
        public static string Sent = GetString("Sent", "Sent");
        //Connectivity
        public static string NoConnectivity = GetString("NoConnectivity", "No Connectivity");
		public static string NoConnectivityCannotUpdate = GetString("NoConnectivityCannotUpdate", "There is no internet connectivity. Data cannot be updated");
		public static string NoServerReachable = GetString("NoServerReachable", "The sever is not responding. Data cannot be updated");
		public static string NoServerReachableTrips = GetString("NoServerReachableTrips", "The sever is not responding. Only saved trip data will be shown");
		public static string NoServerReachableEvents = GetString("NoServerReachableEvents", "The sever is not responding. Event data cannot be shown");
		public static string NoConnectivityCannotFetch = GetString("NoConnectivityCannotFetch", "The server cannot be reached and no data can be fetched");
		public static string ViaWiFi = GetString("ViaWiFi", "Via WiFi");
		public static string ViaData = GetString("ViaData", "Via Mobile Data");
		public static string InternetConnected = GetString("InternetConnected", "Internet Connected");
		public static string NoInternet = GetString("NoInternet", "No Internet");

		//Address
		public static string Country = GetString("Country", "Country");
		public static string Province = GetString("Province", "Province");
		public static string Town = GetString("Town", "Town");
		public static string Suburb = GetString("Suburb", "Suburb");
		public static string Street = GetString("Street", "Street");
		public static string HouseNumber = GetString("HouseNumber", "House Number");
        public static string PostalCode = GetString("PostalCode", "Postal Code");

        //New
        public static string AccountUnauthorised = GetString ("AccountUnauthorised", "Your account is unauthorised. Please contact your administrator.");

		//Installer Item
		public static string InstallerName = GetString("InstallerName", "Name");
		public static string MobileNumber = GetString("MobileNumber", "Mobile Number");
		public static string PhoneNumber = GetString("PhoneNumber", "Phone Number");
		public static string WebSite = GetString("WebSite", "Web Site");
		public static string EmailAddress = GetString("EmailAddress", "Email Address");
		public static string ContactPerson = GetString("ContactPerson", "Contact Person");
		public static string Address = GetString("Address", "Address");
				
		public static string AccountName = GetString ("AccountName", "Account Name");		
		public static string AdminInformation = GetString ("AdminInformation", "Admin Information");
        public static string CustomerInfor = GetString("CustomerInfor", "Customer Information");
        public static string AddressInfor = GetString("AddressInfor", "Addres Information");

        public static string Config = GetString("Config", "Config Additional Details");
		public static string ContactName = GetString ("ContactName", "Contact Name");
		public static string ContactPhone = GetString ("ContactPhone", "Contact Phone");
				 		
		//Provider Details		
		public static string ProviderInfor = GetString("ProviderInfor", "Provider Infor");
		public static string ContactInfor = GetString("ContactInfor", "Address Infor");
		public static string ProductInfor = GetString("ProductInfor", "Product Infor");
		
		public static string ProviderSummary = GetString("ProviderSummary", "Provider Summary");
        public static string ProductSummary = GetString("ProductSummary", "Product Summary");
        public static string ContactSummary = GetString("ContactSummary", "Contact Summary");
						
		public static string ConfigDetails = GetString ("ConfigDetails", "Config Additional Details");
		public static string AssetId = GetString ("AssetId", "Asset ID");
		public static string MobileDeviceType = GetString ("MobileDeviceType", "Mobile Device Type");
		public static string DeviceSerial = GetString ("DeviceSerial", "Device Serial");
		public static string ConfigurationGroup = GetString ("ConfigurationGroup", "Configuration Group");
		public static string ConfigurationStatus = GetString ("ConfigurationStatus", "Configuration Status");
		public static string DriverManagement = GetString ("DriverManagement", "Driver Management");
		public static string StarterCut = GetString ("StarterCut", "Starter Cut");

		public static string OrderAccepted = GetString ("OrderAccepted", "The order was accepted");
		public static string OrderNotAccepted = GetString ("OrderNotAccepted", "There was a problem accepting this order");

		public static string OrderDeAccepted = GetString ("OrderDeAccepted", "The order(s) were de-accepted");
		public static string OrderNotDeAccepted = GetString ("OrderNotDeAccepted", "There was a problem de-accepting order number {0}");
	}
}

