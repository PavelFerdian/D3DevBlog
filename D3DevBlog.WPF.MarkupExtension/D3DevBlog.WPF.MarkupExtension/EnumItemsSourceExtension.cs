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
    public class EnumItemsSourceExtension : System.Windows.Markup.MarkupExtension
    {
        public event EventHandler<EnumItemsSourceExtensionEventArgs> CreateItem;

        public Type EnumType { set; get; }
        public bool AddEmptyValue { set; get; }
        public string EmptyValueText { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            if (targetProvider.TargetObject is ItemsControl)
            {
                ItemsControl itemsControl = targetProvider.TargetObject as ItemsControl;
                if (itemsControl.ItemTemplate == null && itemsControl.ItemTemplateSelector == null)
                    itemsControl.DisplayMemberPath = "Text";
            }
            if (targetProvider.TargetObject is Selector)
            {
                ((Selector)targetProvider.TargetObject).SelectedValuePath = "Value";
            }

            var result = Enum.GetValues(EnumType).Cast<Enum>().Select(en =>
            {
                EnumItemsSourceExtensionEventArgs args = new EnumItemsSourceExtensionEventArgs(en, GetEnumDescription(en));
                CreateItem?.Invoke(this, args);
                if (args.Cancel)
                    return null;
                return new { Value = (Enum)en, Text = args.Text };
            }).Where(i => i != null).ToList();

            if (AddEmptyValue)
            {
                if (EmptyValueText == null)
                    EmptyValueText = string.Empty;

                EnumItemsSourceExtensionEventArgs args = new EnumItemsSourceExtensionEventArgs(null, EmptyValueText, true);
                CreateItem?.Invoke(this, args);
                if (args.Cancel == false)
                {
                    result.Insert(0, new { Value = (Enum)null, Text = args.Text });
                }
            }
            return result;
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
