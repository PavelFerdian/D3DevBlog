using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{
    public abstract class ValueFilterBase<T> : FilterBase
    {
        public T FilterValue
        {
            get { return (T)GetValue(FilterValueProperty); }
            set { SetValue(FilterValueProperty, value); }
        }
        public static DependencyProperty FilterValueProperty = DependencyProperty.Register("FilterValue", typeof(T), typeof(ValueFilterBase<T>), new PropertyMetadata(new PropertyChangedCallback(OnFilterValueChanged)));

        private static void OnFilterValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValueFilterBase<T> filter = d as ValueFilterBase<T>;
            filter.OnFiltering();
        }

        public override bool IsClear()
        {
            return FilterValue == null;
        }
    }
}
