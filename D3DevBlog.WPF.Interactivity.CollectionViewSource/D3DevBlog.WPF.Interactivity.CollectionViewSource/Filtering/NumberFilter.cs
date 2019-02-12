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
    public class NumberFilter : ValueFilterBase<double?>
    {
        public NumberFilterModeEnum Mode
        {
            get { return (NumberFilterModeEnum)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(NumberFilterModeEnum), typeof(NumberFilter), new PropertyMetadata(new PropertyChangedCallback(OnModeChanged)));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberFilter filter = d as NumberFilter;
            filter.OnFiltering();
        }

        public override bool Filter(object originalValue)
        {
            if (FilterValue == null)
                return true;

            double val;
            if (string.IsNullOrWhiteSpace(PropertyName) && originalValue.IsNumericType())
            {
                val = (double)originalValue;
            }
            else
            {
                PropertyInfo property = originalValue.GetPropertyByPath(PropertyName);
                if (property == null)
                    throw new InvalidOperationException($"Property with name '{PropertyName}' not found in type '{originalValue.GetType()}'");
                if (property.PropertyType.IsNumericType() == false)
                    throw new InvalidOperationException($"Property with name '{PropertyName}' in type '{originalValue.GetType()}' is not numeric");

                object obj = property.GetValue(originalValue);
                if (obj == null)
                    return false;
                double.TryParse(obj.ToString(), out val);
            }

            switch (Mode)
            {
                case NumberFilterModeEnum.Equal:
                    return FilterValue == val;
                case NumberFilterModeEnum.NotEqual:
                    return FilterValue != val;
                case NumberFilterModeEnum.GreaterThan:
                    return val > FilterValue;
                case NumberFilterModeEnum.GreaterThanOrEqual:
                    return val >= FilterValue;
                case NumberFilterModeEnum.LessThan:
                    return val < FilterValue;
                case NumberFilterModeEnum.LessThanOrEqual:
                    return val <= FilterValue;
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
            return new NumberFilter();
        }
    }
}
