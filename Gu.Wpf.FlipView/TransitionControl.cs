namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using Gu.Wpf.FlipView.Internals;

    /// <summary>
    /// A <see cref="ContentControl"/> that animates transitions when content changes.
    /// </summary>
    [TemplatePart(Name = PartNewContent, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartOldContent, Type = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = nameof(NewContentStyle), StyleTargetType = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = nameof(OldContentStyle), StyleTargetType = typeof(ContentPresenter))]
    public class TransitionControl : ContentControl
    {
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1202 // Elements must be ordered by access

        public const string PartOldContent = "PART_OldContent";
        public const string PartNewContent = "PART_NewContent";

        /// <summary>Identifies the <see cref="ContentChanged"/> routed event.</summary>
        public static readonly RoutedEvent ContentChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ContentChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TransitionControl));

        /// <summary>Identifies the <see cref="OldContentStyle"/> dependency property.</summary>
        public static readonly DependencyProperty OldContentStyleProperty = DependencyProperty.Register(
            nameof(OldContentStyle),
            typeof(Style),
            typeof(TransitionControl),
            new PropertyMetadata(default(Style)));

        /// <summary>Identifies the <see cref="OutAnimation"/> dependency property.</summary>
        public static readonly DependencyProperty OutAnimationProperty = DependencyProperty.Register(
            nameof(OutAnimation),
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                EmptyStoryboard.Instance,
                OnOutAnimationChanged,
                (_, v) => OnAnimationCoerce(v)));

        /// <summary>Identifies the <see cref="NewContentStyle"/> dependency property.</summary>
        public static readonly DependencyProperty NewContentStyleProperty = DependencyProperty.Register(
            nameof(NewContentStyle),
            typeof(Style),
            typeof(TransitionControl),
            new PropertyMetadata(default(Style)));

        /// <summary>Identifies the <see cref="InAnimation"/> dependency property.</summary>
        public static readonly DependencyProperty InAnimationProperty = DependencyProperty.Register(
            nameof(InAnimation),
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                EmptyStoryboard.Instance,
                null,
               (_, v) => OnAnimationCoerce(v)));

        private static readonly DependencyPropertyKey OldContentPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(OldContent),
            typeof(object),
            typeof(TransitionControl),
            new PropertyMetadata(default(object)));

        /// <summary>Identifies the <see cref="OldContent"/> dependency property.</summary>
        public static readonly DependencyProperty OldContentProperty = OldContentPropertyKey.DependencyProperty;
#pragma warning restore SA1202 // Elements must be ordered by access
#pragma warning restore SA1600 // Elements must be documented

        private readonly DispatcherTimer timer;
        private readonly RoutedEventArgs contentChangedEventArgs;

        private ContentPresenter oldContentPresenter;
        private ContentPresenter newContentPresenter;

        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionControl"/> class.
        /// </summary>
        public TransitionControl()
        {
            this.timer = new DispatcherTimer(
                TimeSpan.Zero,
                DispatcherPriority.Background,
                (_, __) => this.OnOldContentTransitionCompleted(),
                this.Dispatcher);
            this.timer.Stop();
            this.contentChangedEventArgs = new RoutedEventArgs(ContentChangedEvent, this);
        }

        /// <summary>
        /// Notifies when content changes.
        /// </summary>
        public event RoutedEventHandler ContentChanged
        {
            add => this.AddHandler(ContentChangedEvent, value);
            remove => this.RemoveHandler(ContentChangedEvent, value);
        }

        /// <summary>
        /// Gets the content being removed.
        /// This will be set to null when the animation finishes.
        /// </summary>
        public object OldContent
        {
            get => this.GetValue(OldContentProperty);
            private set => this.SetValue(OldContentPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the style for the old content presenter.
        /// </summary>
        public Style OldContentStyle
        {
            get => (Style)this.GetValue(OldContentStyleProperty);
            set => this.SetValue(OldContentStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the storyboard that controls how new content animates into view
        /// </summary>
        public Storyboard OutAnimation
        {
            get => (Storyboard)this.GetValue(OutAnimationProperty);
            set => this.SetValue(OutAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets the style for the new content presenter.
        /// </summary>
        public Style NewContentStyle
        {
            get => (Style)this.GetValue(NewContentStyleProperty);
            set => this.SetValue(NewContentStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the storyboard that controls how old content animates out of view
        /// </summary>
        public Storyboard InAnimation
        {
            get => (Storyboard)this.GetValue(InAnimationProperty);
            set => this.SetValue(InAnimationProperty, value);
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            this.newContentPresenter = this.GetTemplateChild(PartNewContent) as ContentPresenter;
            this.oldContentPresenter = this.GetTemplateChild(PartOldContent) as ContentPresenter;
            base.OnApplyTemplate();
        }

        /// <inheritdoc />
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (!ReferenceEquals(this.OldContent, oldContent))
            {
                this.OldContent = oldContent;
                this.oldContentPresenter?.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this.oldContentPresenter));
            }

            if (this.IsLoaded)
            {
                this.newContentPresenter?.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this.newContentPresenter));
                this.RaiseEvent(this.contentChangedEventArgs);
                if (ReferenceEquals(this.OutAnimation, EmptyStoryboard.Instance) ||
                    this.timer == null ||
                    this.timer.Interval == TimeSpan.Zero)
                {
                    this.OldContent = null;
                }
                else
                {
                    // We can't subscribe to .Completed for the storyboard as it might be frozen.
                    // Hacking it like this instead.
                    this.timer.Start();
                }
            }
        }

        /// <summary>
        /// Called when the animation for the old value changes.
        /// </summary>
        /// <param name="newAnimation">The storyboard for animating a value out of the view.</param>
        protected virtual void OnOldTransitionChanged(Storyboard newAnimation)
        {
            // We can't subscribe to .Completed for the storyboard as it might be frozen.
            // Hacking it like this instead.
            this.timer.Interval = newAnimation?.GetTimeToFinished() ?? TimeSpan.Zero;
            base.OnContentChanged(this.OldContent, null);
            this.OldContent = null;
        }

        /// <summary>
        /// Called when the animation finishes for the old content.
        /// </summary>
        protected virtual void OnOldContentTransitionCompleted()
        {
            base.OnContentChanged(this.OldContent, null);
            this.timer.Stop();
            this.OldContent = null;
        }

        private static void OnOutAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TransitionControl)d).OnOldTransitionChanged(e.NewValue as Storyboard);
        }

        private static object OnAnimationCoerce(object basevalue)
        {
            if (basevalue is Storyboard storyboard)
            {
                return storyboard;
            }

            return EmptyStoryboard.Instance;
        }

        private static class EmptyStoryboard
        {
            internal static readonly Storyboard Instance = CreateEmptyStoryboard();

            private static Storyboard CreateEmptyStoryboard()
            {
                var storyboard = new Storyboard { FillBehavior = FillBehavior.Stop };
                if (storyboard.CanFreeze)
                {
                    storyboard.Freeze();
                }

                return storyboard;
            }
        }
    }
}
