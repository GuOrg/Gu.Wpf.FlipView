namespace WPF.FlipView
{
    using System;
    using System.Collections.Generic;

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
       
        public IEnumerable<IGestureTracker> GestureTrackers
        {
            get
            {
                return this._gestureTrackers;
            }
        }

        public Gesture Gesture { get; private set; }
    }
}