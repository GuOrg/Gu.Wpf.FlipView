#pragma warning disable CA1051 // Do not declare visible instance fields
namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    /// <summary>
    /// A point in a gesture, can be a timestamped position in a mouse move.
    /// </summary>
    public readonly struct GesturePoint : IEquatable<GesturePoint>
    {
        /// <summary>
        /// The position.
        /// </summary>
        public readonly Point Point;

        /// <summary>
        /// The time when the input occurred.
        /// </summary>
        public readonly int Time;

        /// <summary>
        /// Initializes a new instance of the <see cref="GesturePoint"/> struct.
        /// </summary>
        /// <param name="point">The position.</param>
        /// <param name="time">The time when the input occurred.</param>
        public GesturePoint(Point point, int time)
        {
            this.Point = point;
            this.Time = time;
        }

        public static bool operator ==(GesturePoint left, GesturePoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GesturePoint left, GesturePoint right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(GesturePoint other)
        {
            return this.Point.Equals(other.Point) && this.Time == other.Time;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is GesturePoint other && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Point.GetHashCode() * 397) ^ this.Time;
            }
        }
    }
}
