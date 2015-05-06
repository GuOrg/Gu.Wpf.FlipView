namespace Gu.Wpf.FlipView.Demo.MocksAndHelpers
{
    using System.Text;
    using System.Windows.Input;

    public static class ManipulationStringer
    {
        public static string ToUiString(this ManipulationDeltaEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("CumulativeManipulation: {0}", e.CumulativeManipulation.ToUiString());
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat(", DeltaManipulation: {0}", e.DeltaManipulation.ToUiString());
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat(", Velocities: {0}", e.Velocities);
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        public static string ToUiString(this ManipulationDelta delta)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Translation: ({0}, {1})", delta.Translation.X, delta.Translation.Y);
            stringBuilder.AppendFormat("Expansion: ({0}, {1})", delta.Expansion.X, delta.Expansion.Y);
            stringBuilder.AppendFormat("Scale: ({0}, {1})", delta.Scale.X, delta.Scale.Y);
            stringBuilder.AppendFormat("Rotation: {0}", delta.Rotation.ToString("F1"));
            return stringBuilder.ToString();
        }
    }
}
