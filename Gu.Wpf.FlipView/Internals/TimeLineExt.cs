namespace Gu.Wpf.FlipView.Internals
{
    using System;
    using System.Linq;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Extension methods for <see cref="Timeline"/>.
    /// </summary>
    internal static class TimeLineExt
    {
        /// <summary>
        /// Get the total time of the animation.
        /// </summary>
        /// <param name="timeline">The source <see cref="Timeline"/>.</param>
        /// <returns>The total time the animation will run after it is started.</returns>
        internal static TimeSpan GetTimeToFinished(this Timeline timeline)
        {
            if (timeline.Duration.HasTimeSpan)
            {
                var beginTime = timeline.BeginTime ?? TimeSpan.Zero;
                return beginTime + timeline.Duration.TimeSpan;
            }

            if (timeline is Storyboard storyboard)
            {
                if (storyboard.Children.Count == 0)
                {
                    return TimeSpan.Zero;
                }

                return storyboard.Children.Max(x => GetTimeToFinished(x));
            }

            throw new NotSupportedException($"GetTimeToFinished not implemented for: {timeline.GetType().FullName}");
        }
    }
}
