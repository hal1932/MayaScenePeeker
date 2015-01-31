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
    /// メッセージの送受信を行う
    /// </summary>
    public class InteractionMessengar : IDisposable
    {
        public event EventHandler<InteractionMessage> MessageReceived;


        public InteractionMessengar()
        {
            _listenOnly = true;
            _messengerList.Add(this);
        }


        public InteractionMessengar(ModelBase sender)
        {
            _listenOnly = false;
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


        public void SetAsDispatcher(object sender)
        {
            if (sender != null)
            {
                _listenOnly = false;
                _sender = sender;
            }
            else
            {
                _listenOnly = true;
                _sender = null;
            }
        }


        public void AddMessageFilter(string messageKey)
        {
            _keyFilter.Add(messageKey);
        }


        public void DispatchMessage(InteractionMessage message)
        {
            if (_listenOnly) throw new InvalidOperationException("not allowed to dispatch message: listen only");

            Task.Factory.StartNew(() => DispatchMessageImpl(message));
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
        }



        private static List<InteractionMessengar> _messengerList = new List<InteractionMessengar>();

        private bool _listenOnly;
        private object _sender;
        private List<string> _keyFilter = new List<string>();

    }
}
