namespace Gu.Wpf.FlipView.Demo.ControlDemos
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    public class TransitionItem : INotifyPropertyChanged
    {
        private SolidColorBrush brush;
        private string text;

        public TransitionItem()
        {
        }

        public TransitionItem(SolidColorBrush brush, string text)
        {
            this.Brush = brush;
            this.text = text;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SolidColorBrush Brush
        {
            get => this.brush;

            set
            {
                if (ReferenceEquals(value, this.brush))
                {
                    return;
                }

                this.brush = value;
                this.OnPropertyChanged();
            }
        }

        public string Text
        {
            get => this.text;

            set
            {
                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
