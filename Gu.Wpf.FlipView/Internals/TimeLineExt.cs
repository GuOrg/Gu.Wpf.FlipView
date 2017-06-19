namespace Gu.Wpf.FlipView.Internals
{
    using System;
    using System.Linq;
    using System.Windows.Media.Animation;

    internal static class TimeLineExt
    {
        internal static TimeSpan GetTimeToFinished(this Timeline timeline)
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
    }
}