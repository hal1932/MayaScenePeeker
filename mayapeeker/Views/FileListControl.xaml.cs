using System.Windows.Controls;
using System.Windows.Input;

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

            var viewModel = new ViewModels.FileListViewModel();
            _container.DataContext = viewModel;

            Loaded += (sender, e) =>
            {
                viewModel.Initialize();
            };

            _listBox.PreviewKeyUp += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                }
            };
        }
    }
}
