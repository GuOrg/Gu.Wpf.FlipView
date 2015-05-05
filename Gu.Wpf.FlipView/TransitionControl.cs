namespace Gu.Wpf.FlipView
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    [TemplatePart(Name = PART_NewContent, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_OldContent, Type = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "NewContentStyle", StyleTargetType = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "OldContentStyle", StyleTargetType = typeof(ContentPresenter))]
    public class TransitionControl : ContentControl
    {
        public const string PART_OldContent = "PART_OldContent";
        public const string PART_NewContent = "PART_NewContent";
        public static readonly RoutedEvent ContentChangedEvent = EventManager.RegisterRoutedEvent("ContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TransitionControl));
        public static readonly RoutedEvent OldContentChangedEvent = EventManager.RegisterRoutedEvent("OldContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TransitionControl));
        public static readonly RoutedEvent NewContentChangedEvent = EventManager.RegisterRoutedEvent("NewContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TransitionControl));
        public static readonly DependencyProperty OldContentStyleProperty = DependencyProperty.Register("OldContentStyle", typeof(Style), typeof(TransitionControl), new PropertyMetadata(default(Style)));
        public static readonly DependencyProperty NewContentStyleProperty = DependencyProperty.Register("NewContentStyle", typeof(Style), typeof(TransitionControl), new PropertyMetadata(default(Style)));

        private static readonly DependencyPropertyKey OldContentPropertyKey = DependencyProperty.RegisterReadOnly(
            "OldContent",
            typeof(object),
            typeof(TransitionControl),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty OldTransitionProperty = DependencyProperty.Register(
            "OldTransition",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty NewTransitionProperty = DependencyProperty.Register(
            "NewTransition",
            typeof(Storyboard),
            typeof(TransitionControl),
            new PropertyMetadata(default(Storyboard)));

        private ContentPresenter _oldContentPresenter;
        private ContentPresenter _newContentPresenter;

        public static readonly DependencyProperty OldContentProperty = OldContentPropertyKey.DependencyProperty;

        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
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
            protected set { SetValue(OldContentPropertyKey, value); }
        }

        public Style OldContentStyle
        {
            get { return (Style)GetValue(OldContentStyleProperty); }
            set { SetValue(OldContentStyleProperty, value); }
        }

        /// <summary>
        /// Gest or sets the storyboard that controls how old content animates out of view
        /// </summary>
        public Storyboard OldTransition
        {
            get { return (Storyboard)GetValue(OldTransitionProperty); }
            set { SetValue(OldTransitionProperty, value); }
        }

        public Style NewContentStyle
        {
            get { return (Style)GetValue(NewContentStyleProperty); }
            set { SetValue(NewContentStyleProperty, value); }
        }

        /// <summary>
        /// Gest or sets the storyboard that controls how new content animates into view
        /// </summary>
        public Storyboard NewTransition
        {
            get { return (Storyboard)GetValue(NewTransitionProperty); }
            set { SetValue(NewTransitionProperty, value); }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            OldContent = oldContent;
            RaiseEvent(new RoutedEventArgs(ContentChangedEvent, this));

            RaiseEvent(new RoutedEventArgs(OldContentChangedEvent, this));
            if (_oldContentPresenter != null)
            {
                _oldContentPresenter.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, _oldContentPresenter));
            }

            RaiseEvent(new RoutedEventArgs(NewContentChangedEvent, this));
            if (_newContentPresenter != null)
            {
                _newContentPresenter.RaiseEvent(new RoutedEventArgs(ContentChangedEvent, _newContentPresenter));
            }
            base.OnContentChanged(oldContent, newContent);
        }

        public override void OnApplyTemplate()
        {
            _newContentPresenter = GetTemplateChild(PART_NewContent) as ContentPresenter;
            _oldContentPresenter = GetTemplateChild(PART_OldContent) as ContentPresenter;
            base.OnApplyTemplate();
        }
    }
}
