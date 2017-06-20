namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System;
    using System.Collections.Generic;
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
            typeof(InertiaRotationBehavior)
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
                    PropertyInfo[] propertyInfos = this.args.GetType()
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
            if (o == null)
            {
                return "null";
            }

            var d = o as double?;
            if (d != null)
            {
                return d.Value.ToString("F1");
            }

            var v = o as Vector?;
            if (v != null)
            {
                return $"({v.Value.X:F1}, {v.Value.Y:F1})";
            }

            var p = o as Point?;
            if (p != null)
            {
                return $"({p.Value.X:F1}, {p.Value.Y:F1})";
            }

            return o.ToString();
        }
    }
}
