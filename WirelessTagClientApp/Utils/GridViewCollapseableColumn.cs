using System.Windows.Controls;
using System.Windows;

namespace Wpf.Util
{
    /// <summary>
    /// Attached behaviour for collapsible column in a grid view
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/1392811/c-wpf-make-a-gridviewcolumn-visible-false
    /// </remarks>
    public class GridViewCollapsibleColumn
    {
        public static readonly DependencyProperty CollapsibleColumnProperty =
            DependencyProperty.RegisterAttached("CollapsibleColumn",
                                                 typeof(bool),
                                                 typeof(GridViewCollapsibleColumn),
                                                 new UIPropertyMetadata(false, OnCollapsibleColumnChanged));

        public static bool GetCollapsibleColumn(DependencyObject d)
        {
            return (bool)d.GetValue(CollapsibleColumnProperty);
        }

        public static void SetCollapsibleColumn(DependencyObject d, bool value)
        {
            d.SetValue(CollapsibleColumnProperty, value);
        }

        private static void OnCollapsibleColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            header.IsVisibleChanged += new DependencyPropertyChangedEventHandler(AdjustWidth);
        }

        static void AdjustWidth(object sender, DependencyPropertyChangedEventArgs e)
        {
            var header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            if (header.Visibility == Visibility.Collapsed)
            {
                header.Column.Width = 0d;
            }
            else
            {
                header.Column.Width = double.NaN;   // "Auto"
            }
        }
    }
}
