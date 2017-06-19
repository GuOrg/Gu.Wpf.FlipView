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
        private readonly Dispatcher dispatcher;
        private Storyboard storyboard;
        private DispatcherTimer timer;

        internal AnimationTracker(Storyboard storyboard, Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.storyboard = storyboard;
        }

        public event EventHandler Completed;

        internal void Update(Storyboard @new)
        {
            this.Clear();
            this.storyboard = @new;
        }

        internal void Clear()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer = null;
            }
        }

        public void Run()
        {
            if (this.storyboard == null)
            {
                return;
            }

            this.timer?.Stop();
            this.timer = new DispatcherTimer(GetTimeToFinished(this.storyboard), DispatcherPriority.Render, this.OnCompleted, this.dispatcher);
        }

        protected virtual void OnCompleted()
        {
            this.Completed?.Invoke(this, EventArgs.Empty);
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
                if (storyboard.Children.Count == 0)
                {
                    return TimeSpan.Zero;
                }

                return storyboard.Children.Max(x => GetTimeToFinished(x));
            }

            throw new NotImplementedException($"GetTimeToFinished not implemented for: {timeline.GetType().FullName}");
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            this.OnCompleted();
        }
    }
}