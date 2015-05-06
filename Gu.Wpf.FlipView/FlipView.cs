namespace Gu.Wpf.FlipView
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using Gu.Wpf.FlipView.Gestures;

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
        }

        public FlipView()
        {
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, OnPreviousExecuted, OnPreviousCanExecute));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, OnNextExecuted, OnNextCanExecute));
            AddHandler(GesturePanel.GesturedEvent, new GesturedEventhandler(OnGesture));
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index increases
        /// </summary>
        public Storyboard IncreaseInAnimation
        {
            get { return (Storyboard)GetValue(IncreaseInAnimationProperty); }
            set { SetValue(IncreaseInAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates out when selected index increases
        /// </summary>
        public Storyboard IncreaseOutAnimation
        {
            get { return (Storyboard)GetValue(IncreaseOutAnimationProperty); }
            set { SetValue(IncreaseOutAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseInAnimation
        {
            get { return (Storyboard)GetValue(DecreaseInAnimationProperty); }
            set { SetValue(DecreaseInAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets how new content animates in when selected index decreases
        /// </summary>
        public Storyboard DecreaseOutAnimation
        {
            get { return (Storyboard)GetValue(DecreaseOutAnimationProperty); }
            set { SetValue(DecreaseOutAnimationProperty, value); }
        }

        /// <summary>
        /// Gets how new content animates in
        /// </summary>
        public Storyboard CurrentInAnimation
        {
            get { return (Storyboard)GetValue(CurrentInAnimationProperty); }
            protected set { SetValue(CurrentInAnimationPropertyKey, value); }
        }

        /// <summary>
        /// Gets how new content animates out
        /// </summary>
        public Storyboard CurrentOutAnimation
        {
            get { return (Storyboard)GetValue(CurrentOutAnimationProperty); }
            protected set { SetValue(CurrentOutAnimationPropertyKey, value); }
        }

        public bool ShowIndex
        {
            get { return (bool)GetValue(ShowIndexProperty); }
            set { SetValue(ShowIndexProperty, value); }
        }

        public IndexPlacement IndexPlacement
        {
            get { return (IndexPlacement)GetValue(IndexPlacementProperty); }
            set { SetValue(IndexPlacementProperty, value); }
        }

        /// <summary>
        /// A style for how the index items looks, BasedOn ListboxItem
        /// </summary>
        public Style IndexItemStyle
        {
            get { return (Style)GetValue(IndexItemStyleProperty); }
            set { SetValue(IndexItemStyleProperty, value); }
        }

        public bool ShowArrows
        {
            get
            {
                return (bool)GetValue(ShowArrowsProperty);
            }
            set
            {
                SetValue(ShowArrowsProperty, value);
            }
        }

        public ArrowPlacement ArrowPlacement
        {
            get
            {
                return (ArrowPlacement)GetValue(ArrowPlacementProperty);
            }
            set
            {
                SetValue(ArrowPlacementProperty, value);
            }
        }

        public Style ArrowButtonStyle
        {
            get { return (Style)GetValue(ArrowButtonStyleProperty); }
            set { SetValue(ArrowButtonStyleProperty, value); }
        }

        private bool TransitionTo(int newIndex)
        {
            if (newIndex == SelectedIndex)
            {
                return false;
            }
            var isWithinBounds = IsWithinBounds(newIndex);
            if (isWithinBounds)
            {
                bool isIncrease = newIndex > SelectedIndex;
                if (isIncrease)
                {
                    CurrentInAnimation = IncreaseInAnimation;
                    CurrentOutAnimation = IncreaseOutAnimation;
                }
                else
                {
                    CurrentInAnimation = DecreaseInAnimation;
                    CurrentOutAnimation = DecreaseOutAnimation;
                }
                SelectedIndex = newIndex;
            }
            return isWithinBounds;
        }

        private bool IsWithinBounds(int newIndex)
        {
            if (newIndex < 0 || newIndex > (Items.Count - 1))
            {
                return false;
            }
            return true;
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsWithinBounds(SelectedIndex - 1);
            e.Handled = true;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = TransitionTo(SelectedIndex - 1);
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsWithinBounds(SelectedIndex + 1);
            e.Handled = true;
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = TransitionTo(SelectedIndex + 1);
        }

        private void OnGesture(object sender, GesturedEventArgs e)
        {
            if (e.Gesture == GestureType.SwipeLeft)
            {
                TransitionTo(SelectedIndex - 1);
            }
            if (e.Gesture == GestureType.SwipeRight)
            {
                TransitionTo(SelectedIndex + 1);
            }
        }
    }
}