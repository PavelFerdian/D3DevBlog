using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{ 
    public abstract class FilterBase : Freezable
    {
        public event FilteringDelegate Filtering;

        public string PropertyName { set; get; }

        protected void OnFiltering()
        {
            Filtering?.Invoke(this);
        }

        public abstract bool Filter(object originalValue);

        public abstract bool IsClear();
        public abstract void Clear();
    }
}
