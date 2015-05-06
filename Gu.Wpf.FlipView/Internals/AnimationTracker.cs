namespace Gu.Wpf.FlipView.Internals
{
    using System;
    using System.Linq;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;

    /// <summary>
    /// Can't do animation.Completed += ... due to frozen. Hence this helper class
    /// </summary>
    internal class AnimationTracker : DispatcherTimer
    {
        private readonly Dispatcher _dispatcher;
        private Storyboard _storyboard;
        private DispatcherTimer _timer;

        internal AnimationTracker(Storyboard storyboard, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _storyboard = storyboard;
        }

        public event EventHandler Completed;

        internal void Update(Storyboard storyboard)
        {
            Clear();
            _storyboard = storyboard;
        }

        internal void Clear()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        public void Run()
        {
            if (_storyboard == null)
            {
                return;
            }
            if (_timer != null)
            {
                _timer.Stop();
            }
            _timer = new DispatcherTimer(GetTimeToFinished(_storyboard), DispatcherPriority.DataBind, OnCompleted, _dispatcher);
        }

        protected virtual void OnCompleted()
        {
            var handler = Completed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private static TimeSpan GetTimeToFinished(Timeline timeline)
        {
            if (timeline.Duration.HasTimeSpan)
            {
                var beginTime = timeline.BeginTime ?? TimeSpan.Zero;
                return beginTime + timeline.Duration.TimeSpan;
            }
            var storyboard = timeline as Storyboard;
            if (storyboard != null)
            {
                return storyboard.Children.Max(x => GetTimeToFinished(x));
            }
            throw new NotImplementedException(string.Format("GetTimeToFinished not implemented for: {0}", timeline.GetType().FullName));
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            OnCompleted();
        }
    }
}