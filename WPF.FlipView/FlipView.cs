using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.FlipView
{
    using System.Windows.Media.Animation;

    public class FlipView : Selector
    {
        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));
        private readonly TranslateTransform _currentTransform = new TranslateTransform();
        private readonly TranslateTransform _nextTransform = new TranslateTransform();
        private bool _isAnimating = false;
        private ContentPresenter PART_CurrentItem;
        private ContentPresenter PART_NextItem;
        private Grid PART_Root;
        private FrameworkElement PART_Container;
        private ListBox PART_Index;
        private double fromValue = 0.0;
        private double elasticFactor = 1.0;
        private bool cancelManipulation = false;
        private int _nextIndex;

        public static readonly DependencyProperty TransitionTimeProperty = DependencyProperty.Register(
            "TransitionTime",
            typeof(TimeSpan),
            typeof(FlipView),
            new PropertyMetadata(TimeSpan.FromMilliseconds(300)));

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
            TouchDown += OnTouchDown;
            TouchMove += OnTouchMove;
            TouchUp += OnTouchUp;
        }

        public TimeSpan TransitionTime
        {
            get
            {
                return (TimeSpan)GetValue(TransitionTimeProperty);
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

        public TranslateTransform NextTransform
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
                return PART_CurrentItem.Content;
            }
            set
            {
                PART_CurrentItem.Content = value;
            }
        }

        public object NextItem
        {
            get
            {
                return PART_NextItem.Content;
            }
            set
            {
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
                if (value == _nextIndex)
                {
                    return;
                }
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

        private TouchPoint TouchStart { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_NextItem = this.GetTemplateChild("PART_NextItem") as ContentPresenter;
            this.PART_CurrentItem = this.GetTemplateChild("PART_CurrentItem") as ContentPresenter;
            this.PART_Root = this.GetTemplateChild("PART_Root") as Grid;
            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;
            this.PART_Index = this.GetTemplateChild("PART_Index") as ListBox;
        }

        private void OnTouchDown(object sender, TouchEventArgs e)
        {
            TouchStart = e.GetTouchPoint(this);
        }

        private void OnTouchMove(object sender, TouchEventArgs e)
        {
            if (TouchStart == null)
            {
                e.Handled = false;
                return;
            }
            var tp = e.GetTouchPoint(this);
            var delta = tp.Position - TouchStart.Position;
            if (Math.Abs(delta.X) < Math.Abs(delta.Y))
            {
                e.Handled = false;
                return;
            }
            InternalHandleTouchMove(delta);
            e.Handled = true;
        }

        /// <summary>
        /// Exposing this for tests
        /// </summary>
        /// <param name="delta"></param>
        internal void InternalHandleTouchMove(Vector delta)
        {
            CurrentTransform.X = delta.X;
            NextTransform.X = PART_CurrentItem.ActualWidth + delta.X;
            NextIndex = delta.X < 0 ? SelectedIndex + 1 : SelectedIndex - 1;
            var treshold = PART_CurrentItem.ActualWidth / 2;
            if (delta.X > treshold)
            {
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                }
            }
            if (delta.X < -treshold)
            {
                if (SelectedIndex < Items.Count)
                {
                    SelectedIndex++;
                }
            }
        }

        private void OnTouchUp(object sender, TouchEventArgs e)
        {
            _isAnimating = true;
            var animation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(50)));
            animation.Completed += this.OnAnimationCompleted;
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            double to = NextTransform.X < 0 ? -PART_CurrentItem.ActualWidth : PART_CurrentItem.ActualWidth;
            var nextAnimation = new DoubleAnimation(to, new Duration(TimeSpan.FromMilliseconds(50)));
            NextTransform.BeginAnimation(TranslateTransform.XProperty, nextAnimation);
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = (FlipView)d;
            if (((int)e.OldValue) == -1)
            {
                return;
            }
            flipView.AnimateTo((int)e.OldValue, (int)e.NewValue);
        }

        private void AnimateTo(int oldIndex, int newIndex)
        {
            if (!this.EnsureTemplateParts())
            {
                return;
            }
            NextIndex = newIndex;
            if (_isAnimating)
            {
                CurrentItem = this.GetItemAt(oldIndex);
                return;
            }
            var transition = new Transition(oldIndex, newIndex);
            if (newIndex >= 0 && newIndex < this.Items.Count)
            {
                this.RunSlideAnimation(transition);
            }
        }

        private void RefreshViewPort(int selectedIndex)
        {
            CurrentItem = this.GetItemAt(selectedIndex);
            NextItem = null;
        }

        public void RunSlideAnimation(Transition transition)
        {
            if (!this.EnsureTemplateParts())
            {
                return;
            }
            if (_isAnimating)
            {
                return;
            }
            _isAnimating = true;
            double toValue = transition.From < transition.To ? -this.ActualWidth : this.ActualWidth;
            NextTransform.BeginAnimation(TranslateTransform.XProperty, AnimationFactory.CreateAnimation(-1 * toValue, fromValue, TransitionTime));
            var animation = AnimationFactory.CreateAnimation(fromValue, toValue, TransitionTime);
            animation.Completed += this.OnAnimationCompleted;
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void OnAnimationCompleted(object sender, EventArgs args)
        {
            this.RefreshViewPort(this.SelectedIndex);
            CurrentTransform.BeginAnimation(TranslateTransform.XProperty, null);
            this._isAnimating = false;
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