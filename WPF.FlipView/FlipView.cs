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
    [TemplatePart(Name = PART_SwipePanelName, Type = typeof(Panel))]
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
            new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

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

        public static readonly DependencyProperty PreviousItemProperty = DependencyProperty.Register(
            "PreviousItem",
            typeof(object),
            typeof(FlipView),
            new PropertyMetadata(null));

        private readonly TranslateTransform _currentTransform = new TranslateTransform();
        private readonly TranslateTransform _previousOffsetTransform = new TranslateTransform();
        private readonly TransformGroup _previousTransform;
        private AnimationTimeline _animation;
        private int? _previousIndex = -1;
        private bool _isSwiping;
        private const string PART_SwipePanelName = "PART_SwipePanel";
        private Panel PART_SwipePanel;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
        }

        public FlipView()
        {
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, this.OnPreviousExecuted, this.OnPreviousCanExecute));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, this.OnNextExecuted, this.OnNextCanExecute));

            this._previousTransform = new TransformGroup();
            this._previousTransform.Children.Add(this._previousOffsetTransform);
            this._previousTransform.Children.Add(_currentTransform);
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

        public object PreviousItem
        {
            get { return (object)GetValue(PreviousItemProperty); }
            set { SetValue(PreviousItemProperty, value); }
        }

        public TranslateTransform CurrentTransform
        {
            get
            {
                return _currentTransform;
            }
        }

        internal TranslateTransform PreviousOffsetTransform
        {
            get
            {
                return this._previousOffsetTransform;
            }
        }

        public TransformGroup PreviousTransform
        {
            get
            {
                return this._previousTransform;
            }
        }

        internal int? PreviousIndex
        {
            get
            {
                return this._previousIndex;
            }
            set
            {
                if (value == this._previousIndex)
                {
                    return;
                }
                this._previousIndex = value;
                if (this._previousIndex != null && this.IsWithinBounds(this._previousIndex.Value))
                {
                    SetCurrentValue(PreviousItemProperty, this.Items[this._previousIndex.Value]);
                    var sign = this.PreviousIndex > SelectedIndex ? 1 : -1;
                    this._previousOffsetTransform.X = sign * PART_SwipePanel.ActualWidth;
                }
                else
                {
                    SetCurrentValue(PreviousItemProperty, null);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_SwipePanel = this.GetTemplateChild(PART_SwipePanelName) as Panel;
            if (this.GestureTracker != null)
            {
                this.GestureTracker.InputElement = PART_SwipePanel;
            }
        }

        private bool TransitionTo(int newIndex)
        {
            var animation = TransitionTo(SelectedIndex, newIndex);
            AnimateCurrentTransform(animation);
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
                this.PreviousIndex = newIndex;
            }
            if (_animation != null)
            {
                return null;
            }
            if (newIndex >= 0 && newIndex < this.Items.Count)
            {
                return CreateCurrentTransformSlideAnimation(new Transition(oldIndex, newIndex));
            }
            return null;
        }

        /// <summary>
        /// Creates the animation for animating to the next slide
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>null if TransitionTime == 0</returns>
        internal AnimationTimeline CreateCurrentTransformSlideAnimation(Transition? transition)
        {
            if (transition == null)
            {
                return null;
            }

            if (_animation != null)
            {
                return null;
            }
            var actualWidth = PART_SwipePanel.ActualWidth;
            double toValue = 0;
            if (transition.Value.From != transition.Value.To)
            {
                toValue = transition.Value.From < transition.Value.To ? -actualWidth : actualWidth;
            }
            if (TransitionTime > 0)
            {
                double delta = Math.Abs(CurrentTransform.X - toValue);
                var duration = TimeSpan.FromMilliseconds((delta / actualWidth) * TransitionTime);
                var animation = AnimationFactory.CreateAnimation(CurrentTransform.X, toValue, duration);
                return animation;
            }
            else
            {
                CurrentTransform.X = toValue;
            }
            return null;
        }

        /// <summary>
        /// Applies the animation on CurrentTransform.X
        /// </summary>
        /// <param name="animation"></param>
        private void AnimateCurrentTransform(AnimationTimeline animation)
        {
            if (_animation != null && animation != null)
            {
                _animation.Completed -= OnAnimationCompleted;
                _animation = null;
                CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
            }
            if (animation != null)
            {
                _animation = animation;
                animation.Completed += this.OnAnimationCompleted;
                CurrentTransform.BeginAnimation(TranslateTransform.XProperty, animation);
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
            SelectedIndex = this.PreviousIndex.Value;
            CommandManager.InvalidateRequerySuggested();
            this.PreviousIndex = null;
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
            CurrentTransform.X = 0;
            _animation = null;
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gestureFinder = (IGestureTracker)e.NewValue;
            var flipView = ((FlipView)o);
            if (gestureFinder != null)
            {
                gestureFinder.InputElement = flipView.PART_SwipePanel;
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

        private void OnGesture(object sender, GestureEventArgs gesture)
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

            if (interpreter.IsBack(gesture))
            {
                TransitionTo(SelectedIndex - 1);
            }

            if (interpreter.IsForward(gesture))
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