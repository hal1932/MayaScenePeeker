﻿using System.Windows.Controls;

namespace mayapeeker.Views
{
    /// <summary>
    /// Interaction logic for OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        public OptionControl()
        {
            InitializeComponent();

            var viewModel = new ViewModels.PreviewViewModel();
            _container.DataContext = viewModel;

            Loaded += (sender, e) =>
            {
                viewModel.Initialize();
            };
        }
    }
}
