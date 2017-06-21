namespace Gu.Wpf.FlipView.Internals
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    internal static class DependencyObjExtensions
    {
        internal static IEnumerable<DependencyObject> VisualChildrenRecursive(this DependencyObject parent)
        {
            if (parent == null)
            {
                yield break;
            }

            if ((parent is Visual) || (parent is Visual3D))
            {
                var visualChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (var i = 0; i < visualChildrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;

                    foreach (var recursive in VisualChildrenRecursive(child))
                    {
                        yield return recursive;
                    }
                }
            }
        }
    }
}