namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    public class ArgsVm
    {
        private static readonly Type[] Types =
        {
            typeof(Vector),
            typeof(Point),
            typeof(ManipulationDelta),
            typeof(ManipulationVelocities),
            typeof(InputDevice),
            typeof(InertiaExpansionBehavior),
            typeof(InertiaTranslationBehavior),
            typeof(InertiaRotationBehavior),
        };

        private readonly object args;
        private readonly PropertyInfo info;

        private ArgsVm[] children;

        public ArgsVm(object args)
        {
            this.args = args;
            this.Name = this.StringIt(args);
            this.children = args.GetType()
                            .GetProperties()
                            .Where(x => Types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();
        }

        private ArgsVm(object args, PropertyInfo info)
        {
            this.args = args;
            this.info = info;
            this.Name = $"{info.Name}: {this.StringIt(args)}";
        }

        public IEnumerable<ArgsVm> Children
        {
            get
            {
                if (this.children == null && this.args != null && this.info != null && !this.info.PropertyType.IsPrimitive)
                {
                    var propertyInfos = this.args.GetType()
                                                        .GetProperties();
                    this.children = propertyInfos
                                .Where(x => x != null && x.CanRead)
                                .Select(x => new ArgsVm(x.GetValue(this.args), x))
                                .ToArray();
                }

                return this.children;
            }
        }

        public string Name { get; }

        private string StringIt(object o)
        {
            switch (o)
            {
                case null:
                    return "null";
                case double d:
                    return d.ToString("F1", CultureInfo.InvariantCulture);
                case Vector v:
                    return $"({v.X:F1}, {v.Y:F1})";
                case Point p:
                    return $"({p.X:F1}, {p.Y:F1})";
                default:
                    return o.ToString();
            }
        }
    }
}
