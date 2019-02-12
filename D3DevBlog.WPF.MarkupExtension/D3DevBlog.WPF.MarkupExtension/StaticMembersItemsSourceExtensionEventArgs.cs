using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.WPF.MarkupExtension
{
    public class StaticMembersItemsSourceExtensionEventArgs : EventArgs
    {
        public bool Cancel { set; get; }
        public string Text { set; get; }
        public object Value { private set; get; }
        public bool IsEmpty { private set; get; }

        public StaticMembersItemsSourceExtensionEventArgs(object value, string text)
            : this(value, text, false)
        {

        }

        public StaticMembersItemsSourceExtensionEventArgs(object value, string text, bool isEmpty)
        {
            Value = value;
            Text = text;
            IsEmpty = isEmpty;
        }
    }
}
