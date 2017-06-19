namespace Gu.Wpf.FlipView
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
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

        private static readonly DependencyPropertyKey DeferredSelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(
            "DeferredSelectedItem",
            typeof(object),
            typeof(FlipView),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty DeferredSelectedItemProperty = DeferredSelectedItemPropertyKey.DependencyProperty;

        private static readonly DependencyProperty SelectedIndexProxyProperty = DependencyProperty.Register(
            "SelectedIndexProxy",
            typeof(int),
            typeof(FlipView),
            new PropertyMetadata(
                -1,
                null,
                CoerceSelectedIndexProxy));

        private int previousIndex = -1;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
            IsTabStopProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            CommandManager.RegisterClassCommandBinding(typeof(FlipView), new CommandBinding(NavigationCommands.BrowseBack, OnPreviousExecuted, OnPreviousCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FlipView), new CommandBinding(NavigationCommands.BrowseForward, OnNextExecuted, OnNextCanExecute));
            EventManager.RegisterClassHandler(typeof(FlipView), GesturePanel.GesturedEvent, new GesturedEventhandler(OnGesture));
        }

        public FlipView()
        {
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath(SelectedIndexProperty),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            BindingOperations.SetBinding(this, SelectedIndexProxyProperty, binding);
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index increases
        /// </summary>
        public Storyboard IncreaseInAnimation
        {
            get { return (Storyboard)this.GetValue(IncreaseInAnimationProperty); }
            set { this.SetValue(IncreaseInAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates out when selected index increases
        /// </summary>
        public Storyboard IncreaseOutAnimation
        {
            get { return (Storyboard)this.GetValue(IncreaseOutAnimationProperty); }
            set { this.SetValue(IncreaseOutAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseInAnimation
        {
            get { return (Storyboard)this.GetValue(DecreaseInAnimationProperty); }
            set { this.SetValue(DecreaseInAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseOutAnimation
        {
            get { return (Storyboard)this.GetValue(DecreaseOutAnimationProperty); }
            set { this.SetValue(DecreaseOutAnimationProperty, value); }
        }

        /// <summary>
        /// Gets how new content animates in
        /// </summary>
        public Storyboard CurrentInAnimation
        {
            get { return (Storyboard)this.GetValue(CurrentInAnimationProperty); }
            protected set { this.SetValue(CurrentInAnimationPropertyKey, value); }
        }

        /// <summary>
        /// Gets how new content animates out
        /// </summary>
        public Storyboard CurrentOutAnimation
        {
            get { return (Storyboard)this.GetValue(CurrentOutAnimationProperty); }
            protected set { this.SetValue(CurrentOutAnimationPropertyKey, value); }
        }

        /// <summary>
        /// This is updated to SelectedItem after the animations are adjusted for the transtition.
        /// This is the property the TransitionControl should be bound to
        /// </summary>
        public object DeferredSelectedItem
        {
            get { return (object)this.GetValue(DeferredSelectedItemProperty); }
            protected set { this.SetValue(DeferredSelectedItemPropertyKey, value); }
        }

        public bool ShowIndex
        {
            get { return (bool)this.GetValue(ShowIndexProperty); }
            set { this.SetValue(ShowIndexProperty, value); }
        }

        public IndexPlacement IndexPlacement
        {
            get { return (IndexPlacement)this.GetValue(IndexPlacementProperty); }
            set { this.SetValue(IndexPlacementProperty, value); }
        }

        /// <summary>
        /// A style for how the index items looks, BasedOn ListboxItem
        /// </summary>
        public Style IndexItemStyle
        {
            get { return (Style)this.GetValue(IndexItemStyleProperty); }
            set { this.SetValue(IndexItemStyleProperty, value); }
        }

        public bool ShowArrows
        {
            get { return (bool)this.GetValue(ShowArrowsProperty); }
            set { this.SetValue(ShowArrowsProperty, value); }
        }

        public ArrowPlacement ArrowPlacement
        {
            get { return (ArrowPlacement)this.GetValue(ArrowPlacementProperty); }
            set { this.SetValue(ArrowPlacementProperty, value); }
        }

        public Style ArrowButtonStyle
        {
            get { return (Style)this.GetValue(ArrowButtonStyleProperty); }
            set { this.SetValue(ArrowButtonStyleProperty, value); }
        }

        private static object CoerceSelectedIndexProxy(DependencyObject d, object basevalue)
        {
            var flipView = (FlipView)d;
            var index = (int)basevalue;
            flipView.PreviewSelectedIndexChanged(flipView.previousIndex, index);
            flipView.previousIndex = index;
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

            this.DeferredSelectedItem = this.SelectedItem;
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