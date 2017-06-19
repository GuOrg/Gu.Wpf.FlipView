namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;

    public class CompositeGestureTracker : IGestureTracker
    {
        private readonly ObservableCollection<IGestureTracker> gestureTrackers = new ObservableCollection<IGestureTracker>();
        private bool disposed = false;

        private UIElement inputElement;

        public CompositeGestureTracker()
        {
            this.GestureTrackers.CollectionChanged += this.GestureFindersOnCollectionChanged;
            this.Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public IGestureInterpreter Interpreter { get; set; }

        public ObservableCollection<IGestureTracker> GestureTrackers => this.gestureTrackers;

        public UIElement InputElement
        {
            get => this.inputElement;

            set
            {
                this.inputElement = value;
                foreach (var tracker in this.GestureTrackers.Where(x => x != null))
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern. 
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var gestureFinder in this.GestureTrackers)
                {
                    gestureFinder.Dispose();
                }
            }

            this.disposed = true;
        }

        protected virtual void OnGestured(Gesture e)
        {
            var handler = this.Gestured;
            if (handler != null)
            {
                handler(this, new GestureEventArgs(this, e));
            }
        }

        private void OnSubfinderGestured(object sender, GestureEventArgs e)
        {
            var handler = this.Gestured;
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
                    finder.InputElement = this.InputElement;
                    finder.Gestured += this.OnSubfinderGestured;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var finder in e.OldItems.OfType<IGestureTracker>())
                {
                    if (finder != null)
                    {
                        finder.InputElement = null;
                        finder.Gestured -= this.OnSubfinderGestured;
                    }
                }
            }
        }
    }
}