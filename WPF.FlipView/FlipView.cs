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
    [TemplatePart(Name = PART_RootName, Type = typeof(Panel))]
    [TemplatePart(Name = PART_CurrentItemName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_NextItemName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_ContainermName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_IndexName, Type = typeof(Selector))]
    public class FlipView : Selector
    {
        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));

        public static readonly DependencyProperty TransitionTimeProperty = DependencyProperty.Register(
            "TransitionTime",
            typeof(int),
            typeof(FlipView),
            new PropertyMetadata(300));

        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity",
            typeof(double),
            typeof(FlipView),
            new PropertyMetadata(0.0));

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
            new PropertyMetadata(new SolidColorBrush(Colors.DeepSkyBlue)));

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

        public static readonly DependencyProperty NextItemProperty = DependencyProperty.Register(
            "NextItem",
            typeof(object),
            typeof(FlipView),
            new PropertyMetadata(null));

        private readonly TranslateTransform _currentTransform = new TranslateTransform();
        private readonly TranslateTransform _nextOffsetTransform = new TranslateTransform();
        private readonly TransformGroup _nextTransform;
        private AnimationTimeline _animation;
        private int _nextIndex = -1;
        private bool _isPanning;
        private const string PART_NextItemName = "PART_NextItem";
        private const string PART_CurrentItemName = "PART_CurrentItem";
        private const string PART_RootName = "PART_Root";
        private const string PART_ContainermName = "PART_Container";
        private const string PART_IndexName = "PART_Index";
        private ContentPresenter PART_CurrentItem;
        private ContentPresenter PART_NextItem;
        private Panel PART_Root;
        private FrameworkElement PART_Container;
        private Selector PART_Index;

        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
        }

        public FlipView()
        {
            this.CommandBindings.Add(new CommandBinding(NextCommand, this.OnNextExecuted, this.OnNextCanExecute));
            this.CommandBindings.Add(new CommandBinding(PreviousCommand, this.OnPreviousExecuted, this.OnPreviousCanExecute));

            ManipulationDelta += (sender, args) =>
            {
                //if (args.ManipulationContainer == PART_Root)
                //{
                //    var delta = args.CumulativeManipulation.Translation;
                //    if (Math.Abs(delta.X) < 3)
                //    {
                //        args.Handled = false;
                //        return;
                //    }
                //    if (Math.Abs(args.Velocities.LinearVelocity.X) < MinSwipeVelocity)
                //    {
                //        args.Handled = false;
                //        CurrentTransform.X = 0;
                //        return;
                //    }
                //    if (Math.Abs(delta.X) < Math.Abs(delta.Y))
                //    {
                //        args.Handled = false;
                //        return;
                //    }
                //    else
                //    {
                //        OnPan(delta);
                //        args.Handled = true;
                //    }
                //}
                //args.Handled = false;
            };

            ManipulationCompleted += (sender, args) =>
            {
                //if (args.ManipulationContainer == PART_Root && _isPanning)
                //{
                //    OnPanEnded(args.TotalManipulation.Translation, args.FinalVelocities.LinearVelocity);
                //    args.Handled = true;
                //}
                //else
                //{
                //    args.Handled = false;
                //}
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

        public double MinSwipeVelocity
        {
            get { return (double)GetValue(MinSwipeVelocityProperty); }
            set { SetValue(MinSwipeVelocityProperty, value); }
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

        public object NextItem
        {
            get { return (object)GetValue(NextItemProperty); }
            set { SetValue(NextItemProperty, value); }
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
                   SetCurrentValue(NextItemProperty , this.GetItemAt(_nextIndex));
                    var sign = NextIndex > SelectedIndex ? 1 : -1;
                    _nextOffsetTransform.X = sign * PART_CurrentItem.ActualWidth;
                }
                else
                {
                    SetCurrentValue(NextItemProperty, null);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_NextItem = this.GetTemplateChild("PART_NextItem") as ContentPresenter;
            this.PART_CurrentItem = this.GetTemplateChild("PART_CurrentItem") as ContentPresenter;
            this.PART_Root = this.GetTemplateChild("PART_Root") as Panel;
            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;
            this.PART_Index = this.GetTemplateChild("PART_Index") as Selector;
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

            var animation = TransitionTo(oldIndex, SelectedIndex);
            _isPanning = false;
            AnimateCurrentTransform(animation);
        }

        /// <summary>
        /// Exposed for tests
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <returns></returns>
        internal AnimationTimeline TransitionTo(int oldIndex, int newIndex)
        {
            NextIndex = newIndex;
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
            SelectedIndex = NextIndex;
            NextIndex = int.MinValue;
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
            e.CanExecute =this.SelectedIndex > 0;
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
            e.Handled = true; // When am I supposed to set this?
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var animation = TransitionTo(SelectedIndex, SelectedIndex + 1);
            AnimateCurrentTransform(animation);
            e.Handled = true;
        }
    }
}