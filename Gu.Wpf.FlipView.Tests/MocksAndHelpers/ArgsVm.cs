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
        private ArgsVm[] _children;
        public ArgsVm(object args)
        {
            _args = args;
            Name = StringIt(args);
            _children = args.GetType()
                            .GetProperties()
                            .Where(x => Types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();
        }

        private ArgsVm(object args, PropertyInfo info)
        {
            _args = args;
            _info = info;
            Name = string.Format("{0}: {1}", info.Name, StringIt(args));
        }

        public IEnumerable<ArgsVm> Children
        {
            get
            {
                if (_children == null && _args != null && _info != null && !_info.PropertyType.IsPrimitive)
                {
                    PropertyInfo[] propertyInfos = _args.GetType()
                                                        .GetProperties();
                    _children = propertyInfos
                                .Where(x => x != null && x.CanRead)
                                .Select(x => new ArgsVm(x.GetValue(_args), x))
                                .ToArray();
                }
                return _children;
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
