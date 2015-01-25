using System.Windows.Controls;

namespace mayapeeker.Views
{
    /// <summary>
    /// Interaction logic for BookmarkControl.xaml
    /// </summary>
    public partial class BookmarkControl : UserControl
    {
        public BookmarkControl()
        {
            InitializeComponent();

            var viewModel = new ViewModels.BookmarkViewModel();
            _container.DataContext = viewModel;

            Loaded += (sender, e) =>
            {
                viewModel.Initialize();
            };
        }
    }
}
