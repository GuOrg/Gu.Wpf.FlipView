namespace WPF.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class TouchGestureFinder : GestureFinderBase<TouchEventArgs>
    {
        public TouchGestureFinder()
        {
            this.Patterns = new[]
                                {
                                    new EventPattern(
                                        x => x.TouchDown += this.OnStart,
                                        x => x.TouchDown -= this.OnStart),
                                    new EventPattern(
                                        x => x.TouchMove += this.OnMove,
                                        x => x.TouchMove -= this.OnMove),
                                    new EventPattern(
                                        x => x.TouchUp += this.OnEnd,
                                        x => x.TouchUp -= this.OnEnd),
                                    new EventPattern(
                                        x => x.TouchLeave += this.OnEnd,
                                        x => x.TouchLeave -= this.OnEnd),
                                    new EventPattern(x=> x.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, OnBrowseForward)),
                                                     x=> x.CommandBindings.Remove(new CommandBinding(NavigationCommands.BrowseForward, OnBrowseForward))),
                                    new EventPattern(x=> x.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack)),
                                                     x=> x.CommandBindings.Remove(new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack))) 
                                };
        }

        private void OnBrowseForward(object sender, ExecutedRoutedEventArgs e)
        {
            this.OnGestured(new Gesture(e));
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            this.OnGestured(new Gesture(e));
        }

        protected override Freezable CreateInstanceCore()
        {
            return new ManipulationGestureFinder();
        }

        protected override bool TryAddPoint(TouchEventArgs args)
        {
            var inputElement = this.InputElement;

            if (inputElement == null)
            {
                return false;
            }
            this.Points.Add(new GesturePoint(args.GetTouchPoint(inputElement).Position, args.Timestamp));
            return true;
        }
    }
}