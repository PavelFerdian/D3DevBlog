using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace D3DevBlog.WPF.MarkupExtension
{
    [MarkupExtensionReturnType(typeof(IEnumerable))]
    public class StaticMembersItemsSourceExtension : System.Windows.Markup.MarkupExtension
    {
        public event EventHandler<StaticMembersItemsSourceExtensionEventArgs> CreateItem;

        public Type Type { set; get; }
        public bool AddEmptyValue { set; get; }
        public string EmptyValueText { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            if (targetProvider.TargetObject is ItemsControl)
            {
                ItemsControl itemsControl = targetProvider.TargetObject as ItemsControl;
                DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemTemplateProperty, typeof(ItemsControl)).AddValueChanged(itemsControl, ItemTemplateChanged);
                DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemTemplateSelectorProperty, typeof(ItemsControl)).AddValueChanged(itemsControl, ItemTemplateChanged);
                if (itemsControl.ItemTemplate == null && itemsControl.ItemTemplateSelector == null)
                    itemsControl.DisplayMemberPath = "Text";
            }
            if (targetProvider.TargetObject is Selector)
            {
                ((Selector)targetProvider.TargetObject).SelectedValuePath = "Value";
            }

            var result = Type.GetProperties(BindingFlags.Static | BindingFlags.Public).Select(en =>
            {
                StaticMembersItemsSourceExtensionEventArgs args = new StaticMembersItemsSourceExtensionEventArgs(en, GetPropertyDescription(en));
                CreateItem?.Invoke(this, args);
                if (args.Cancel)
                    return null;
                return new { Value = en.GetValue(null), Text = args.Text };
            }).Where(i => i != null).ToList();

            if (AddEmptyValue)
            {
                if (EmptyValueText == null)
                    EmptyValueText = string.Empty;

                StaticMembersItemsSourceExtensionEventArgs args = new StaticMembersItemsSourceExtensionEventArgs(null, EmptyValueText, true);
                CreateItem?.Invoke(this, args);
                if (args.Cancel == false)
                {
                    result.Insert(0, new { Value = (object)null, Text = args.Text });
                }
            }
            return result;
        }

        private void ItemTemplateChanged(object sender, EventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (itemsControl.DisplayMemberPath == null)
                itemsControl.DisplayMemberPath = null;
            else
                itemsControl.DisplayMemberPath = null;
        }

        private string GetPropertyDescription(PropertyInfo propertyInfo)
        {
            DescriptionAttribute description = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            if (description != null)
                return description.Description;
            return propertyInfo.Name;
        }
    }
}
