using System;
using Android.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace Mancoba.Sompisi.Droid.Classes.Elements
{
    public class ActionBarDrawerEventArgs : EventArgs
    {
        public View DrawerView { get; set; }
        public float SlideOffset { get; set; }
        public int NewState { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="e">The <see cref="ActionBarDrawerEventArgs"/> instance containing the event data.</param>
    public delegate void ActionBarDrawerChangedEventHandler(object s, ActionBarDrawerEventArgs e);

    public class MyActionBarDrawerToggle : ActionBarDrawerToggle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyActionBarDrawerToggle"/> class.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="drawerLayout">The drawer layout.</param>
        /// <param name="toolbar">The toolbar.</param>
        /// <param name="openDrawerContentDescRes">The open drawer content desc resource.</param>
        /// <param name="closeDrawerContentDescRes">The close drawer content desc resource.</param>
        public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            //			: base(activity, drawerLayout, drawerImageRes, openDrawerContentDescRes, closeDrawerContentDescRes)
            //		: base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyActionBarDrawerToggle"/> class.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="drawerLayout">The drawer layout.</param>
        /// <param name="openDrawerContentDescRes">The open drawer content desc resource.</param>
        /// <param name="closeDrawerContentDescRes">The close drawer content desc resource.</param>
        public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
        { }

        public event ActionBarDrawerChangedEventHandler DrawerClosed;
        public event ActionBarDrawerChangedEventHandler DrawerOpened;
        public event ActionBarDrawerChangedEventHandler DrawerSlide;
        public event ActionBarDrawerChangedEventHandler DrawerStateChanged;

        /// <summary>
        /// Called when [drawer closed].
        /// </summary>
        /// <param name="drawerView">The drawer view.</param>
        public override void OnDrawerClosed(View drawerView)
        {
            if (null != DrawerClosed)
                DrawerClosed(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerClosed(drawerView);
        }

        /// <summary>
        /// Called when [drawer opened].
        /// </summary>
        /// <param name="drawerView">The drawer view.</param>
        public override void OnDrawerOpened(View drawerView)
        {
            if (null != DrawerOpened)
                DrawerOpened(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerOpened(drawerView);
        }

        /// <summary>
        /// Called when [drawer slide].
        /// </summary>
        /// <param name="drawerView">The drawer view.</param>
        /// <param name="slideOffset">The slide offset.</param>
        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            if (null != DrawerSlide)
                DrawerSlide(this, new ActionBarDrawerEventArgs
                {
                    DrawerView = drawerView,
                    SlideOffset = slideOffset
                });
            base.OnDrawerSlide(drawerView, slideOffset);
        }

        /// <summary>
        /// Called when [drawer state changed].
        /// </summary>
        /// <param name="newState">The new state.</param>
        public override void OnDrawerStateChanged(int newState)
        {
            if (null != DrawerStateChanged)
                DrawerStateChanged(this, new ActionBarDrawerEventArgs
                {
                    NewState = newState
                });
            base.OnDrawerStateChanged(newState);
        }
    }
}