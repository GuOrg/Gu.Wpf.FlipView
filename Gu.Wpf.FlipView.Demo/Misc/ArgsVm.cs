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
        private readonly object args;
        private readonly PropertyInfo info;

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
            this.Name = string.Format("{0}: {1}", info.Name, this.StringIt(args));
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
                return string.Format("({0:F1}, {1:F1})", v.Value.X, v.Value.Y);
            }

            var p = o as Point?;
            if (p != null)
            {
                return string.Format("({0:F1}, {1:F1})", p.Value.X, p.Value.Y);
            }

            return o.ToString();
        }
    }
}
