using D3DevBlog.WPF.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{
    public class StringFilter : ValueFilterBase<string>
    {
        public StringFilterModeEnum Mode
        {
            get { return (StringFilterModeEnum)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(StringFilterModeEnum), typeof(StringFilter), new PropertyMetadata(new PropertyChangedCallback(OnModeChanged)));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StringFilter filter = d as StringFilter;
            filter.OnFiltering();
        }

        public override bool Filter(object originalValue)
        {
            if (string.IsNullOrWhiteSpace(FilterValue))
                return true;

            string val = null;
            if (string.IsNullOrWhiteSpace(PropertyName) && originalValue is string)
            {
                val = originalValue.ToString();
            }
            else
            {
                PropertyInfo property = originalValue.GetPropertyByPath(PropertyName);
                if (property == null)
                    throw new InvalidOperationException($"Property with name '{PropertyName}' not found in type '{originalValue.GetType()}'");
                if (property.PropertyType != typeof(string))
                    throw new InvalidOperationException($"Property with name '{PropertyName}' in type '{originalValue.GetType()}' is not string");

                val = (string)property.GetValue(originalValue);
            }

            switch (Mode)
            {
                case StringFilterModeEnum.Equal:
                    return FilterValue.ToUpper() == val.ToUpper();
                case StringFilterModeEnum.NotEqual:
                    return FilterValue.ToUpper() != val.ToUpper();
                case StringFilterModeEnum.Contains:
                    return val.ToUpper().Contains(FilterValue.ToUpper());
                case StringFilterModeEnum.NotContains:
                    return !val.ToUpper().Contains(FilterValue.ToUpper());
                default:
                    return false;
            }
        }

        public override void Clear()
        {
            FilterValue = null;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new StringFilter();
        }
    }
}
