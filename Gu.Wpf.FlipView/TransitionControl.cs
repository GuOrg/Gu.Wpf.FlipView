namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media.Animation;

    using Gu.Wpf.FlipView.Internals;

    [TemplatePart(Name = PART_NewContent, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_OldContent, Type = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "NewContentStyle", StyleTargetType = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "OldContentStyle", StyleTargetType = typeof(ContentPresenter))]
    public class TransitionControl : ContentControl
    {
        public const string PART_OldContent = "PART_OldContent";
        public const string PART_NewContent = "PART_NewContent";

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

        private static readonly DependencyPropertyKey oldContentPropertyKey = DependencyProperty.RegisterReadOnly(
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

        public static readonly DependencyProperty OldContentProperty = oldContentPropertyKey.DependencyProperty;

        private static readonly Storyboard EmptyStoryboard = CreateEmptyStoryboard();
        private readonly AnimationTracker _oldContentAnimationTracker;
        private ContentPresenter _oldContentPresenter;
        private ContentPresenter _newContentPresenter;

        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
        }

        public TransitionControl()
        {
            _oldContentAnimationTracker = new AnimationTracker(null, Dispatcher);
            _oldContentAnimationTracker.Completed += OnOldContentTransitionCompleted;
        }

        public event RoutedEventHandler ContentChanged
        {
            add { AddHandler(ContentChangedEvent, value); }
            remove { RemoveHandler(ContentChangedEvent, value); }
        }

        public event RoutedEventHandler OldContentChanged
        {
            add { AddHandler(OldContentChangedEvent, value); }
            remove { RemoveHandler(OldContentChangedEvent, value); }
        }

        public event RoutedEventHandler NewContentChanged
        {
            add { AddHandler(NewContentChangedEvent, value); }
            remove { RemoveHandler(NewContentChangedEvent, value); }
        }

        public object OldContent
        {
            get { return (object)GetValue(OldContentProperty); }
            protected set { SetValue(oldContentPropertyKey, value); }
        }

        public Style OldContentStyle
        {
            get { return (Style)GetValue(OldContentStyleProperty); }
            set { SetValue(OldContentStyleProperty, value); }
        }

        /// <summary>
        /// Gest or sets the storyboard that controls how old content animates out of view
        /// </summary>
        public Storyboard InAnimation
        {
            get { return (Storyboard)GetValue(InAnimationProperty); }
            set { SetValue(InAnimationProperty, value); }
        }

        public Style NewContentStyle
        {
            get { return (Style)GetValue(NewContentStyleProperty); }
            set { SetValue(NewContentStyleProperty, value); }
        }

        /// <summary>
        /// Gest or sets the storyboard that controls how new content animates into view
        /// </summary>
        public Storyboard OutAnimation
        {
            get { return (Storyboard)GetValue(OutAnimationProperty); }
            set { SetValue(OutAnimationProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            _newContentPresenter = GetTemplateChild(PART_NewContent) as ContentPresenter;
            _oldContentPresenter = GetTemplateChild(PART_OldContent) as ContentPresenter;
            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this));
            if (OldContent != oldContent)
            {
                OldContent = oldContent;
                RaiseEvent(new RoutedEventArgs(OldContentChangedEvent, this));
                if (_oldContentPresenter != null)
                {
                    _oldContentAnimationTracker.Run();
                    _oldContentPresenter.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, _oldContentPresenter));
                }
            }
            if (oldContent != newContent)
            {
                RaiseEvent(new RoutedEventArgs(NewContentChangedEvent, this));
                if (_newContentPresenter != null)
                {
                    _newContentPresenter.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, _newContentPresenter));
                }
                base.OnContentChanged(oldContent, newContent);
            }
        }

        private static void OnOldTransitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var transitionControl = (TransitionControl)d;
            transitionControl._oldContentAnimationTracker.Update((Storyboard)e.NewValue);
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
            OldContent = null;
        }
    }
}
