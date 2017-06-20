namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GestureEventArgs : EventArgs
    {
        private readonly List<IGestureTracker> gestureTrackers = new List<IGestureTracker>();

        public GestureEventArgs(IGestureTracker tracker, Gesture gesture)
        {
            this.gestureTrackers.Add(tracker);
            this.Gesture = gesture;
        }

        public GestureEventArgs(IGestureTracker tracker, GestureEventArgs args)
        {
            this.gestureTrackers.AddRange(args.GestureTrackers);
            this.gestureTrackers.Add(tracker);
            this.Gesture = args.Gesture;
        }

        /// <summary>
        /// When event is routed the most recent tracker is last
        /// Typically you want to use the first tracker with an interpreter != null
        /// </summary>
        public IEnumerable<IGestureTracker> GestureTrackers => this.gestureTrackers;

        /// <summary>
        /// The detected gesture.
        /// </summary>
        public Gesture Gesture { get; }

        public IGestureInterpreter Interpreter => this.gestureTrackers.FirstOrDefault(x => x.Interpreter != null)?.Interpreter;
    }
}