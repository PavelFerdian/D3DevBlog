using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.WPF.MarkupExtension
{
    public class EnumItemsSourceExtensionEventArgs : EventArgs
    {
        public bool Cancel { set; get; }
        public string Text { set; get; }
        public Enum Value { private set; get; }
        public bool IsEmpty { private set; get; }

        public EnumItemsSourceExtensionEventArgs(Enum value, string text)
            : this(value, text, false)
        {

        }

        public EnumItemsSourceExtensionEventArgs(Enum value, string text, bool isEmpty)
        {
            Value = value;
            Text = text;
            IsEmpty = isEmpty;
        }
    }
}
