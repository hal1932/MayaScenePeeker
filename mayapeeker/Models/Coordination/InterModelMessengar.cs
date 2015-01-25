using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models.Coordination
{
    public class InterModelMessageReceivedEventArgs : EventArgs
    {
        public InterModelMessage Message { get; private set; }

        public InterModelMessageReceivedEventArgs(InterModelMessage message)
        {
            Message = message;
        }
    }


    public class InterModelMessengar : IDisposable
    {
        public event EventHandler<InterModelMessage> MessageReceived;


        public InterModelMessengar(ModelBase sender)
        {
            _messengerList.Add(this);
            _sender = sender;
        }


        #region IDisposable
        ~InterModelMessengar()
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


        public void DispatchMessage(InterModelMessage message)
        {
            Task.Factory.StartNew(() => DispatchMessageImpl(message));
        }


        private void DispatchMessageImpl(InterModelMessage message)
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
        }


        private static List<InterModelMessengar> _messengerList = new List<InterModelMessengar>();

        private ModelBase _sender;
        private List<string> _keyFilter = new List<string>();

    }
}
