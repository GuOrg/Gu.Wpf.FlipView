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
        public static readonly DependencyProperty GestureFinderProperty = DependencyProperty.Register(
            "GestureFinder",
            typeof(IGestureFinder),
            typeof(FlipView),
            new PropertyMetadata(new ManipulationGestureFinder(), OnGestureFinderChanged));

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
        private readonly TranslateTransform _nextOffsetTransform = new TranslateTransform();
        private readonly TransformGroup _nextTransform;
        private AnimationTimeline _animation;
        private int? _nextIndex = -1;
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

            _nextTransform = new TransformGroup();
            _nextTransform.Children.Add(_nextOffsetTransform);
            _nextTransform.Children.Add(_currentTransform);
        }

        public IGestureFinder GestureFinder
        {
            get { return (IGestureFinder)GetValue(GestureFinderProperty); }
            set { SetValue(GestureFinderProperty, value); }
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

        public TranslateTransform NextOffsetTransform
        {
            get
            {
                return _nextOffsetTransform;
            }
        }

        public TransformGroup NextTransform
        {
            get
            {
                return _nextTransform;
            }
        }

        internal int? NextIndex
        {
            get
            {
                return this._nextIndex;
            }
            set
            {
                if (value == _nextIndex)
                {
                    return;
                }
                this._nextIndex = value;
                if (_nextIndex != null && _nextIndex >= 0 && _nextIndex < Items.Count)
                {
                    SetCurrentValue(PreviousItemProperty, this.Items[_nextIndex.Value]);
                    var sign = NextIndex > SelectedIndex ? 1 : -1;
                    _nextOffsetTransform.X = sign * PART_SwipePanel.ActualWidth;
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
            if (this.GestureFinder != null)
            {
                this.GestureFinder.InputElement = PART_SwipePanel;
            }
        }

        /// <summary>
        /// Exposing this for tests
        /// </summary>
        /// <param name="delta"></param>
        internal void OnSwipe(Vector delta)
        {
            if (_animation != null)
            {
                CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
                _animation.Completed -= OnAnimationCompleted;
                _animation = null;
            }
            _isSwiping = true;
            var actualWidth = this.PART_SwipePanel.ActualWidth;
            this.CurrentTransform.X += delta.X;
            var sign = this.CurrentTransform.X > 0 ? -1 : 1;
            this.NextOffsetTransform.X = sign * actualWidth;
            this.NextIndex = SelectedIndex + sign;
        }

        internal void OnSwipeEnded(Vector velocity)
        {
            var animation = TransitionTo(SelectedIndex, NextIndex.Value, velocity);
            _isSwiping = false;
            AnimateCurrentTransform(animation);
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
                NextIndex = newIndex;
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
            SelectedIndex = NextIndex.Value;
            CommandManager.InvalidateRequerySuggested();
            NextIndex = null;
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
            CurrentTransform.X = 0;
            _animation = null;
        }

        private static void OnGestureFinderChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gestureFinder = (ManipulationGestureFinder)e.NewValue;
            if (gestureFinder != null)
            {
                gestureFinder.InputElement = ((FlipView)o).PART_SwipePanel;
            }
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex > 0;
            e.Handled = true;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var animation = TransitionTo(SelectedIndex, SelectedIndex - 1);
            AnimateCurrentTransform(animation);
            e.Handled = true;
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex < (this.Items.Count - 1);
            e.Handled = true;
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var animation = TransitionTo(SelectedIndex, SelectedIndex + 1);
            AnimateCurrentTransform(animation);
            e.Handled = true;
        }
    }
}