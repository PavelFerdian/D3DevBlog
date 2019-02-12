using D3DevBlog.WPF.Shared;
using D3DevBlog.WPF.Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource.App.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEnumerable<RowItem> _RowItems;
        public IEnumerable<RowItem> RowItems
        {
            get { return _RowItems; }
            set { SetValue<IEnumerable<RowItem>>(ref _RowItems, value); }
        }

        public IList<string> Cities { private set; get; }
        public IList<string> Countries { private set; get; }
        public IList<string> CountryCodes { private set; get; }

        public MainWindowViewModel()
        {
            RowItems = RowItemsProvider.Instance.RowItems;
            Cities = RowItemsProvider.Instance.Cities;
            Countries = RowItemsProvider.Instance.Countries;
            CountryCodes = RowItemsProvider.Instance.CountryCodes;

            Cities.Insert(0, "");
            Countries.Insert(0, "");
            CountryCodes.Insert(0, "");
        }
    }
}
