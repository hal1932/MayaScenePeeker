using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mayapeeker.Models.Interactivity
{
    public class InteractionMessageReceivedEventArgs : EventArgs
    {
        public InteractionMessage Message { get; private set; }

        public InteractionMessageReceivedEventArgs(InteractionMessage message)
        {
            Message = message;
        }
    }


    /// <summary>
    /// M/VM の間でメッセージの受信だけを行う
    /// </summary>
    public class InteractionMessageListener : IDisposable
    {
        public event EventHandler<InteractionMessage> MessageReceived;


        public InteractionMessageListener()
        {
            InteractionMessengar.AddListener(this);
        }


        #region IDisposable
        ~InteractionMessageListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_disposed) return;
                _disposed = true;

                InteractionMessengar.RemoveListener(this);
            }
        }

        private bool _disposed;
        #endregion


        public void AddMessageFilter(string filter)
        {
            _filterList.Add(filter);
        }


        internal void OnPeekMessage(object sender, InteractionMessage message)
        {
            if (MessageReceived == null) return;
            if (_filterList.Count > 0 && !_filterList.Contains(message.Key)) return;

            MessageReceived(sender, message);
        }


        private List<string> _filterList = new List<string>();

    }


    /// <summary>
    /// M/VM の間でメッセージの送受信を行う
    /// </summary>
    public class InteractionMessengar : IDisposable
    {
        public event EventHandler<InteractionMessage> MessageReceived;


        public InteractionMessengar(ModelBase sender)
        {
            _messengerList.Add(this);
            _sender = sender;
        }


        #region IDisposable
        ~InteractionMessengar()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_disposed) return;
                _disposed = true;

                _messengerList.Remove(this);
            }
        }

        private bool _disposed;
        #endregion


        public void AddMessageFilter(string messageKey)
        {
            _keyFilter.Add(messageKey);
        }


        public void DispatchMessage(InteractionMessage message)
        {
            Task.Factory.StartNew(() => DispatchMessageImpl(message));
        }


        internal static void AddListener(InteractionMessageListener listener)
        {
            _listenerList.Add(listener);
        }

        internal static void RemoveListener(InteractionMessageListener listener)
        {
            _listenerList.Remove(listener);
        }


        private void DispatchMessageImpl(InteractionMessage message)
        {
            foreach (var messenger in _messengerList)
            {
                if (messenger == this) continue;
                if (messenger.MessageReceived == null) continue;

                if (messenger._keyFilter.Count > 0)
                {
                    if (!messenger._keyFilter.Contains(message.Key))
                    {
                        continue;
                    }
                }
                messenger.MessageReceived(_sender, message);
            }

            foreach (var listener in _listenerList)
            {
                listener.OnPeekMessage(_sender, message);
            }
        }


        private static List<InteractionMessengar> _messengerList = new List<InteractionMessengar>();
        private static List<InteractionMessageListener> _listenerList = new List<InteractionMessageListener>();

        private ModelBase _sender;
        private List<string> _keyFilter = new List<string>();

    }
}
