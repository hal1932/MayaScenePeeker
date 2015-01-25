using System.Windows.Controls;

namespace mayapeeker.Views
{
    /// <summary>
    /// Interaction logic for PreviewControl.xaml
    /// </summary>
    public partial class PreviewControl : UserControl
    {
        public PreviewControl()
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
