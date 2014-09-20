namespace WPF.FlipView
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GestureEventArgs : EventArgs
    {
        private readonly List<IGestureTracker> _gestureTrackers = new List<IGestureTracker>();

        public GestureEventArgs(IGestureTracker tracker, Gesture gesture)
        {
            this._gestureTrackers.Add(tracker);
            this.Gesture = gesture;
        }
        public GestureEventArgs(IGestureTracker tracker, GestureEventArgs args)
        {
            this._gestureTrackers.AddRange(args.GestureTrackers);
            this._gestureTrackers.Add(tracker);
            this.Gesture = args.Gesture;
        }

        /// <summary>
        /// When event is routed the most recent tracker is last
        /// Typically you want to use the first tracker with an interpreter != null
        /// </summary>
        public IEnumerable<IGestureTracker> GestureTrackers
        {
            get
            {
                return this._gestureTrackers;
            }
        }

        public Gesture Gesture { get; private set; }

        public IGestureInterpreter Interpreter
        {
            get
            {
                var tracker = _gestureTrackers.FirstOrDefault(x => x.Interpreter != null);
                if (tracker == null)
                {
                    return null;
                }
                return tracker.Interpreter;
            }
        }
    }
}