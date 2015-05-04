namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    public class TouchGestureTracker : GestureTrackerBase<TouchEventArgs>
    {
        public TouchGestureTracker()
        {
            Patterns = new[]
                                {
                                    new EventPattern(
                                        x => x.PreviewTouchDown += OnStart,
                                        x => x.PreviewTouchDown -= OnStart),
                                    new EventPattern(
                                        x => x.PreviewTouchMove += OnMove,
                                        x => x.PreviewTouchMove -= OnMove),
                                    new EventPattern(
                                        x => x.PreviewTouchUp += OnEnd,
                                        x => x.PreviewTouchUp -= OnEnd),
                                    new EventPattern(x => x.TouchLeave += OnEnd, x => x.TouchLeave -= OnEnd),
                                    new EventPattern(
                                        x =>
                                        x.CommandBindings.Add(
                                            new CommandBinding(NavigationCommands.BrowseForward, OnBrowseForward)),
                                        x =>
                                        x.CommandBindings.Remove(
                                            new CommandBinding(NavigationCommands.BrowseForward, OnBrowseForward))),
                                    new EventPattern(
                                        x =>
                                        x.CommandBindings.Add(
                                            new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack)),
                                        x =>
                                        x.CommandBindings.Remove(
                                            new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack)))
                                };
        }

        private void OnBrowseForward(object sender, ExecutedRoutedEventArgs e)
        {
            IsGesturing = false;
            OnGestured(new Gesture(e));
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            IsGesturing = false;
            OnGestured(new Gesture(e));
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TouchGestureTracker();
        }

        protected override bool TryAddPoint(TouchEventArgs args)
        {
            var inputElement = InputElement;

            if (inputElement == null)
            {
                return false;
            }
            Points.Add(new GesturePoint(args.GetTouchPoint(inputElement).Position, args.Timestamp));
            return true;
        }
    }
}