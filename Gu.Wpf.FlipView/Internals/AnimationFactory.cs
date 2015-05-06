namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Helper for creating transition animations
    /// </summary>
    internal class AnimationFactory
    {
        public static AnimationFactory Instance
        {
            get
            {
                return new AnimationFactory();
            }
        }

        public Storyboard CreateTranslationStoryBoard(DependencyObject target, double @from, double to, TimeSpan time)
        {
            var story = new Storyboard();
            Storyboard.SetTargetProperty(story, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTarget(story, target);
            var doubleAnimation = CreateAnimation(to, @from, time);
            story.Children.Add(doubleAnimation);
            return story;
        }

        public static DoubleAnimationUsingKeyFrames CreateAnimation(double @from, double to, TimeSpan duration)
        {
            var doubleAnimation = new DoubleAnimationUsingKeyFrames();

            var fromFrame = new EasingDoubleKeyFrame(@from);
            fromFrame.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn };
            fromFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));

            var toFrame = new EasingDoubleKeyFrame(to);
            toFrame.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            toFrame.KeyTime = KeyTime.FromTimeSpan(duration);

            doubleAnimation.KeyFrames.Add(fromFrame);
            doubleAnimation.KeyFrames.Add(toFrame);
            return doubleAnimation;
        }

        public static AnimationTimeline CreateAnimation(double @from, double to, double end, TimeSpan duration)
        {
            var doubleAnimation = new DoubleAnimationUsingKeyFrames();

            var fromFrame = new EasingDoubleKeyFrame(@from);
            fromFrame.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn };
            fromFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));

            var toFrame = new EasingDoubleKeyFrame(to);
            // toFrame.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            toFrame.KeyTime = KeyTime.FromTimeSpan(duration);

            doubleAnimation.KeyFrames.Add(fromFrame);
            doubleAnimation.KeyFrames.Add(toFrame);
            doubleAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(end, duration));
            return doubleAnimation;
        }
    }
}
