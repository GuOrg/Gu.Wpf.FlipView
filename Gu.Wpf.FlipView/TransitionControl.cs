namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using Gu.Wpf.FlipView.Internals;

    [TemplatePart(Name = PartNewContent, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartOldContent, Type = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "NewContentStyle", StyleTargetType = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "OldContentStyle", StyleTargetType = typeof(ContentPresenter))]
    public class TransitionControl : ContentControl
    {
        public const string PartOldContent = "PART_OldContent";
        public const string PartNewContent = "PART_NewContent";

        public static readonly RoutedEvent ContentChangedEvent = EventManager.RegisterRoutedEvent(
            "ContentChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TransitionControl));

        public static readonly RoutedEvent OldContentChangedEvent = EventManager.RegisterRoutedEvent(
            "OldContentChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TransitionControl));

        public static readonly RoutedEvent NewContentChangedEvent = EventManager.RegisterRoutedEvent(
            "NewContentChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TransitionControl));

        public static readonly DependencyProperty OldContentStyleProperty = DependencyProperty.Register(
            "OldContentStyle",
            typeof(Style),
            typeof(TransitionControl),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty NewContentStyleProperty = DependencyProperty.Register(
            "NewContentStyle",
            typeof(Style),
            typeof(TransitionControl),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty InAnimationProperty = DependencyProperty.Register(
            "InAnimation",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                EmptyStoryboard.Instance,
                OnOldTransitionChanged,
               (_, v) => OnAnimationCoerce(v)));

        public static readonly DependencyProperty OutAnimationProperty = DependencyProperty.Register(
            "OutAnimation",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                EmptyStoryboard.Instance,
                null,
                (_, v) => OnAnimationCoerce(v)));

        private static readonly DependencyPropertyKey OldContentPropertyKey = DependencyProperty.RegisterReadOnly(
            "OldContent",
            typeof(object),
            typeof(TransitionControl),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty OldContentProperty = OldContentPropertyKey.DependencyProperty;

        private readonly DispatcherTimer timer;
        private readonly RoutedEventArgs contentChangedEventArgs;
        private readonly RoutedEventArgs oldContentChangedEventArgs;
        private readonly RoutedEventArgs newContentChangedEventArgs;

        private ContentPresenter oldContentPresenter;
        private ContentPresenter newContentPresenter;

        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
        }

        public TransitionControl()
        {
            this.timer = new DispatcherTimer(
                TimeSpan.Zero,
                DispatcherPriority.Background,
                (_, __) => this.OnOldContentTransitionCompleted(),
                this.Dispatcher);
            this.timer.Stop();
            this.contentChangedEventArgs = new RoutedEventArgs(ContentChangedEvent, this);
            this.oldContentChangedEventArgs = new RoutedEventArgs(OldContentChangedEvent, this);
            this.newContentChangedEventArgs = new RoutedEventArgs(NewContentChangedEvent, this);
        }

        public event RoutedEventHandler ContentChanged
        {
            add => this.AddHandler(ContentChangedEvent, value);
            remove => this.RemoveHandler(ContentChangedEvent, value);
        }

        public event RoutedEventHandler OldContentChanged
        {
            add => this.AddHandler(OldContentChangedEvent, value);
            remove => this.RemoveHandler(OldContentChangedEvent, value);
        }

        public event RoutedEventHandler NewContentChanged
        {
            add => this.AddHandler(NewContentChangedEvent, value);
            remove => this.RemoveHandler(NewContentChangedEvent, value);
        }

        /// <summary>
        /// Gets the content being removed.
        /// </summary>
        public object OldContent
        {
            get => (object)this.GetValue(OldContentProperty);
            protected set => this.SetValue(OldContentPropertyKey, value);
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
        /// Gets or sets the storyboard that controls how old content animates out of view
        /// </summary>
        public Storyboard InAnimation
        {
            get => (Storyboard)this.GetValue(InAnimationProperty);
            set => this.SetValue(InAnimationProperty, value);
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
        /// Gets or sets the storyboard that controls how new content animates into view
        /// </summary>
        public Storyboard OutAnimation
        {
            get => (Storyboard)this.GetValue(OutAnimationProperty);
            set => this.SetValue(OutAnimationProperty, value);
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
                this.RaiseEvent(this.oldContentChangedEventArgs);
                this.oldContentPresenter?.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this.oldContentPresenter));
            }

            if (this.IsLoaded)
            {
                this.RaiseEvent(this.newContentChangedEventArgs);
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

        private static void OnOldTransitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TransitionControl)d).OnOldTransitionChanged(e.NewValue as Storyboard);
        }

        private static object OnAnimationCoerce(object basevalue)
        {
            var storyboard = basevalue as Storyboard;
            if (storyboard == null)
            {
                return EmptyStoryboard.Instance;
            }

            return storyboard;
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
