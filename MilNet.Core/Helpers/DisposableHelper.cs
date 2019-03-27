using System;

namespace MilNet.Core.Helpers
{
    /// <summary>Disposable helper</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public sealed class DisposableHelper : IDisposable
    {
        private Action end;

        /// <summary>Constructor</summary>
        /// <remarks>Write the <paramref name="begin"/> action</remarks>
        /// <param name="begin">Begin action</param>
        /// <param name="end">End action</param>
        /// <exception cref="ArgumentNullException"/>
        public DisposableHelper(Action begin, Action end)
        {
            if (begin == null)
                throw new ArgumentNullException(nameof(begin));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            this.end = end;
            begin();
        }

        /// <summary>Dispose</summary>
        /// <remarks>Write the end action</remarks>
        public void Dispose() => end();
    }
}
