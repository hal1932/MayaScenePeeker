using Microsoft.Practices.Prism.Mvvm;
using System;

namespace mayapeeker.ViewModels
{
    class ViewModelBase : BindableBase, IDisposable
    {
        public virtual void Initialize() { }


        #region IDisposable
        ~ViewModelBase()
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
        #endregion

    }
}
