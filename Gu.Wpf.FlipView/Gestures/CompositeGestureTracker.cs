namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;

    public class CompositeGestureTracker : IGestureTracker
    {
        private readonly ObservableCollection<IGestureTracker> _gestureTrackers = new ObservableCollection<IGestureTracker>();
        private bool _disposed = false;

        private UIElement _inputElement;

        public CompositeGestureTracker()
        {
            GestureTrackers.CollectionChanged += GestureFindersOnCollectionChanged;
            Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public IGestureInterpreter Interpreter { get; set; }

        public ObservableCollection<IGestureTracker> GestureTrackers
        {
            get { return _gestureTrackers; }
        }

        public UIElement InputElement
        {
            get
            {
                return _inputElement;
            }
            set
            {
                _inputElement = value;
                foreach (var tracker in GestureTrackers.Where(x => x != null))
                {
                    tracker.InputElement = value;
                }
            }
        }

        /// <summary>
        /// Dispose(true); //I am calling you from Dispose, it's safe
        /// GC.SuppressFinalize(this); //Hey, GC: don't bother calling finalize later
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern. 
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var gestureFinder in GestureTrackers)
                {
                    gestureFinder.Dispose();
                }
            }
            _disposed = true;
        }

        protected virtual void OnGestured(Gesture e)
        {
            var handler = Gestured;
            if (handler != null)
            {
                handler(this, new GestureEventArgs(this, e));
            }
        }

        private void OnSubfinderGestured(object sender, GestureEventArgs e)
        {
            var handler = Gestured;
            if (handler != null)
            {
                handler(this, new GestureEventArgs(this, e));
            }
        }

        private void GestureFindersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var finder in e.NewItems.OfType<IGestureTracker>())
                {
                    finder.InputElement = InputElement;
                    finder.Gestured += OnSubfinderGestured;
                }
            }
            if (e.OldItems != null)
            {
                foreach (var finder in e.OldItems.OfType<IGestureTracker>())
                {
                    if (finder != null)
                    {
                        finder.InputElement = null;
                        finder.Gestured -= OnSubfinderGestured;
                    }
                }
            }
        }
    }
}