namespace Gu.Wpf.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Gu.Wpf.FlipView.Gestures;

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
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, this.OnPreviousExecuted, this.OnPreviousCanExecute));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, this.OnNextExecuted, this.OnNextCanExecute));

            this._otherItemTransform = new TransformGroup();
            this._otherItemTransform.Children.Add(this._otherItemOffsetTransform);
            this._otherItemTransform.Children.Add(this._selectedItemTransform);
        }

        public IGestureTracker GestureTracker
        {
            get { return (IGestureTracker)this.GetValue(GestureTrackerProperty); }
            set { this.SetValue(GestureTrackerProperty, value); }
        }

        public int TransitionTime
        {
            get
            {
                return (int)this.GetValue(FlipView.TransitionTimeProperty);
            }
            set
            {
                this.SetValue(FlipView.TransitionTimeProperty, value);
            }
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
            get
            {
                return (bool)this.GetValue(ShowArrowsProperty);
            }
            set
            {
                this.SetValue(ShowArrowsProperty, value);
            }
        }

        public ArrowPlacement ArrowPlacement
        {
            get
            {
                return (ArrowPlacement)this.GetValue(ArrowPlacementProperty);
            }
            set
            {
                this.SetValue(ArrowPlacementProperty, value);
            }
        }

        public Style ArrowButtonStyle
        {
            get { return (Style)this.GetValue(ArrowButtonStyleProperty); }
            set { this.SetValue(ArrowButtonStyleProperty, value); }
        }

        /// <summary>
        /// This is the other item that shows during transitions
        /// </summary>
        public object OtherItem
        {
            get { return (object)this.GetValue(OtherItemProperty); }
            set { this.SetValue(OtherItemProperty, value); }
        }

        public TranslateTransform SelectedItemTransform
        {
            get
            {
                return this._selectedItemTransform;
            }
        }

        internal TranslateTransform OtherItemOffsetTransform
        {
            get
            {
                return this._otherItemOffsetTransform;
            }
        }

        public TransformGroup OtherItemTransform
        {
            get
            {
                return this._otherItemTransform;
            }
        }

        internal int? OtherIndex
        {
            get
            {
                return this._otherIndex;
            }
            set
            {
                if (value == this._otherIndex)
                {
                    return;
                }
                this._otherIndex = value;
                if (this._otherIndex != null && this.IsWithinBounds(this._otherIndex.Value) && this._partSwipePanel != null)
                {
                    this.SetCurrentValue(OtherItemProperty, this.Items[this._otherIndex.Value]);
                    var sign = this.OtherIndex > this.SelectedIndex ? 1 : -1;
                    this._otherItemOffsetTransform.X = sign * this._partSwipePanel.ActualWidth;
                }
                else
                {
                    this.SetCurrentValue(OtherItemProperty, null);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._partSwipePanel = this.GetTemplateChild(PartSwipePanelName) as Panel;
            if (this.GestureTracker != null && this._partSwipePanel != null)
            {
                this.GestureTracker.InputElement = this._partSwipePanel;
                this.GestureTracker.Gestured += this.OnGesture;
            }
        }

        private bool TransitionTo(int newIndex)
        {
            var animation = this.TransitionTo(this.SelectedIndex, newIndex);
            this.AnimateTransition(animation);
            return this.IsWithinBounds(newIndex);
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
                if (this.IsWithinBounds(newIndex))
                {
                    this.SelectedIndex = newIndex;
                    this.OtherIndex = oldIndex;
                }
                else
                {
                    this.OtherIndex = null;
                }
            }
            if (this._animation != null) // Just replace the items and let the current animation continue
            {
                return null;
            }
            return this.CreateTransitionAnimation(new Transition(this.OtherIndex, this.SelectedIndex));
        }

        /// <summary>
        /// Creates the animation for animating to the next slide
        /// Returns null if animation is already running
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>null if TransitionTime == 0</returns>
        internal AnimationTimeline CreateTransitionAnimation(Transition? transition)
        {
            if (transition == null || this._partSwipePanel == null)
            {
                return null;
            }

            if (this._animation != null)
            {
                return null;
            }
            var actualWidth = this._partSwipePanel.ActualWidth;
            double toValue = 0;
            if (transition.Value.From != transition.Value.To)
            {
                if (this._animation == null)
                {
                    var sign = transition.Value.From < transition.Value.To ? 1 : -1;
                    this.SelectedItemTransform.X = sign * actualWidth;
                    this.OtherItemOffsetTransform.X = -1 * sign * actualWidth;
                }
            }
            if (this.TransitionTime > 0)
            {
                double delta = Math.Abs(this.SelectedItemTransform.X - toValue);
                var duration = TimeSpan.FromMilliseconds((delta / actualWidth) * this.TransitionTime);
                var animation = AnimationFactory.CreateAnimation(this.SelectedItemTransform.X, 0, duration);
                return animation;
            }
            else
            {
                this.SelectedItemTransform.X = toValue;
            }
            return null;
        }

        /// <summary>
        /// Applies the animation on SelectedItemTransform.X
        /// </summary>
        /// <param name="animation"></param>
        internal void AnimateTransition(AnimationTimeline animation)
        {
            if (this._animation != null && animation != null)
            {
                this._animation.Completed -= this.OnAnimationCompleted;
                this._animation = null;
                this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            }
            if (animation != null)
            {
                this._animation = animation;
                animation.Completed += this.OnAnimationCompleted;
                this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }
            else
            {
                if (this._animation == null)
                {
                    this.OnAnimationCompleted(null, null);
                }
            }
        }

        internal void OnAnimationCompleted(object sender, EventArgs args)
        {
            //CommandManager.InvalidateRequerySuggested();
            this.OtherIndex = null;
            this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            this.SelectedItemTransform.X = 0;
            this._animation = null;
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
            if (newIndex < 0 || newIndex > (this.Items.Count - 1))
            {
                return false;
            }
            return true;
        }

        private void OnGesture(object sender, GestureEventArgs e)
        {
            var tracker = this.GestureTracker;
            if (tracker == null || !ReferenceEquals(tracker.InputElement, this._partSwipePanel))
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
                this.TransitionTo(this.SelectedIndex - 1);
            }

            if (interpreter.IsForward(e.Gesture))
            {
                this.TransitionTo(this.SelectedIndex + 1);
            }
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsWithinBounds(this.SelectedIndex - 1);
            e.Handled = true;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = this.TransitionTo(this.SelectedIndex - 1);
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsWithinBounds(this.SelectedIndex + 1);
            e.Handled = true;
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = this.TransitionTo(this.SelectedIndex + 1);
        }
    }
}