namespace Gu.Wpf.FlipView.Internals
{
    using System;
    using System.Collections;

    internal class EmptyEnumerator : IEnumerator
    {
        internal static readonly IEnumerator Instance = new EmptyEnumerator();

        private EmptyEnumerator()
        {
        }

        /// <inheritdoc />
        public object Current => throw new InvalidOperationException();

        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public bool MoveNext() => false;
    }
}