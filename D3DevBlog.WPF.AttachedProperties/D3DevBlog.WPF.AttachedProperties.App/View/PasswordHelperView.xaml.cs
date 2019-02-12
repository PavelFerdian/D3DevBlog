using D3DevBlog.WPF.AttachedProperties.App.ViewModel;
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

namespace D3DevBlog.WPF.AttachedProperties.App.View
{
    /// <summary>
    /// Interaction logic for PasswordHelperView.xaml
    /// </summary>
    public partial class PasswordHelperView : UserControl
    {
        public PasswordHelperView()
        {
            InitializeComponent();

            DataContext = new PasswordHelperViewModel();
        }
    }
}
