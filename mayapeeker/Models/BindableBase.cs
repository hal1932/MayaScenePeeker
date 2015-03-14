using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class BindableBase : INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;


        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            Debug.Assert(propertyName != null);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

            List<Action<PropertyChangedEventArgs>> handlerList;
            if (_propertyChangedEventHandlerListDic.TryGetValue(propertyName, out handlerList))
            {
                var e = new PropertyChangedEventArgs(propertyName);
                foreach (var h in handlerList)
                {
                    h(e);
                }
            }
        }
        #endregion


        #region IDisposable
        ~BindableBase()
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
                    RemovePropertyChangedHandler();
                }

                DisposeUnmanagedResource();
            }
        }

        private bool _disposed;
        private object _lockObj = new object();
        #endregion


        public void AddPropertyChangedHandler(string propertyName, Action<PropertyChangedEventArgs> handler)
        {
            List<Action<PropertyChangedEventArgs>> handlerList;
            if (_propertyChangedEventHandlerListDic.TryGetValue(propertyName, out handlerList))
            {
                handlerList.Add(handler);
            }
            else
            {
                _propertyChangedEventHandlerListDic[propertyName] = new List<Action<PropertyChangedEventArgs>>() { handler };
            }
        }


        public void RemovePropertyChangedHandler(string propertyName = null)
        {
            if (propertyName == null)
            {
                _propertyChangedEventHandlerListDic.Clear();
            }
            else
            {
                _propertyChangedEventHandlerListDic.Remove(propertyName);
            }
        }


        private Dictionary<string, List<Action<PropertyChangedEventArgs>>> _propertyChangedEventHandlerListDic = new Dictionary<string,List<Action<PropertyChangedEventArgs>>>();

    }
}
