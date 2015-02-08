using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mayapeeker.Behaviors
{
    class MouseDoubleClickBehavior
    {
        // Using a DependencyProperty as the backing store for CanRaise.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanRaiseProperty =
            DependencyProperty.Register(
                "CanRaise", typeof(bool),
                typeof(MouseDoubleClickBehavior),
                new PropertyMetadata(OnCanRaisePropertyChanged));


        private static void OnCanRaisePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;

            var element = d as FrameworkElement;
            if (element == null) return;

            element.PreviewMouseDown += element_PreviewMouseDown;
            element.Unloaded += (sender, e1) => element.PreviewMouseDown -= element_PreviewMouseDown;
        }


        static void element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                ((FrameworkElement)sender).RaiseEvent(
                    new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                    {
                        RoutedEvent = Control.MouseDoubleClickEvent,
                        Source = sender,
                    });
            }
        }
        
    }
}
