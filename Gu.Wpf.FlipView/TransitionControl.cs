namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
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

        private static readonly DependencyPropertyKey OldContentPropertyKey = DependencyProperty.RegisterReadOnly(
            "OldContent",
            typeof(object),
            typeof(TransitionControl),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty InAnimationProperty = DependencyProperty.Register(
            "InAnimation",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                CreateEmptyStoryboard(), 
                OnOldTransitionChanged,
                OnAnimationCoerce));

        public static readonly DependencyProperty OutAnimationProperty = DependencyProperty.Register(
            "OutAnimation",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(
                CreateEmptyStoryboard(),
                null,
                OnAnimationCoerce));

        public static readonly DependencyProperty OldContentProperty = OldContentPropertyKey.DependencyProperty;

        private static readonly Storyboard EmptyStoryboard = CreateEmptyStoryboard();
        private readonly AnimationTracker oldContentAnimationTracker;
        private ContentPresenter oldContentPresenter;
        private ContentPresenter newContentPresenter;

        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
        }

        public TransitionControl()
        {
            this.oldContentAnimationTracker = new AnimationTracker(null, this.Dispatcher);
            this.oldContentAnimationTracker.Completed += this.OnOldContentTransitionCompleted;
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

        public object OldContent
        {
            get => (object)this.GetValue(OldContentProperty);
            protected set => this.SetValue(OldContentPropertyKey, value);
        }

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

        public override void OnApplyTemplate()
        {
            this.newContentPresenter = this.GetTemplateChild(PartNewContent) as ContentPresenter;
            this.oldContentPresenter = this.GetTemplateChild(PartOldContent) as ContentPresenter;
            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (this.OldContent != oldContent)
            {
                this.OldContent = oldContent;
                this.RaiseEvent(new RoutedEventArgs(OldContentChangedEvent, this));
                if (this.oldContentPresenter != null)
                {
                    this.oldContentAnimationTracker.Run();
                    this.oldContentPresenter.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this.oldContentPresenter));
                }
            }

            if (oldContent != newContent)
            {
                this.RaiseEvent(new RoutedEventArgs(NewContentChangedEvent, this));
                this.newContentPresenter?.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this.newContentPresenter));
                base.OnContentChanged(oldContent, newContent);
            }
        }

        private static void OnOldTransitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var transitionControl = (TransitionControl)d;
            transitionControl.oldContentAnimationTracker.Update((Storyboard)e.NewValue);
            transitionControl.OldContent = null;
        }

        private static object OnAnimationCoerce(DependencyObject d, object basevalue)
        {
            var storyboard = basevalue as Storyboard;
            if (storyboard == null)
            {
                return EmptyStoryboard;
            }

            return storyboard;
        }

        private static Storyboard CreateEmptyStoryboard()
        {
            var storyboard = new Storyboard { FillBehavior = FillBehavior.Stop };
            if (storyboard.CanFreeze)
            {
                storyboard.Freeze();
            }

            return storyboard;
        }

        private void OnOldContentTransitionCompleted(object sender, EventArgs e)
        {
            this.OldContent = null;
        }
    }
}
