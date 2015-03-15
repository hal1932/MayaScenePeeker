using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace mayapeeker.Views
{
    class FollowablePopup : Popup
    {
        static FollowablePopup()
        {
            // IsOpen のイベントハンドラをオーバーライド
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FollowablePopup), new FrameworkPropertyMetadata(typeof(FollowablePopup)));
            FollowablePopup.IsOpenProperty.OverrideMetadata(
                typeof(FollowablePopup), new FrameworkPropertyMetadata(IsOpenChanged));
        }

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FollowablePopup;
            if (d == null) return;

            var target = control.PlacementTarget;
            if (target == null) return;

            var window = Window.GetWindow(target);
            var scrollViewer = control.FindAncestor(control, typeof(ScrollViewer)) as ScrollViewer;

            // false -> true
            if (e.NewValue != null && (bool)e.NewValue == true)
            {
                Debug.Assert(e.OldValue == null || (bool)e.OldValue == false);

                if (window != null)
                {
                    window.LocationChanged += control.OnWindowPropertyChanged;
                    window.SizeChanged += control.OnWindowPropertyChanged;
                }
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollChanged += control.OnScrollChanged;
                }
            }

            // true -> false
            if (e.OldValue != null && (bool)e.OldValue == true)
            {
                Debug.Assert(e.NewValue != null && (bool)e.NewValue == false);

                if (window != null)
                {
                    window.LocationChanged -= control.OnWindowPropertyChanged;
                    window.SizeChanged -= control.OnWindowPropertyChanged;
                }
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollChanged -= control.OnScrollChanged;
                }
            }
        }

        private void OnWindowPropertyChanged(object sender, EventArgs e)
        {
            var tmp = HorizontalOffset;
            HorizontalOffset = tmp + 1;// force update HorizontalOffsetProperty
            HorizontalOffset = tmp;
        }

        private void OnScrollChanged(object sender, EventArgs e)
        {
            IsOpen = false;
        }

        private DependencyObject FindAncestor(DependencyObject obj, Type type)
        {
            var parent = obj;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent)) break;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }
    }
}
