namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class DebugView : UserControl
    {
        public DebugView()
        {
            this.InitializeComponent();
        }

        private void InkCanvas_OnGesture(object sender, InkCanvasGestureEventArgs e)
        {
            var recognitionResults = e.GetGestureRecognitionResults();
            this.ArgsBox.Items.Insert(0, $"received gesture: {recognitionResults.Count}");

            foreach (var gestureRecognitionResult in recognitionResults)
            {
                string gesture =
                    $"{gestureRecognitionResult.ApplicationGesture} confidence: {gestureRecognitionResult.RecognitionConfidence}";
                this.ArgsBox.Items.Insert(0, gesture);
            }
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            this.ArgsBox.Items.Clear();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.ArgsBox.Items.Insert(0, "Click");
        }
    }
}
