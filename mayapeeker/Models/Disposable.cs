using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class Disposable : ModelBase
    {
        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeManagedResource() { }
        protected virtual void DisposeUnmanagedResource() { }

        private void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                if (_disposed) return;
                _disposed = true;

                if (disposing)
                {
                    DisposeManagedResource();
                }

                DisposeUnmanagedResource();
            }
        }

        private bool _disposed;
        private object _lockObj = new object();
    }
}
