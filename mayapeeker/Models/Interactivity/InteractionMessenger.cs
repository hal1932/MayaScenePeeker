using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mayapeeker.Models.Interactivity
{
    /// <summary>
    /// メッセージの送受信を行う
    /// </summary>
    public class InteractionMessenger : IDisposable
    {
        public InteractionMessenger()
        {
            _listenOnly = true;
            _messengerList.Add(this);
        }


        public InteractionMessenger(ModelBase sender)
        {
            _listenOnly = false;
            _messengerList.Add(this);
            _sender = sender;
        }


        #region IDisposable
        ~InteractionMessenger()
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


        public void AddMessageHandler(string messageKey, Action<InteractionMessage> handler)
        {
            var info = new HandlerInfo(this, handler);

            List<HandlerInfo> handlerInfoList;
            if (_handlerDic.TryGetValue(messageKey, out handlerInfoList))
            {
                handlerInfoList.Add(info);
            }
            else
            {
                _handlerDic[messageKey] = new List<HandlerInfo>() { info };
            }
        }


        public void DispatchMessage(InteractionMessage message)
        {
            if (_listenOnly) throw new InvalidOperationException("not allowed to dispatch message: listen only");

            Task.Factory.StartNew(() => DispatchMessageImpl(message));
        }


        private void DispatchMessageImpl(InteractionMessage message)
        {
            List<HandlerInfo> handlerInfoList;
            if (_handlerDic.TryGetValue(message.Key, out handlerInfoList))
            {
                foreach (var info in handlerInfoList)
                {
                    if (info.Instance == this) continue;
                    info.Handler(message);
                }
            }
        }



        private static List<InteractionMessenger> _messengerList = new List<InteractionMessenger>();

        private bool _listenOnly;
        private object _sender;

        private class HandlerInfo
        {
            public InteractionMessenger Instance { get; private set; }
            public Action<InteractionMessage> Handler { get; private set; }
            public HandlerInfo(InteractionMessenger instance, Action<InteractionMessage> handler)
            {
                Instance = instance;
                Handler = handler;
            }
        }
        private Dictionary<string, List<HandlerInfo>> _handlerDic = new Dictionary<string, List<HandlerInfo>>();

    }
}
