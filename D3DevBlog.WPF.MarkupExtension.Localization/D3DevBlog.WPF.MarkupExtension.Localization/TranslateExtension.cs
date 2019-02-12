using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace D3DevBlog.WPF.MarkupExtension.Localization
{
    public class TranslateExtension : UpdatableMarkupExtension
    {
        [ConstructorArgument("key")]
        public string Key { set; get; }

        public TranslateExtension(string key)
        {
            Key = key;
            LanguageManager.Instance.LanguageChanged += LanguageChanged;
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            UpdateValue(GetByKey());
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            return GetByKey();
        }

        private string GetByKey()
        {
            Assembly assembly = RootObject.GetType().Assembly;
            return LanguageManager.Instance.Get(Key, assembly);
        }
    }
}
