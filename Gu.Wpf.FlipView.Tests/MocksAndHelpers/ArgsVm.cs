namespace Gu.Wpf.FlipView.Tests.MocksAndHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    public class ArgsVm
    {
        private readonly object _args;
        private readonly PropertyInfo _info;

        private static readonly Type[] _types =
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
        private ArgsVm[] _children;
        public ArgsVm(object args)
        {
            this._args = args;
            this.Name = this.StringIt(args);
            this._children = args.GetType()
                            .GetProperties()
                            .Where(x => _types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();
        }

        private ArgsVm(object args, PropertyInfo info)
        {
            this._args = args;
            this._info = info;
            this.Name = string.Format("{0}: {1}", info.Name, this.StringIt(args));
        }

        public IEnumerable<ArgsVm> Children
        {
            get
            {
                if (this._children == null && this._args != null && this._info != null && !this._info.PropertyType.IsPrimitive)
                {
                    PropertyInfo[] propertyInfos = this._args.GetType()
                                                        .GetProperties();
                    this._children = propertyInfos
                                .Where(x => x != null && x.CanRead)
                                .Select(x => new ArgsVm(x.GetValue(this._args), x))
                                .ToArray();
                }
                return this._children;
            }
        }

        public string Name { get; private set; }

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
