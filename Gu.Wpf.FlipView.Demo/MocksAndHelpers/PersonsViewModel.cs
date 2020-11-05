namespace Gu.Wpf.FlipView.Demo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class PersonsViewModel : INotifyPropertyChanged
    {
        private static readonly IReadOnlyList<Person> StaticPersons = new[]
        {
            new Person { FirstName = "Johan", LastName = "Larsson" },
            new Person { FirstName = "Erik", LastName = "Svensson" },
            new Person { FirstName = "Reed", LastName = "Forkmann" },
            new Person { FirstName = "Cat", LastName = "Incremented" },
        };

        private ObservableCollection<Person>? people;

        public PersonsViewModel()
        {
            this.people = new ObservableCollection<Person>(StaticPersons);
            this.ClearCommand = new RelayCommand(_ => this.People?.Clear(), _ => this.People?.Any() == true);
            this.SetToNullCommand = new RelayCommand(_ => this.People = null, _ => this.People != null);
            this.SetToEmptyCommand = new RelayCommand(_ => this.People = new ObservableCollection<Person>());
            this.ResetCommand = new RelayCommand(_ => this.People = new ObservableCollection<Person>(StaticPersons));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ClearCommand { get; }

        public ICommand SetToNullCommand { get; }

        public ICommand SetToEmptyCommand { get; }

        public ICommand ResetCommand { get; }

        public ObservableCollection<Person>? People
        {
            get => this.people;

            set
            {
                if (ReferenceEquals(value, this.people))
                {
                    return;
                }

                this.people = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
