using System.Windows;
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
            _container.DataContext = viewModel;

            Loaded += (sender, e) =>
            {
                viewModel.Initialize();

                Window.GetWindow(this).Closing += (sender1, e1) =>
                {
                    viewModel.Dispose();
                };
            };
        }

        private void PathTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Return) return;
            (_container.DataContext as ViewModels.ShelfViewModel).PushCurrentToHistory();
        }
    }
}
