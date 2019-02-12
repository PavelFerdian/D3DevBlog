using D3DevBlog.WPF.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.WPF.AttachedProperties.App.ViewModel
{
    public class PasswordHelperViewModel : ViewModelBase
    {
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetValue<string>(ref _Password, value); }
        }

    }
}
