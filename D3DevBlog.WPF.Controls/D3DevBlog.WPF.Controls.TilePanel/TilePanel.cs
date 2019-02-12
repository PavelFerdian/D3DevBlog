using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DevBlog.WPF.Controls.TilePanel
{
    public class TilePanel : System.Windows.Controls.Panel
    {
        public TilePanel()
        {

        }

        protected override Size ArrangeOverride(System.Windows.Size finalSize)
        {
            if (Children == null || Children.Count == 0) return base.ArrangeOverride(finalSize);
            double x = 0, y = 0, colWidth = 0, rowHeight = 0;
            int col = 0;
            colWidth = Children.Cast<UIElement>().Select(c => c.DesiredSize.Width).Max();

            foreach (UIElement child in Children)
            {
                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);

                if (x + child.DesiredSize.Width > (colWidth * (col + 1)))
                {
                    // New row
                    y += rowHeight;
                    x = (colWidth * (col));
                    rowHeight = child.DesiredSize.Height;
                }

                if (y + rowHeight > finalSize.Height)
                {
                    // New column
                    col++;
                    x = (colWidth * (col));
                    y = 0;
                }

                child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
                x += child.DesiredSize.Width;
            }
            MinWidth = x;
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children == null || Children.Count == 0) return base.MeasureOverride(availableSize);
            double x = 0, y = 0, colWidth = 0;

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                if (x + child.DesiredSize.Height > availableSize.Height)
                {
                    x += colWidth;
                    y = 0;
                    colWidth = 0;
                }

                y += child.DesiredSize.Height;
                if (child.DesiredSize.Width > colWidth)
                {
                    colWidth = child.DesiredSize.Width;
                }
            }
            x += colWidth;

            var resultSize = new Size();

            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ? x : availableSize.Width;
            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ? y : availableSize.Height;

            return resultSize;
        }
    }
}
