namespace Gu.Wpf.FlipView.Tests.Misc_and_helpers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using NUnit.Framework;

    public class DumpCommands
    {
        [Test, Explicit]
        public void DumpBindings()
        {
            // CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, (sender, args) => Args.Add("Forward")));
            var types = new[]
                            {
                                typeof(ApplicationCommands),
                                typeof(NavigationCommands),
                                typeof(MediaCommands),
                                typeof(ComponentCommands)
                            };
            foreach (var type in types)
            {
                PropertyInfo[] propertyInfos = type.GetProperties().Where(x => x.PropertyType.IsSubclassOf(typeof(RoutedCommand))).ToArray();
                foreach (var propertyInfo in propertyInfos)
                {
                    Console.WriteLine(@"CommandBindings.Add(new CommandBinding({0}.{1}, (sender, args) => Args.Add(""{1}"")));", propertyInfo.DeclaringType.Name, propertyInfo.Name);

                }
            }
        }
    }
}
