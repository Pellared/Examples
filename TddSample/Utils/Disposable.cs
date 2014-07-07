﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class Disposable : IDisposable
    {
        const int DisposedFlag = 1;
        int _isDisposed;

#if DEBUG
        ~Disposable()
        {
            DisposeUnmanaged();
            Debug.Fail(GetType() + " in not disposed");
        }
#endif

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose is implemented correctly, FxCop just doesn't see it.")]
        public void Dispose()
        {
            var isDisposed = _isDisposed;
            Interlocked.CompareExchange(ref _isDisposed, DisposedFlag, isDisposed);
            if (isDisposed == 0)
            {
                DisposeManaged();
                DisposeUnmanaged();
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void DisposeManaged()
        {
        }

        protected virtual void DisposeUnmanaged()
        {
        }

        /// <summary>
        /// Returns true if the current instance has been disposed; otherwise false;
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                Thread.MemoryBarrier();
                return _isDisposed == DisposedFlag;
            }
        }
    }
}
