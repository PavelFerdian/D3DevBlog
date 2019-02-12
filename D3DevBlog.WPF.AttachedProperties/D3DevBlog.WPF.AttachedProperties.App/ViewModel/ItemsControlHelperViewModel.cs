using D3DevBlog.WPF.Shared;
using D3DevBlog.WPF.Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D3DevBlog.WPF.AttachedProperties.App.ViewModel
{
    public class ItemsControlHelperViewModel : ViewModelBase
    {
        private ObservableCollection<RowItem> _Items;
        public ObservableCollection<RowItem> Items
        {
            get { return _Items; }
            set { SetValue<ObservableCollection<RowItem>>(ref _Items, value); }
        }

        public ItemsControlHelperViewModel()
        {
            Items = new ObservableCollection<RowItem>();
        }

        public ICommand AddCommand { private set; get; }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            AddCommand = new RelayCommand(AddCommandExecute);
        }

        private void AddCommandExecute()
        {
            Items.Add(RowItemsProvider.Instance.GetRandomRowItem());
        }
    }
}
