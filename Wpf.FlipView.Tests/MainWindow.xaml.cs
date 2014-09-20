using System.Windows;

namespace Wpf.FlipView.Tests
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Ink;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void InkCanvas_OnGesture(object sender, InkCanvasGestureEventArgs e)
        {
            var recognitionResults = e.GetGestureRecognitionResults();
            ArgsBox.Items.Insert(0, string.Format("recieved gesture: {0}", recognitionResults.Count));

            foreach (var gestureRecognitionResult in recognitionResults)
            {
                string gesture = string.Format("{0} confidence: {1}", gestureRecognitionResult.ApplicationGesture, gestureRecognitionResult.RecognitionConfidence);
                ArgsBox.Items.Insert(0, gesture);
            }
        }
        private void OnClear(object sender, RoutedEventArgs e)
        {
            ArgsBox.Items.Clear();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ArgsBox.Items.Insert(0,"Click");
        }
    }
}
