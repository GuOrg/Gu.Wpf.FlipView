namespace Gu.Wpf.FlipView.Demo.ControlDemos
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    public partial class FlipViewDataTemplateView : UserControl
    {
        public FlipViewDataTemplateView()
        {
            this.InitializeComponent();
        }
    }

    public class ViewModel
    {
        public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>
        {
            new Person {FirstName = "Johan", LastName = "Larsson"},
            new Person {FirstName = "Erik", LastName = "Svensson"},
            new Person {FirstName = "Reed", LastName = "Forkmann"},
            new Person {FirstName = "Cat", LastName = "Incremented"}
        };
    }

    public class Person : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                if (value == this.firstName)
                {
                    return;
                }

                this.firstName = value;
                this.OnPropertyChanged();
            }
        }
        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                if (value == this.lastName)
                {
                    return;
                }

                this.lastName = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
