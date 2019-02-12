using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.MarkupExtension.Localization
{
    public sealed class LanguageManager : INotifyPropertyChanged
    {
        static LanguageManager _Instance;
        public static LanguageManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LanguageManager();
                return _Instance;
            }
        }

        Languages _Language;
        public Languages Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
                SetLanguage(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Language"));
            }
        }

        public string this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        public event EventHandler LanguageChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        LanguageManager()
        {

        }

        private void SetLanguage(Languages language)
        {
            string name = language.GetEnumDescription();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);

            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        private string Get(string key)
        {
            return Get(key, null);
        }

        public string Get(string key, Assembly assembly)
        {
            Type resourcesType = null;
            if (assembly == null)
            {
                resourcesType = Application.Current.GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name == "Resources");
            }
            else
            {
                resourcesType = assembly.GetTypes().FirstOrDefault(t => t.Name == "Resources");
            }

            var resourceManagerProperty = resourcesType?.GetProperty("ResourceManager", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            System.Resources.ResourceManager resourceManager = resourceManagerProperty?.GetValue(null) as System.Resources.ResourceManager;
            string result = resourceManager?.GetString(key);
            if(result == null && assembly != null)
            {
                result = Get(key);
            }
            return result;
        }
    }
}
