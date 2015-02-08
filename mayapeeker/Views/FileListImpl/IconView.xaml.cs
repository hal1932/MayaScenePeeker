using mayapeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mayapeeker.Views.FileListImpl
{
    /// <summary>
    /// Interaction logic for IconView.xaml
    /// </summary>
    public partial class IconView : UserControl
    {
        public IconView()
        {
            InitializeComponent();

            _listBox.PreviewKeyUp += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                }
            };
        }
    }
}
