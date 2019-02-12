using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{
    public class FilterCollection : FreezableCollection<FilterBase>, IList, IList<FilterBase>
    { }
}
