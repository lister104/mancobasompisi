using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Mancoba.Sompisi.Droid.Classes.Controls
{
    public class FloatingActionButton : FrameLayout, ICheckable
    {
        private static BuildVersionCodes currentapiVersion = Build.VERSION.SdkInt;
        // An array of states
        private int[] CHECKED_STATE_SET = { Android.Resource.Attribute.StateChecked };

        private const string TAG = "FloatingActionButton";

        // A bool that tells if the FAB is checked or not
        protected bool check;


        // The View that is revealed
        protected View revealView;

        // The coordinates of a touch action
        protected Point touchPoint;

        // A GestureDetector to detect touch actions
        private GestureDetector gestureDetector;

        // A listener to communicate that the FAB has changed states
        private IOnCheckedChangeListener onCheckedChangeListener;

        private Context _context;

        public bool Checked
        {
            get { return check; }
            set { check = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingActionButton"/> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        public FloatingActionButton(IntPtr a, Android.Runtime.JniHandleOwnership b) : base(a, b)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingActionButton"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public FloatingActionButton(Context context) : this(context, null, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingActionButton"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        public FloatingActionButton(Context context, IAttributeSet attrs) : this(context, attrs, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingActionButton"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        /// <param name="defStyleAttr">The definition style attribute.</param>
        public FloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingActionButton"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        /// <param name="defStyleAttr">The definition style attribute.</param>
        /// <param name="defStyleRes">The definition style resource.</param>
        public FloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {

            _context = context;
            // When a view is clickable it will change its state to "pressed" on every click.
            Clickable = true;

            // Create a GestureDetector to detect single taps
            gestureDetector = new GestureDetector(context, new MySimpleOnGestureListener(this));

            //A new View is created

            revealView = new View(context);
            if (currentapiVersion < BuildVersionCodes.Lollipop)
            {
                SetBackgroundResource(Resource.Drawable.ripple_oval);
            }
            else
            {
                //				SetBackgroundResource (Resource.Drawable.fab_background);
                AddView(revealView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            }
        }

        private class MySimpleOnGestureListener : GestureDetector.SimpleOnGestureListener
        {
            FloatingActionButton b;
            public MySimpleOnGestureListener(FloatingActionButton bu)
            {
                b = bu;
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
                b.touchPoint = new Point((int)e.GetX(), (int)e.GetY());

                b.Toggle();
                return true;
            }
        }

        // Sets the checked/unchecked state of the FAB.
        /// <summary>
        /// Sets the checked.
        /// </summary>
        public void SetChecked()
        {
            // If trying to change to the current state, ignore
            if (this.check == true)
                return;
            this.check = true;
            
            if (currentapiVersion >= BuildVersionCodes.Lollipop)
            {
                Animator anim = CreateAnimator();
                anim.SetDuration(Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime));
                anim.Start();
                //				revealView.SetBackgroundColor (check ? new Color(Resource.Color.fab_color_2 )
                //					: new Color(Resource.Color.fab_color_1));
                revealView.Visibility = ViewStates.Visible;
            }
            else
            {
                Animation anim = AnimationUtils.LoadAnimation(Context,
                    Resource.Animation.fab_anim2);
                anim.AnimationEnd += (object sender, Animation.AnimationEndEventArgs e) =>
                {
                    this.check = false;
                    DoOnCheckChanged();
                };
                StartAnimation(anim);
            }
        }

        /// <summary>
        /// Sets the on checked change listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void SetOnCheckedChangeListener(IOnCheckedChangeListener listener)
        {
            onCheckedChangeListener = listener;
            Clickable = listener != null;
        }

        /// <summary>
        /// Does the on check changed.
        /// </summary>
        public void DoOnCheckChanged()
        {
            if (onCheckedChangeListener != null)
                onCheckedChangeListener.OnCheckedChanged(this, check);
        }

        // Interface of a callback to be invoked when the checked state of a compound button changes.
        public interface IOnCheckedChangeListener
        {
            void OnCheckedChanged(FloatingActionButton fabView, bool isChecked);
        }

        /// <summary>
        /// Creates the animator.
        /// </summary>
        /// <returns></returns>
        protected Animator CreateAnimator()
        {
            // Calculate the longest distance from the hot spot to the edge of the circle.
            int endRadius = Width / 2 + ((int)Math.Sqrt(Math.Pow(Width / 2 - touchPoint.Y, 2)
                + Math.Pow(Width / 2 - touchPoint.X, 2)));

            // Make sure the touch point is defined or set it to the middle of the view.
            if (touchPoint == null)
                touchPoint = new Point(Width / 2, Height / 2);

            Animator anim = ViewAnimationUtils.CreateCircularReveal(revealView, touchPoint.X, touchPoint.Y, 0, endRadius);
            anim.AddListener(new MyAnimatorListenerAdapter(this));
            return anim;
        }

        private class MyAnimatorListenerAdapter : AnimatorListenerAdapter
        {
            FloatingActionButton b;
            public MyAnimatorListenerAdapter(FloatingActionButton bu)
            {
                b = bu;
            }
            public override void OnAnimationEnd(Animator animation)
            {
                b.RefreshDrawableState();

                b.revealView.Visibility = ViewStates.Gone;
                // Reset the touch point as the next call to setChecked might not come
                // from a tap.
                b.touchPoint = null;

                b.check = false;
                b.DoOnCheckChanged();
            }
        }

        /// <summary>
        /// Implement this method to handle touch screen motion events.
        /// </summary>
        /// <param name="e">The motion event.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Implement this method to handle touch screen motion events.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/view/View.html#onTouchEvent(android.view.MotionEvent)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (gestureDetector.OnTouchEvent(e))
                return true;

            return base.OnTouchEvent(e);
        }

        /// <summary>
        /// Determines whether this instance is checked.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChecked()
        {
            return check;
        }

        /// <summary>
        /// Change the checked state of the view to the inverse of its current state
        /// </summary>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Change the checked state of the view to the inverse of its current state
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/widget/Checkable.html#toggle()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public void Toggle()
        {
            SetChecked();
        }

        /// <summary>
        /// Generate the new <c><see cref="T:Android.Graphics.Drawables.Drawable" /></c> state for
        /// this view.
        /// </summary>
        /// <param name="extraSpace">if non-zero, this is the number of extra entries you
        /// would like in the returned array in which you can place your own
        /// states.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Generate the new <c><see cref="T:Android.Graphics.Drawables.Drawable" /></c> state for
        /// this view. This is called by the view
        /// system when the cached Drawable state is determined to be invalid.  To
        /// retrieve the current state, you should use <c><see cref="M:Android.Views.View.GetDrawableState" /></c>.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/view/View.html#onCreateDrawableState(int)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        /// <altmember cref="M:Android.Views.View.MergeDrawableStates(System.Int32[], System.Int32[])" />
        protected override int[] OnCreateDrawableState(int extraSpace)
        {
            int[] drawableState = base.OnCreateDrawableState(extraSpace + 1);
            if (IsChecked())
                MergeDrawableStates(drawableState, CHECKED_STATE_SET);

            return drawableState;
        }      
    }
}
//		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
//		{
//			base.OnSizeChanged (w, h, oldw, oldh);
////
//			var outline = new Outline ();
//			OutlineProvider = new OutlineProv ();
//			outline.SetOval (0, 0, w, h);
//			OutlineProvider.GetOutline (this, outline);
//			ClipToOutline = true;
//		}
//
//		private class OutlineProv : ViewOutlineProvider
//		{
//			public override void GetOutline (View view, Outline outline)
//			{
//				outline.SetOval (0, 0, view.Width, view.Height);
//			}
//		}
//  