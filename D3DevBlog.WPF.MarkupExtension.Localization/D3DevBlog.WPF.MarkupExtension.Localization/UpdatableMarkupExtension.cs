using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace D3DevBlog.WPF.MarkupExtension.Localization
{
    public abstract class UpdatableMarkupExtension : System.Windows.Markup.MarkupExtension
    {
        private object _targetObject;
        private object _targetProperty;
        private object _rootObject;

        protected object TargetObject
        {
            get { return _targetObject; }
        }

        protected object TargetProperty
        {
            get { return _targetProperty; }
        }

        public object RootObject
        {
            get { return _rootObject; }
        }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            IRootObjectProvider rootObject = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;

            if (target != null)
            {
                _targetObject = target.TargetObject;
                _targetProperty = target.TargetProperty;
            }
            if
                (rootObject != null)
            {
                _rootObject = rootObject.RootObject;
            }

            return ProvideValueInternal(serviceProvider);
        }

        protected void UpdateValue(object value)
        {
            if (_targetObject != null)
            {
                if (_targetProperty is DependencyProperty)
                {
                    DependencyObject obj = _targetObject as DependencyObject;
                    DependencyProperty prop = _targetProperty as DependencyProperty;

                    Action updateAction = () => obj.SetValue(prop, value);

                    // Check whether the target object can be accessed from the
                    // current thread, and use Dispatcher.Invoke if it can't

                    if (obj.CheckAccess())
                        updateAction();
                    else
                        obj.Dispatcher.Invoke(updateAction);
                }
                else // _targetProperty is PropertyInfo
                {
                    PropertyInfo prop = _targetProperty as PropertyInfo;
                    prop.SetValue(_targetObject, value, null);
                }
            }
        }

        protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
    }
}
