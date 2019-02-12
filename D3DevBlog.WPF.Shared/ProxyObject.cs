using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Shared
{
    public class ProxyObject : Freezable
    {
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(ProxyObject), null);

        protected override Freezable CreateInstanceCore()
        {
            return new ProxyObject();
        }
    }
}
