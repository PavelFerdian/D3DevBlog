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
    public class ObjectFilter : ValueFilterBase<object>
    {
        public ReferenceFilterModeEnum Mode
        {
            get { return (ReferenceFilterModeEnum)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(ReferenceFilterModeEnum), typeof(ObjectFilter), new PropertyMetadata(new PropertyChangedCallback(OnModeChanged)));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ObjectFilter filter = d as ObjectFilter;
            filter.OnFiltering();
        }

        public bool AcceptNullOrEmpty
        {
            get { return (bool)GetValue(AcceptNullOrEmptyProperty); }
            set { SetValue(AcceptNullOrEmptyProperty, value); }
        }
        public static DependencyProperty AcceptNullOrEmptyProperty = DependencyProperty.Register("AcceptNullOrEmpty", typeof(bool), typeof(ObjectFilter), null);

        public override bool Filter(object originalValue)
        {
            if (FilterValue == null && AcceptNullOrEmpty == false)
                return false;

            if (AcceptNullOrEmpty)
            {
                if (FilterValue == null || (FilterValue is string && string.IsNullOrWhiteSpace(FilterValue?.ToString())))
                    return true;
            }

            object val;
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                val = originalValue;
            }
            else
            {
                PropertyInfo property = originalValue.GetPropertyByPath(PropertyName);
                if (property == null)
                    throw new InvalidOperationException($"Property with name '{PropertyName}' not found in type '{originalValue.GetType()}'");

                val = property.GetValue(originalValue);
            }

            switch (Mode)
            {
                case ReferenceFilterModeEnum.Equal:
                    return val == FilterValue || object.Equals(val, FilterValue);
                case ReferenceFilterModeEnum.NotEqual:
                    return val != FilterValue && !object.Equals(val, FilterValue);
            }
            return false;
        }

        public override void Clear()
        {
            FilterValue = null;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new ObjectFilter();
        }
    }
}
