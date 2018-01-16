using System.Collections.Generic;
using System.Reflection;
using Android.Widget;
using Mancoba.Sompisi.Core;
using Mancoba.Sompisi.Core.Services.Contracts;
using Mancoba.Sompisi.Droid.Classes.Platform;
using Mancoba.Sompisi.Droid.Classes.Settings;
using Mancoba.Sompisi.Droid.Classes.UserInteraction;
using Mancoba.Sompisi.Droid.Helpers;
using Mancoba.Sompisi.Utils.Helpers.Settings;
using Mancoba.Sompisi.Utils.Helpers.UserInteraction;
using Mancoba.Sompisi.Utils.Interfaces;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

//using Mindscape.Raygun4Net;

namespace Mancoba.Sompisi.Droid
{
    public class Setup : MvxAndroidSetup
    {
        //		PushNotificationManager _notificationManager;
        public static Android.Content.Context AppContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Setup"/> class.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        public Setup(Android.Content.Context applicationContext)
            : base(applicationContext)
        {

        }

        /// <summary>
        /// Creates the application.
        /// </summary>
        /// <returns></returns>
        protected override IMvxApplication CreateApp()
        {
            //RaygunClient.Attach("uJ1tOdoXPyAdlp4/St9KZw==");
            //ErrorHandler.SendToRaygun = exception => RaygunClient.Current.Send(exception);
            //	ApiDataService.MessageHandler = new ModernHttpClient.NativeMessageHandler();
            return new App();
        }

        /// <summary>
        /// Gets the android view assemblies.
        /// </summary>
        /// <value>
        /// The android view assemblies.
        /// </value>
        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(Android.Support.Design.Widget.NavigationView).Assembly,
                                                                typeof(Android.Support.Design.Widget.FloatingActionButton).Assembly,
                                                                typeof(Android.Support.V7.Widget.Toolbar).Assembly,
                                                                typeof(Android.Support.V4.Widget.DrawerLayout).Assembly,
			                                                    //			typeof(Android.Support.V4.View.ViewPager).Assembly,
			                                                    //			typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
		};

        /// <summary>
        /// This is very important to override. The default view presenter does not know how to show fragments!
        /// </summary>
        /// <returns></returns>
        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var mvxFragmentsPresenter = new MvxFragmentsPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
            return mvxFragmentsPresenter;
        }

        /// <summary>
        /// Initializes the first chance.
        /// </summary>
        protected override void InitializeFirstChance()
        {
            Mvx.ConstructAndRegisterSingleton<IApplicationConfigurationService, AndroidConfigurationService>();

            Mvx.ConstructAndRegisterSingleton<IUserInteraction, UserInteraction>();
            Mvx.ConstructAndRegisterSingleton<ISettings, MvxAndroidSettings>();

            Mvx.ConstructAndRegisterSingleton<IPlatformCapabilities, AndroidPlatformCapabilities>();

            base.InitializeFirstChance();
        }

        /// <summary>
        /// Fills the target factories.
        /// </summary>
        /// <param name="registry">The registry.</param>
        protected override void FillTargetFactories(MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxCustomBindingFactory<LinearLayout>("ShapeBackground", (view) => new ShapeBackgroundBinding(view)));
        }
    }
}

