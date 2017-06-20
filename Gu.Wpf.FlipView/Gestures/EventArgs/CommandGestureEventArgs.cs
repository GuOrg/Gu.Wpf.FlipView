namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows.Input;

    /// <summary>
    /// A gesture triggered by a command.
    /// </summary>
    public class CommandGestureEventArgs : GestureEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGestureEventArgs"/> class.
        /// </summary>
        /// <param name="type">The interpreted gesture</param>
        /// <param name="commandArgs">The command event args that triggered the gesture.</param>
        public CommandGestureEventArgs(GestureType type, ExecutedRoutedEventArgs commandArgs)
            : base(type)
        {
            this.CommandArgs = commandArgs;
        }

        /// <summary>
        /// Gets the command event args that triggered the gesture.
        /// </summary>
        public ExecutedRoutedEventArgs CommandArgs { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Command: {this.CommandArgs.Command}";
        }
    }
}