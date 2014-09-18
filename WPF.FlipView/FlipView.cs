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

        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));

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

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register("ArrowPlacement", typeof(ArrowPlacement), typeof(FlipView), new PropertyMetadata(default(ArrowPlacement)));

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
            Loaded += this.OnLoaded;
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

        private int NextIndex
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
                    var nextItem = this.GetItemAt(_nextIndex);
                    this.PART_NextItem.SetCurrentValue(ContentPresenter.ContentProperty, nextItem);
                }
                else
                {
                    this.PART_NextItem.SetCurrentValue(ContentPresenter.ContentProperty, null);
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

            //this.PART_Root.TouchDown += PART_Root_TouchDown;
            //this.PART_Root.ManipulationStarted += this.OnRootManipulationStarted;
            //this.PART_Root.ManipulationStarting += this.OnRootManipulationStarting;
            //this.PART_Root.ManipulationDelta += this.OnRootManipulationDelta;
            //this.PART_Root.ManipulationCompleted += this.OnRootManipulationCompleted;
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
            PART_CurrentItem.RenderTransform = new TranslateTransform(delta.X, 0);
            var treshold = PART_CurrentItem.ActualWidth/2;
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

        private void PART_Root_TouchDown(object sender, TouchEventArgs e)
        {
            TouchPoint pt = e.GetTouchPoint((UIElement)sender);
            Point point = new Point(pt.Position.X, pt.Position.Y);

            HitTestResult result = VisualTreeHelper.HitTest((UIElement)sender, point);

            if (result != null && result.VisualHit != null)
            {
                DependencyObject button;
                cancelManipulation = HasButtonParent(result.VisualHit, out button);
            }
        }

        private void OnTouchUp(object sender, TouchEventArgs e)
        {
            PART_CurrentItem.RenderTransform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(50))));
            RefreshViewPort(SelectedIndex);
        }

        private void OnRootManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            this.fromValue = e.TotalManipulation.Translation.X;
            int oldIndex = this.SelectedIndex;
            if (this.fromValue > 2)
            {
                if (this.SelectedIndex > 0)
                {
                    this.SelectedIndex -= 1;
                }
            }
            else if (this.fromValue < -2)
            {
                if (this.SelectedIndex < this.Items.Count - 1)
                {
                    this.SelectedIndex += 1;
                }
            }

            //if (this.elasticFactor < 1)
            //{
            //    var direction = new Transition(SelectedIndex, ((Transform)this.PART_Root.RenderTransform).Value.OffsetX);
            //    this.RunSlideAnimation(direction);
            //}

            this.elasticFactor = 1.0;
        }

        private void OnRootManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (!(this.PART_Root.RenderTransform is MatrixTransform))
            {
                this.PART_Root.RenderTransform = new MatrixTransform();
            }

            Matrix matrix = ((MatrixTransform)this.PART_Root.RenderTransform).Matrix;
            var delta = e.DeltaManipulation;

            if ((this.SelectedIndex == 0 && delta.Translation.X > 0 && this.elasticFactor > 0)
                || (this.SelectedIndex == this.Items.Count - 1 && delta.Translation.X < 0 && this.elasticFactor > 0))
            {
                this.elasticFactor -= 0.05;
            }

            matrix.Translate(delta.Translation.X * elasticFactor, 0);
            this.PART_Root.RenderTransform = new MatrixTransform(matrix);

            e.Handled = true;
        }

        private void OnRootManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void OnRootManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this.PART_Container;
            e.Handled = true;
            if (cancelManipulation)
            {
                e.Cancel();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.SelectedIndex > -1)
            {
                this.RefreshViewPort(this.SelectedIndex);
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flipView = (FlipView)d;
            flipView.AnimateTo((int)e.OldValue, (int)e.NewValue);
        }

        private bool HasButtonParent(DependencyObject obj, out DependencyObject button)
        {
            var parent = VisualTreeHelper.GetParent(obj);

            if ((parent != null) && (parent is ButtonBase) == false)
            {
                return HasButtonParent(parent, out button);
            }

            button = parent;
            return parent != null;
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
                this.PART_CurrentItem.Content = this.GetItemAt(oldIndex);
                return;
            }
            var transition = new Transition(oldIndex, newIndex);
            if (newIndex >= 0 && newIndex < this.Items.Count)
            {
                this.PART_NextItem.Content = this.GetItemAt(newIndex);
                this.RunSlideAnimation(transition);
            }
        }

        private void RefreshViewPort(int selectedIndex)
        {
            if (!this.EnsureTemplateParts())
            {
                return;
            }

            var currentItem = this.GetItemAt(selectedIndex);

            this.PART_CurrentItem.RenderTransform = new TranslateTransform();
            this.PART_CurrentItem.Content = currentItem;

            this.PART_NextItem.SetCurrentValue(ContentPresenter.ContentProperty, null);
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
            var currentItem = this.PART_CurrentItem;
            if (!(currentItem.RenderTransform is TranslateTransform))
            {
                currentItem.RenderTransform = new TranslateTransform();
            }
            var toItem = this.PART_NextItem;
            if (!(toItem.RenderTransform is TranslateTransform))
            {
                toItem.RenderTransform = new TranslateTransform();
            }

            toItem.RenderTransform.BeginAnimation(TranslateTransform.XProperty, AnimationFactory.CreateAnimation(-1 * toValue, fromValue, TransitionTime));
            var animation = AnimationFactory.CreateAnimation(fromValue, toValue, TransitionTime);
            animation.Completed += this.OnAnimationCompleted;
            currentItem.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void OnAnimationCompleted(object sender, EventArgs args)
        {
            this.RefreshViewPort(this.SelectedIndex);
            this.PART_CurrentItem.RenderTransform.BeginAnimation(TranslateTransform.XProperty, null);
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
            return this.PART_CurrentItem != null && this.PART_NextItem != null && this.PART_Root != null && this.PART_Index != null;
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