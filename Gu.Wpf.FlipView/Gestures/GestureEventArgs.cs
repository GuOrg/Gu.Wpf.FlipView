namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GestureEventArgs : EventArgs
    {
        private readonly List<IGestureTracker> _gestureTrackers = new List<IGestureTracker>();

        public GestureEventArgs(IGestureTracker tracker, Gesture gesture)
        {
            _gestureTrackers.Add(tracker);
            Gesture = gesture;
        }
        public GestureEventArgs(IGestureTracker tracker, GestureEventArgs args)
        {
            _gestureTrackers.AddRange(args.GestureTrackers);
            _gestureTrackers.Add(tracker);
            Gesture = args.Gesture;
        }

        /// <summary>
        /// When event is routed the most recent tracker is last
        /// Typically you want to use the first tracker with an interpreter != null
        /// </summary>
        public IEnumerable<IGestureTracker> GestureTrackers
        {
            get
            {
                return _gestureTrackers;
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