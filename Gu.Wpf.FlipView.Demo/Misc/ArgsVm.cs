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

        private readonly object? args;
        private readonly PropertyInfo? info;

        private ArgsVm[]? children;

        public ArgsVm(object args)
        {
            this.args = args;
            this.Name = StringIt(args);
            this.children = args.GetType()
                            .GetProperties()
                            .Where(x => Types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();
        }

        private ArgsVm(object? args, PropertyInfo info)
        {
            this.args = args;
            this.info = info;
            this.Name = $"{info.Name}: {StringIt(args)}";
        }

        public string Name { get; }

        public IEnumerable<ArgsVm>? Children
        {
            get
            {
                if (this.children is null &&
                    this.args != null &&
                    this.info != null &&
                    !this.info.PropertyType.IsPrimitive)
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

        private static string StringIt(object? o)
        {
            return o switch
            {
                null => "null",
                double d => d.ToString("F1", CultureInfo.InvariantCulture),
                Vector v => $"({v.X:F1}, {v.Y:F1})",
                Point p => $"({p.X:F1}, {p.Y:F1})",
                _ => o.ToString(),
            };
        }
    }
}
