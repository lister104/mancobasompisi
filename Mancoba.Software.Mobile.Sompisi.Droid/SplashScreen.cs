using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using MvvmCross.Droid.Views;

namespace Mancoba.Sompisi.Droid
{
    [Activity(MainLauncher = true, Icon = "@drawable/ic_launcher", Theme = "@style/SplashTheme", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {

        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            View decorView = Window.DecorView;
            decorView.SystemUiVisibility = StatusBarVisibility.Hidden;

            //  var api = Mvx.Resolve<IMobileDataService>();

            var startupWork = new Task(() =>
            {

                //var prov = api.GetProvinces();
                //var town = api.GetTowns();
                //var sub = api.GetSuburbs();
                //var street = api.GetStreets();
                //    Task.WaitAll(prov, town, sub, street);         
                Task.Delay(5000);  // Simulate a bit of startup work.               
            }, TaskCreationOptions.AttachedToParent);

            Intent intent = this.Intent;

        }

        protected override void OnResume()
        {
            base.OnResume();

            var startupWork = new Task(() =>
            {
                Task.Delay(10000);  // Simulate a bit of startup work.               
            });

            startupWork.Start();
        }
    }
}