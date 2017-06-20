namespace Gu.Wpf.FlipView.Internals
{
    using System;
    using System.Collections;

    internal class EmptyEnumerator : IEnumerator
    {
        public static readonly IEnumerator Instance = new EmptyEnumerator();

        private EmptyEnumerator()
        {
        }

        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public bool MoveNext() => false;

        public object Current => throw new InvalidOperationException();
    }
}