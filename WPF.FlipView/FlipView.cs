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

    public class FlipView : Selector
    {
        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));
        private readonly TranslateTransform _currentTransform = new TranslateTransform();
        private readonly TranslateTransform _nextOffsetTransform = new TranslateTransform();
        private readonly TransformGroup _nextTransform;
        private AnimationTimeline _animation;
        private ContentPresenter PART_CurrentItem;
        private ContentPresenter PART_NextItem;
        private Grid PART_Root;
        private FrameworkElement PART_Container;
        private ListBox PART_Index;
        private double elasticFactor = 1.0;
        private bool cancelManipulation = false;
        private int _nextIndex = -1;

        public static readonly DependencyProperty TransitionTimeProperty = DependencyProperty.Register(
            "TransitionTime",
            typeof(int),
            typeof(FlipView),
            new PropertyMetadata(300));

        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity", typeof (double), typeof (FlipView), new PropertyMetadata(default(double)));

        public double MinSwipeVelocity
        {
            get { return (double) GetValue(MinSwipeVelocityProperty); }
            set { SetValue(MinSwipeVelocityProperty, value); }
        }   

        public static readonly DependencyProperty ShowIndexProperty = DependencyProperty.RegisterAttached(
            "ShowIndex",
            typeof(bool),
            typeof(FlipView),
            new UIPropertyMetadata(true));

        public static readonly DependencyProperty IndexWidthProperty = DependencyProperty.RegisterAttached(
            "IndexWidth",
            typeof(double),
            typeof(FlipView),
            new UIPropertyMetadata(50.0));

        public static readonly DependencyProperty IndexHeightProperty = DependencyProperty.RegisterAttached(
            "IndexHeight",
            typeof(double),
            typeof(FlipView),
            new UIPropertyMetadata(5.0));

        public static readonly DependencyProperty SelectedIndexColorProperty = DependencyProperty.RegisterAttached(
            "SelectedIndexColor",
            typeof(Brush),
            typeof(FlipView),
            new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public static readonly DependencyProperty IndexColorProperty = DependencyProperty.RegisterAttached(
            "IndexColor",
            typeof(Brush),
            typeof(FlipView),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static readonly DependencyProperty ShowArrowsProperty = DependencyProperty.RegisterAttached(
            "ShowArrows",
            typeof(bool),
            typeof(FlipView),
            new UIPropertyMetadata(true));

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register(
            "ArrowPlacement",
            typeof(ArrowPlacement),
            typeof(FlipView),
            new PropertyMetadata(default(ArrowPlacement)));

        private bool _isPanning;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
            SelectedIndexProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(-1, OnSelectedIndexChanged));
        }

        public FlipView()
        {
            this.CommandBindings.Add(new CommandBinding(NextCommand, this.OnNextExecuted, this.OnNextCanExecute));
            this.CommandBindings.Add(new CommandBinding(PreviousCommand, this.OnPreviousExecuted, this.OnPreviousCanExecute));
            this.Focusable = false;
            this.FocusVisualStyle = null;
            Loaded += (_, e) => CurrentItem = this.GetItemAt(SelectedIndex);

            ManipulationDelta += (sender, args) =>
            {
                if (args.ManipulationContainer == PART_Root)
                {
                    var delta = args.CumulativeManipulation.Translation;
                    if (Math.Abs( delta.X) < 3)
                    {
                        args.Handled = false;
                        return;
                    }
                    if (Math.Abs( args.Velocities.LinearVelocity.X) < MinSwipeVelocity)
                    {
                        args.Handled = false;
                        CurrentTransform.X = 0;
                        return;
                    }
                    if (Math.Abs(delta.X) < Math.Abs(delta.Y))
                    {
                        args.Handled = false;
                        return;
                    }
                    else
                    {
                        OnPan(delta);
                        args.Handled = true;
                    }
                }
                args.Handled = false;
            };

            ManipulationCompleted += (sender, args) =>
            {
                if (args.ManipulationContainer == PART_Root && _isPanning)
                {
                    OnPanEnded(args.TotalManipulation.Translation, args.FinalVelocities.LinearVelocity);
                    args.Handled = true;
                }
                else
                {
                    args.Handled = false;
                }
            };
            _nextTransform = new TransformGroup();
            _nextTransform.Children.Add(_nextOffsetTransform);
            _nextTransform.Children.Add(_currentTransform);
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

        public object CurrentItem
        {
            get
            {
                return this.PART_CurrentItem == null ? null : this.PART_CurrentItem.Content;
            }
            set
            {
                if (this.PART_CurrentItem == null)
                {
                    return;
                }
                PART_CurrentItem.Content = value;
            }
        }

        public object NextItem
        {
            get
            {
                return this.PART_NextItem == null ? null : this.PART_NextItem.Content;
            }
            set
            {
                if (this.PART_NextItem == null)
                {
                    return;
                }
                PART_NextItem.Content = value;
            }
        }

        internal int NextIndex
        {
            get
            {
                return this._nextIndex;
            }
            set
            {
                this._nextIndex = value;
                if (_nextIndex >= 0 && _nextIndex < Items.Count)
                {
                    NextItem = this.GetItemAt(_nextIndex);
                }
                else
                {
                    NextItem = null;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_NextItem = this.GetTemplateChild("PART_NextItem") as ContentPresenter;
            this.PART_CurrentItem = this.GetTemplateChild("PART_CurrentItem") as ContentPresenter;
            this.PART_Root = this.GetTemplateChild("PART_Root") as Grid;
            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;
            this.PART_Index = this.GetTemplateChild("PART_Index") as ListBox;
            CurrentItem = GetItemAt(SelectedIndex);
        }

        /// <summary>
        /// Exposing this for tests
        /// </summary>
        /// <param name="delta"></param>
        internal void OnPan(Vector delta)
        {
            if (_animation != null)
            {
                CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
                _animation.Completed -= OnAnimationCompleted;
                _animation = null;
            }
            _isPanning = true;
            var actualWidth = this.PART_CurrentItem.ActualWidth;
            this.NextOffsetTransform.X = delta.X > 0 ? -actualWidth : actualWidth;
            this.CurrentTransform.X = delta.X;
            this.NextIndex = delta.X < 0 ? this.SelectedIndex + 1 : this.SelectedIndex - 1;
        }

        internal void OnPanEnded(Vector delta, Vector velocity)
        {
            var actualWidth = PART_CurrentItem.ActualWidth;
            var treshold = actualWidth / 2;
            int oldIndex = SelectedIndex;
            if (Math.Abs(delta.X) > treshold || Math.Abs(velocity.X) > MinSwipeVelocity)
            {
                if (SelectedIndex > 0 && delta.X > 0)
                {
                    SelectedIndex--;
                }
                if (SelectedIndex < Items.Count && delta.X < 0)
                {
                    SelectedIndex++;
                }
            }

            Transition? transitionTo = TransitionTo(oldIndex, SelectedIndex);
            var animation = CreateCurrentTransformSlideAnimation(transitionTo);
            _isPanning = false;
            AnimateCurrentTransform(animation);
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = (FlipView)d;
            if (flipView._isPanning)
            {
                return;
            }
            if (((int)e.OldValue) == -1)
            {
                if (flipView.PART_CurrentItem != null)
                {
                    flipView.CurrentItem = flipView.GetItemAt((int)e.NewValue);
                }
                return;
            }
            var transition = flipView.TransitionTo((int)e.OldValue, (int)e.NewValue);
            var animation = flipView.CreateCurrentTransformSlideAnimation(transition);
            flipView.AnimateCurrentTransform(animation);
        }

        /// <summary>
        /// Exposed for tests
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <returns></returns>
        internal Transition? TransitionTo(int oldIndex, int newIndex)
        {
            if (oldIndex != newIndex)
            {
                NextIndex = newIndex;
                var actualWidth = PART_CurrentItem.ActualWidth;
                NextOffsetTransform.X = newIndex > oldIndex ? actualWidth : -actualWidth;
            }
            CurrentItem = this.GetItemAt(oldIndex);
            if (_animation != null)
            {
                return null;
            }
            if (newIndex >= 0 && newIndex < this.Items.Count)
            {
                return new Transition(oldIndex, newIndex);
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
            if (!this.EnsureTemplateParts())
            {
                return null;
            }
            if (_animation != null)
            {
                return null;
            }
            var actualWidth = PART_CurrentItem.ActualWidth;
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
            CurrentItem = this.GetItemAt(this.SelectedIndex);
            NextItem = null;
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
            CurrentTransform.X = 0;
            _animation = null;
        }

        private object GetItemAt(int index)
        {
            if (index < 0 || index >= this.Items.Count)
            {
                return null;
            }

            return this.Items[index];
        }

        private bool EnsureTemplateParts()
        {
            return this.PART_CurrentItem != null && this.PART_NextItem != null;
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex > 0;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex -= 1;
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex < (this.Items.Count - 1);
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex += 1;
        }
    }
}