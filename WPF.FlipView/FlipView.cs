using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.FlipView
{
    using System.Runtime.InteropServices;
    using System.Windows.Media.Animation;
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

        public static readonly DependencyProperty IndexWidthProperty = DependencyProperty.RegisterAttached(
            "IndexWidth",
            typeof(double),
            typeof(FlipView),
            new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty IndexHeightProperty = DependencyProperty.RegisterAttached(
            "IndexHeight",
            typeof(double),
            typeof(FlipView),
            new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty SelectedIndexColorProperty = DependencyProperty.RegisterAttached(
            "SelectedIndexColor",
            typeof(Brush),
            typeof(FlipView),
            new FrameworkPropertyMetadata(SystemColors.ActiveBorderBrush, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty IndexColorProperty = DependencyProperty.RegisterAttached(
            "IndexColor",
            typeof(Brush),
            typeof(FlipView),
            new FrameworkPropertyMetadata(SystemColors.ControlBrush, FrameworkPropertyMetadataOptions.None));

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
            get { return (IGestureTracker)GetValue(GestureTrackerProperty); }
            set { SetValue(GestureTrackerProperty, value); }
        }

        public int TransitionTime
        {
            get
            {
                return (int)GetValue(FlipView.TransitionTimeProperty);
            }
            set
            {
                SetValue(FlipView.TransitionTimeProperty, value);
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

        public Brush SelectedIndexColor
        {
            get
            {
                return (Brush)GetValue(SelectedIndexColorProperty);
            }
            set
            {
                SetValue(SelectedIndexColorProperty, value);
            }
        }

        public Brush IndexColor
        {
            get
            {
                return (Brush)GetValue(IndexColorProperty);
            }
            set
            {
                SetValue(IndexColorProperty, value);
            }
        }

        public bool ShowIndex
        {
            get
            {
                return (bool)GetValue(ShowIndexProperty);
            }
            set
            {
                SetValue(ShowIndexProperty, value);
            }
        }

        public double IndexWidth
        {
            get
            {
                return (int)GetValue(IndexWidthProperty);
            }
            set
            {
                SetValue(IndexWidthProperty, value);
            }
        }

        public double IndexHeight
        {
            get
            {
                return (int)GetValue(IndexHeightProperty);
            }
            set
            {
                SetValue(IndexHeightProperty, value);
            }
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
                if (this._otherIndex != null && this.IsWithinBounds(this._otherIndex.Value))
                {
                    SetCurrentValue(OtherItemProperty, this.Items[this._otherIndex.Value]);
                    var sign = this.OtherIndex > SelectedIndex ? 1 : -1;
                    this._otherItemOffsetTransform.X = sign * _partSwipePanel.ActualWidth;
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
            this._partSwipePanel = this.GetTemplateChild(PartSwipePanelName) as Panel;
            if (this.GestureTracker != null)
            {
                this.GestureTracker.InputElement = _partSwipePanel;
            }
        }

        private bool TransitionTo(int newIndex)
        {
            var animation = TransitionTo(SelectedIndex, newIndex);
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
            if (_animation != null) // Just replace the items and let the current animation continue
            {
                return null;
            }
            return this.CreateTransitionAnimation(new Transition(OtherIndex, SelectedIndex));
        }

        /// <summary>
        /// Creates the animation for animating to the next slide
        /// Returns null if animation is already running
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>null if TransitionTime == 0</returns>
        internal AnimationTimeline CreateTransitionAnimation(Transition? transition)
        {
            if (transition == null)
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
                    var sign = transition.Value.From < transition.Value.To? 1 : -1;
                    SelectedItemTransform.X = sign * actualWidth;
                    OtherItemOffsetTransform.X = -1 * sign * actualWidth;
                }
            }
            if (TransitionTime > 0)
            {
                double delta = Math.Abs(this.SelectedItemTransform.X - toValue);
                var duration = TimeSpan.FromMilliseconds((delta / actualWidth) * TransitionTime);
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
            if (_animation != null && animation != null)
            {
                _animation.Completed -= OnAnimationCompleted;
                _animation = null;
                this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            }
            if (animation != null)
            {
                _animation = animation;
                animation.Completed += this.OnAnimationCompleted;
                this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, animation);
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
            this.OtherIndex = null;
            this.SelectedItemTransform.BeginAnimation(TranslateTransform.XProperty, null);
            this.SelectedItemTransform.X = 0;
            _animation = null;
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gestureFinder = (IGestureTracker)e.NewValue;
            var flipView = ((FlipView)o);
            if (gestureFinder != null)
            {
                gestureFinder.InputElement = flipView._partSwipePanel;
                gestureFinder.Gestured += flipView.OnGesture;
            }
            var old = (IGestureTracker)e.OldValue;
            if (old != null)
            {
                old.InputElement = null;
                old.Gestured += flipView.OnGesture;
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
            if (tracker == null)
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
            e.CanExecute = this.IsWithinBounds(SelectedIndex - 1);
            e.Handled = true;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = TransitionTo(SelectedIndex - 1);
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsWithinBounds(SelectedIndex + 1);
            e.Handled = true;
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = TransitionTo(SelectedIndex + 1);
        }
    }
}