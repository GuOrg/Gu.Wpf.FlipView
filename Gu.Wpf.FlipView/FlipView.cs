namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Gestures;

    [TemplatePart(Name = PartSwipePanelName, Type = typeof(Panel))]
    public class FlipView : Selector
    {
        public static readonly DependencyProperty GestureTrackerProperty = DependencyProperty.Register(
            "GestureTracker",
            typeof(IGestureTracker),
            typeof(FlipView),
            new PropertyMetadata(new TouchGestureTracker(), OnGestureTrackerChanged));

        public static readonly DependencyProperty TransitionTimeProperty = DependencyProperty.Register(
            "TransitionTime",
            typeof(int),
            typeof(FlipView),
            new UIPropertyMetadata(300));

        public static readonly DependencyProperty ShowIndexProperty = DependencyProperty.RegisterAttached(
            "ShowIndex",
            typeof(bool),
            typeof(FlipView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty IndexPlacementProperty = DependencyProperty.Register(
            "IndexPlacement",
            typeof(IndexPlacement),
            typeof(FlipView),
            new PropertyMetadata(IndexPlacement.Above));

        public static readonly DependencyProperty IndexItemStyleProperty = DependencyProperty.Register(
            "IndexItemStyle",
            typeof(Style),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ShowArrowsProperty = DependencyProperty.RegisterAttached(
            "ShowArrows",
            typeof(bool),
            typeof(FlipView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register(
            "ArrowPlacement",
            typeof(ArrowPlacement),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(ArrowPlacement), FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ArrowButtonStyleProperty = DependencyProperty.Register(
            "ArrowButtonStyle",
            typeof(Style),
            typeof(FlipView),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty OtherItemProperty = DependencyProperty.Register(
            "OtherItem",
            typeof(object),
            typeof(FlipView),
            new PropertyMetadata(null));

        private readonly TranslateTransform _selectedItemTransform = new TranslateTransform();
        private readonly TranslateTransform _otherItemOffsetTransform = new TranslateTransform();
        private readonly TransformGroup _otherItemTransform;
        private AnimationTimeline _animation;
        private int? _otherIndex = -1;
        private const string PartSwipePanelName = "PART_SwipePanel";
        private Panel _partSwipePanel;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
        }

        public FlipView()
        {
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, OnPreviousExecuted, OnPreviousCanExecute));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, OnNextExecuted, OnNextCanExecute));

            _otherItemTransform = new TransformGroup();
            _otherItemTransform.Children.Add(_otherItemOffsetTransform);
            _otherItemTransform.Children.Add(_selectedItemTransform);
        }

        public IGestureTracker GestureTracker
        {
            get { return (IGestureTracker)GetValue(GestureTrackerProperty); }
            set { SetValue(GestureTrackerProperty, value); }
        }

        public int TransitionTime
        {
            get
            {
                return (int)GetValue(TransitionTimeProperty);
            }
            set
            {
                SetValue(TransitionTimeProperty, value);
            }
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

        /// <summary>
        /// This is the other item that shows during transitions
        /// </summary>
        public object OtherItem
        {
            get { return (object)GetValue(OtherItemProperty); }
            set { SetValue(OtherItemProperty, value); }
        }

        public TranslateTransform SelectedItemTransform
        {
            get
            {
                return _selectedItemTransform;
            }
        }

        internal TranslateTransform OtherItemOffsetTransform
        {
            get
            {
                return _otherItemOffsetTransform;
            }
        }

        public TransformGroup OtherItemTransform
        {
            get
            {
                return _otherItemTransform;
            }
        }

        internal int? OtherIndex
        {
            get
            {
                return _otherIndex;
            }
            set
            {
                if (value == _otherIndex)
                {
                    return;
                }
                _otherIndex = value;
                if (_otherIndex != null && IsWithinBounds(_otherIndex.Value) && _partSwipePanel != null)
                {
                    SetCurrentValue(OtherItemProperty, Items[_otherIndex.Value]);
                    var sign = OtherIndex > SelectedIndex ? 1 : -1;
                    _otherItemOffsetTransform.X = sign * _partSwipePanel.ActualWidth;
                }
                else
                {
                    SetCurrentValue(OtherItemProperty, null);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _partSwipePanel = GetTemplateChild(PartSwipePanelName) as Panel;
            if (GestureTracker != null && _partSwipePanel != null)
            {
                GestureTracker.InputElement = _partSwipePanel;
                GestureTracker.Gestured += OnGesture;
            }
        }

        private bool TransitionTo(int newIndex)
        {
            var animation = TransitionTo(SelectedIndex, newIndex);
            AnimateTransition(animation);
            return IsWithinBounds(newIndex);
        }

        /// <summary>
        /// Exposed for tests
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        internal AnimationTimeline TransitionTo(int oldIndex, int newIndex, Vector velocity = default(Vector))
        {
            if (oldIndex != newIndex)
            {
                if (IsWithinBounds(newIndex))
                {
                    SelectedIndex = newIndex;
                    OtherIndex = oldIndex;
                }
                else
                {
                    OtherIndex = null;
                }
            }
            if (_animation != null) // Just replace the items and let the current animation continue
            {
                return null;
            }
            return CreateTransitionAnimation(new Transition(OtherIndex, SelectedIndex));
        }

        /// <summary>
        /// Creates the animation for animating to the next slide
        /// Returns null if animation is already running
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>null if TransitionTime == 0</returns>
        internal AnimationTimeline CreateTransitionAnimation(Transition? transition)
        {
            if (transition == null || _partSwipePanel == null)
            {
                return null;
            }

            if (_animation != null)
            {
                return null;
            }
            var actualWidth = _partSwipePanel.ActualWidth;
            double toValue = 0;
            if (transition.Value.From != transition.Value.To)
            {
                if (_animation == null)
                {
                    var sign = transition.Value.From < transition.Value.To ? 1 : -1;
                    SelectedItemTransform.X = sign * actualWidth;
                    OtherItemOffsetTransform.X = -1 * sign * actualWidth;
                }
            }
            if (TransitionTime > 0)
            {
                double delta = Math.Abs(SelectedItemTransform.X - toValue);
                var duration = TimeSpan.FromMilliseconds((delta / actualWidth) * TransitionTime);
                var animation = AnimationFactory.CreateAnimation(SelectedItemTransform.X, 0, duration);
                return animation;
            }
            else
            {
                SelectedItemTransform.X = toValue;
            }
            return null;
        }

        /// <summary>
        /// Applies the animation on SelectedItemTransform.X
        /// </summary>
        /// <param name="animation"></param>
        internal void AnimateTransition(AnimationTimeline animation)
        {
            if (_animation != null && animation != null)
            {
                _animation.Completed -= OnAnimationCompleted;
                _animation = null;
                SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            }
            if (animation != null)
            {
                _animation = animation;
                animation.Completed += OnAnimationCompleted;
                SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }
            else
            {
                if (_animation == null)
                {
                    OnAnimationCompleted(null, null);
                }
            }
        }

        internal void OnAnimationCompleted(object sender, EventArgs args)
        {
            //CommandManager.InvalidateRequerySuggested();
            OtherIndex = null;
            SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            SelectedItemTransform.X = 0;
            _animation = null;
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var tracker = (IGestureTracker)e.NewValue;
            var flipView = ((FlipView)o);
            if (tracker != null && flipView._partSwipePanel != null)
            {
                tracker.InputElement = flipView._partSwipePanel;
                tracker.Gestured += flipView.OnGesture;
            }
            var old = (IGestureTracker)e.OldValue;
            if (old != null)
            {
                old.InputElement = null;
                old.Gestured -= flipView.OnGesture;
            }
        }

        private bool IsWithinBounds(int newIndex)
        {
            if (newIndex < 0 || newIndex > (Items.Count - 1))
            {
                return false;
            }
            return true;
        }

        private void OnGesture(object sender, GestureEventArgs e)
        {
            var tracker = GestureTracker;
            if (tracker == null || !ReferenceEquals(tracker.InputElement, _partSwipePanel))
            {
                return;
            }
            var interpreter = tracker.Interpreter;
            if (interpreter == null)
            {
                return;
            }

            if (interpreter.IsBack(e.Gesture))
            {
                TransitionTo(SelectedIndex - 1);
            }

            if (interpreter.IsForward(e.Gesture))
            {
                TransitionTo(SelectedIndex + 1);
            }
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
    }
}