using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{
    public class Sort : Freezable
    {
        public event SortingDelegate Sorting;

        public string PropertyName { set; get; }

        public ListSortDirection? Mode
        {
            get { return (ListSortDirection?)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(ListSortDirection?), typeof(Sort), new PropertyMetadata(new PropertyChangedCallback(OnModeChanged)));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Sort sort = d as Sort;
            sort.OnSorting();
        }

        private void OnSorting()
        {
            Sorting?.Invoke(this);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Sort();
        }
    }
}
