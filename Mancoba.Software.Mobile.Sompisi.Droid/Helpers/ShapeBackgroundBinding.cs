﻿using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace Mancoba.Sompisi.Droid.Helpers
{
    public class ShapeBackgroundBinding : MvxAndroidTargetBinding
    {
        private readonly LinearLayout _linearLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeBackgroundBinding"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ShapeBackgroundBinding(LinearLayout view) : base(view)
        {
            this._linearLayout = view;
        }

        /// <summary>
        /// Sets the value implementation.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        protected override void SetValueImpl(object target, object value)
        {
            // to do logic
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(object value)
        {
            if ((bool)value)
            {
                _linearLayout.SetBackgroundResource(Resource.Drawable.card_shadow_read);
                _linearLayout.Alpha = 1f;
            }
            else
            {
                _linearLayout.SetBackgroundResource(Resource.Drawable.card_shadow_unread);
                _linearLayout.Alpha = 0.4f;
            }
        }

        public override Type TargetType { get { return typeof(bool); } }

        public override MvxBindingMode DefaultMode { get { return MvxBindingMode.TwoWay; } }
    }
}
