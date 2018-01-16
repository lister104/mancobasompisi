//using System.Net.Http;

namespace Bluescore.DStv.Core.Services
{
	//public static class ServerUrls
	//{
	//	private static ObservableCollection<KeyValuePair<string, string>> _collection;

	//	public static ObservableCollection<KeyValuePair<string, string>> Collection
	//	{
	//		get
	//		{
	//			if (_collection != null)
	//				return _collection;

	//			_collection = new ObservableCollection<KeyValuePair<string, string>>
	//			{
	//				new KeyValuePair<string, string>("Select Server", null),
	//				new KeyValuePair<string, string>("ent.mixtelematics.com", "https://ent.mixtelematics.com"),
	//				new KeyValuePair<string, string>("uk.mixtelematics.com", "https://uk.mixtelematics.com"),
	//				new KeyValuePair<string, string>("us.mixtelematics.com", "https://us.mixtelematics.com"),
	//				new KeyValuePair<string, string>("za.mixtelematics.com", "https://za.mixtelematics.com"),
	//				new KeyValuePair<string, string>("au.mixtelematics.com", "https://au.mixtelematics.com"),
	//				new KeyValuePair<string, string>("om.mixtelematics.com", "https://om.mixtelematics.com")
	//			};

	//			//var dataContext = Mvx.Resolve<IDataContext>();
	//			//if (dataContext.IsConnected())
	//			//{
	//				try
	//				{
	//					var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
	//					Task.Run (async () => {
	//						//var json = client.GetStringAsync("http://api.fm-web.co.za/MiXFleet/ServerUrls.json").Result
	//						var json = await client.GetStringAsync ("http://api.fm-web.co.za/MiXFleet/ServerUrls.json");
	//						var serverUrls = JsonConvert.DeserializeObject<ServerUrl []> (json);
	//						_collection.Clear ();

	//						_collection.Add (new KeyValuePair<string, string> ("Select Server", null));
	//						foreach (var serverUrl in serverUrls) {
	//							_collection.Add (new KeyValuePair<string, string> (serverUrl.Name, serverUrl.Url));
	//						}
	//						_collection.Add (new KeyValuePair<string, string> ("Config Internal", "http://10.34.5.39"));
	//						_collection.Add (new KeyValuePair<string, string> ("ZAP Dev", "http://dsstbzap01"));
	//						_collection.Add (new KeyValuePair<string, string> ("ZAP Dev IP", "http://10.34.5.167"));
	//						_collection.Add (new KeyValuePair<string, string> ("Amandas Server", "http://afstbws093"));
	//						_collection.Add (new KeyValuePair<string, string> ("Amandas Server ip", "http://10.34.202.234"));
	//						//						_collection.Add(new KeyValuePair<string, string>("Michael Server", "http://afstbws427"));
	//						_collection.Add (new KeyValuePair<string, string> ("Michael Server ip", "http://10.34.202.218"));
	//						_collection.Add (new KeyValuePair<string, string> ("Timezones API", "http://dsstbuui01"));
	//						_collection.Add (new KeyValuePair<string, string> ("Timezones API ip", "http://10.34.5.11"));
	//						////						_collection.Add(new KeyValuePair<string, string>("Karabo Svr ip", "http://10.34.50.76"));
	//						////						_collection.Add(new KeyValuePair<string, string>("Karabo Server", "http://AFSTBIMAC07"));
	//						//
	//						//						_collection.Add(new KeyValuePair<string, string>("Sala Server ip", "http://10.34.203.2"));
	//						////						_collection.Add(new KeyValuePair<string, string>("Sala Server ip2", "http://10.34.201.146"));
	//						////						_collection.Add(new KeyValuePair<string, string>("config UAT ", "http://config.dev.mixtelematics.com"));
	//					});
	//				}
	//				catch (Exception ex)
	//				{
	//				throw ex;
	//					//ErrorHandler.HandleError("Failed to retrieve server URLs!", ex);
	//				}
	//			//}

	//			return _collection;
	//		}
	//	}
	//}
}

