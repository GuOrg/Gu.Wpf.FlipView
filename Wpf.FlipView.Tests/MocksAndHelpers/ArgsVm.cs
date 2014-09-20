namespace Wpf.FlipView.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    public class ArgsVm
    {
        private static Type[] _types = { typeof(Vector), typeof(Point), typeof(ManipulationDelta), typeof(ManipulationVelocities) };
        private readonly ArgsVm[] _children;
        public ArgsVm(object args)
        {
            Name = StringIt(args);
            _children = args.GetType()
                            .GetProperties()
                            .Where(x => _types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();
        }
        public ArgsVm(object args, PropertyInfo info)
        {
            _children = args.GetType()
                            .GetProperties()
                            .Where(x => _types.Contains(x.PropertyType))
                            .Select(x => new ArgsVm(x.GetValue(args), x))
                            .ToArray();

            Name = _children.Any() ? info.Name : info.Name + StringIt(args);

        }

        public IEnumerable<ArgsVm> Children
        {
            get { return _children; }
        }

        public string Name { get; private set; }

        private string StringIt(object o)
        {
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
