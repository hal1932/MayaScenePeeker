﻿using System.Windows;
using System.Windows.Controls;

namespace mayapeeker.Views
{
    /// <summary>
    /// Interaction logic for ShelfControl.xaml
    /// </summary>
    public partial class ShelfControl : UserControl
    {
        public ShelfControl()
        {
            InitializeComponent();

            var viewModel = new ViewModels.ShelfViewModel();
            DataContext = viewModel;

            Loaded += (sender, e) =>
            {
                viewModel.Initialize();

                Window.GetWindow(this).Closing += (sender1, e1) =>
                {
                    viewModel.Dispose();
                };
            };
        }
    }
}
