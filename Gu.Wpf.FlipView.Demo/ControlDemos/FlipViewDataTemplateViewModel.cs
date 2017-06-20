namespace Gu.Wpf.FlipView.Demo.ControlDemos
{
    using System.Collections.ObjectModel;

    public class FlipViewDataTemplateViewModel
    {
        public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>
                                                              {
                                                                  new Person {FirstName = "Johan", LastName = "Larsson"},
                                                                  new Person {FirstName = "Erik", LastName = "Svensson"},
                                                                  new Person {FirstName = "Reed", LastName = "Forkmann"},
                                                                  new Person {FirstName = "Cat", LastName = "Incremented"}
                                                              };
    }
}