﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XamarinFormsGestures.Controls;
using XamarinFormsGestures.WinPhone.CustomRenderers;

[assembly: ExportRenderer(typeof(GestureView), typeof(GestureViewRenderer))]
namespace XamarinFormsGestures.WinPhone.CustomRenderers
{
    public class GestureViewRenderer : ViewRenderer<GestureView, Canvas>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GestureView> e)
        {
            base.OnElementChanged(e);

            Canvas winControl;

            if (this.Control == null)
            {
                winControl = new Canvas();
                // note that a background is needed for the gestures to fire
                winControl.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));

                // the content view can have 1 child - move it under our Canvas element
                if (Children.Count > 0)
                {
                    var child = Children[0];
                    Children.Remove(child);
                    winControl.Children.Add(child);
                }
                SetNativeControl(winControl);
            }
            else
            {
                winControl = this.Control;
            }

            if (e.NewElement == null)
            {
                winControl.ManipulationDelta -= winControl_ManipulationDelta;
            }

            if (e.OldElement == null)
            {
                // setup listeners
                winControl.ManipulationDelta += winControl_ManipulationDelta;
            }
        }

        double deltaPercentage = .25;
        void winControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            // Left to Right:   +X
            // Right to Left:   -X
            // Top to Bottom:   +Y
            // Bottom to Top:   -Y

            // get the movement both directionally and absolute val
            var delX = e.CumulativeManipulation.Translation.X;
            var absX = Math.Abs(delX);
            var delY = e.CumulativeManipulation.Translation.Y;
            var absY = Math.Abs(delY);

            // calculate how much change will trigger an action based on the control's size
            var upDownMinDelta = this.ActualHeight * deltaPercentage;
            var leftRightMinDelta = this.ActualWidth * deltaPercentage;

            //System.Diagnostics.Debug.WriteLine("X: {0}, Y: {1}", delX, delY);

            if (absX > absY && absX > leftRightMinDelta)
            {
                if (delX < 0)
                    this.Element.OnSwipeLeft();
                else
                    this.Element.OnSwipeRight();
                e.Complete();
            }
            else if (absY > absX && absY > upDownMinDelta)
            {
                if (delY < 0)
                    this.Element.OnSwipeUp();
                else
                    this.Element.OnSwipeDown();
                e.Complete();
            }
        }
    }

}
