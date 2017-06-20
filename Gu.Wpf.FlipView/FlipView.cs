namespace Gu.Wpf.FlipView
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using Gu.Wpf.FlipView.Gestures;

    /// <summary>
    /// A <see cref="Selector"/> for navigating the content.
    /// </summary>
    public class FlipView : Selector
    {
        public static readonly DependencyProperty IncreaseInAnimationProperty = DependencyProperty.Register(
            "IncreaseInAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty IncreaseOutAnimationProperty = DependencyProperty.Register(
            "IncreaseOutAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty DecreaseInAnimationProperty = DependencyProperty.Register(
            "DecreaseInAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty DecreaseOutAnimationProperty = DependencyProperty.Register(
            "DecreaseOutAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        private static readonly DependencyPropertyKey CurrentInAnimationPropertyKey = DependencyProperty.RegisterReadOnly(
            "CurrentInAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty CurrentInAnimationProperty = CurrentInAnimationPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey CurrentOutAnimationPropertyKey = DependencyProperty.RegisterReadOnly(
            "CurrentOutAnimation",
            typeof(Storyboard),
            typeof(FlipView),
            new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty CurrentOutAnimationProperty = CurrentOutAnimationPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ShowIndexProperty = DependencyProperty.RegisterAttached(
            "ShowIndex",
            typeof(bool),
            typeof(FlipView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty IndexPlacementProperty = DependencyProperty.Register(
            "IndexPlacement",
            typeof(IndexPlacement),
            typeof(FlipView),
            new FrameworkPropertyMetadata(IndexPlacement.Above, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty IndexItemStyleProperty = DependencyProperty.Register(
            "IndexItemStyle",
            typeof(Style),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ShowArrowsProperty = DependencyProperty.RegisterAttached(
            "ShowArrows",
            typeof(bool),
            typeof(FlipView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register(
            "ArrowPlacement",
            typeof(ArrowPlacement),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(ArrowPlacement), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ArrowButtonStyleProperty = DependencyProperty.Register(
            "ArrowButtonStyle",
            typeof(Style),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
            IsTabStopProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(false));
            var metadata = (FrameworkPropertyMetadata)SelectedIndexProperty.GetMetadata(typeof(Selector));
            SelectedIndexProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(
                metadata.DefaultValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                metadata.PropertyChangedCallback,
                (d, basevalue) => CoerceSelectedIndexProxy(d, metadata.CoerceValueCallback.Invoke(d, basevalue))));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            CommandManager.RegisterClassCommandBinding(typeof(FlipView), new CommandBinding(NavigationCommands.BrowseBack, OnPreviousExecuted, OnPreviousCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FlipView), new CommandBinding(NavigationCommands.BrowseForward, OnNextExecuted, OnNextCanExecute));
            EventManager.RegisterClassHandler(typeof(FlipView), GesturePanel.GesturedEvent, new GesturedEventhandler(OnGesture));
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index increases
        /// </summary>
        public Storyboard IncreaseInAnimation
        {
            get => (Storyboard)this.GetValue(IncreaseInAnimationProperty);
            set => this.SetValue(IncreaseInAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets how new content animates out when selected index increases
        /// </summary>
        public Storyboard IncreaseOutAnimation
        {
            get => (Storyboard)this.GetValue(IncreaseOutAnimationProperty);
            set => this.SetValue(IncreaseOutAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseInAnimation
        {
            get => (Storyboard)this.GetValue(DecreaseInAnimationProperty);
            set => this.SetValue(DecreaseInAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseOutAnimation
        {
            get => (Storyboard)this.GetValue(DecreaseOutAnimationProperty);
            set => this.SetValue(DecreaseOutAnimationProperty, value);
        }

        /// <summary>
        /// Gets how new content animates in
        /// </summary>
        public Storyboard CurrentInAnimation
        {
            get => (Storyboard)this.GetValue(CurrentInAnimationProperty);
            protected set => this.SetValue(CurrentInAnimationPropertyKey, value);
        }

        /// <summary>
        /// Gets how new content animates out
        /// </summary>
        public Storyboard CurrentOutAnimation
        {
            get => (Storyboard)this.GetValue(CurrentOutAnimationProperty);
            protected set => this.SetValue(CurrentOutAnimationPropertyKey, value);
        }

        public bool ShowIndex
        {
            get => (bool)this.GetValue(ShowIndexProperty);
            set => this.SetValue(ShowIndexProperty, value);
        }

        public IndexPlacement IndexPlacement
        {
            get => (IndexPlacement)this.GetValue(IndexPlacementProperty);
            set => this.SetValue(IndexPlacementProperty, value);
        }

        /// <summary>
        /// A style for how the index items looks, BasedOn ListboxItem
        /// </summary>
        public Style IndexItemStyle
        {
            get => (Style)this.GetValue(IndexItemStyleProperty);
            set => this.SetValue(IndexItemStyleProperty, value);
        }

        public bool ShowArrows
        {
            get => (bool)this.GetValue(ShowArrowsProperty);
            set => this.SetValue(ShowArrowsProperty, value);
        }

        public ArrowPlacement ArrowPlacement
        {
            get => (ArrowPlacement)this.GetValue(ArrowPlacementProperty);
            set => this.SetValue(ArrowPlacementProperty, value);
        }

        public Style ArrowButtonStyle
        {
            get => (Style)this.GetValue(ArrowButtonStyleProperty);
            set => this.SetValue(ArrowButtonStyleProperty, value);
        }

        private static object CoerceSelectedIndexProxy(DependencyObject d, object basevalue)
        {
            if (basevalue is int index)
            {
                var flipView = (FlipView)d;
                flipView.PreviewSelectedIndexChanged(flipView.SelectedIndex, index);
            }

            return basevalue;
        }

        protected virtual void PreviewSelectedIndexChanged(int previousIndex, int newIndex)
        {
            if (previousIndex == -1 || (previousIndex == newIndex))
            {
                this.CurrentInAnimation = null;
                this.CurrentOutAnimation = null;
            }
            else if (newIndex > previousIndex)
            {
                this.CurrentInAnimation = this.IncreaseInAnimation;
                this.CurrentOutAnimation = this.IncreaseOutAnimation;
            }
            else
            {
                this.CurrentInAnimation = this.DecreaseInAnimation;
                this.CurrentOutAnimation = this.DecreaseOutAnimation;
            }
        }

        private static void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                e.CanExecute = flipView.IsWithinBounds(flipView.SelectedIndex - 1);
                e.Handled = true;
            }
        }

        private static void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                e.Handled = flipView.TransitionTo(flipView.SelectedIndex - 1);
            }
        }

        private static void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                e.CanExecute = flipView.IsWithinBounds(flipView.SelectedIndex + 1);
                e.Handled = true;
            }
        }

        private static void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                e.Handled = flipView.TransitionTo(flipView.SelectedIndex + 1);
            }
        }

        private static void OnGesture(object sender, GesturedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                if (e.Gesture == GestureType.SwipeLeft)
                {
                    flipView.TransitionTo(flipView.SelectedIndex + 1);
                }

                if (e.Gesture == GestureType.SwipeRight)
                {
                    flipView.TransitionTo(flipView.SelectedIndex - 1);
                }
            }
        }

        private bool TransitionTo(int newIndex)
        {
            if (newIndex == this.SelectedIndex)
            {
                return false;
            }

            var isWithinBounds = this.IsWithinBounds(newIndex);
            if (isWithinBounds)
            {
                this.SetCurrentValue(SelectedIndexProperty, newIndex);
            }

            return isWithinBounds;
        }

        private bool IsWithinBounds(int newIndex)
        {
            if (newIndex < 0 || newIndex > (this.Items.Count - 1))
            {
                return false;
            }

            return true;
        }
    }
}