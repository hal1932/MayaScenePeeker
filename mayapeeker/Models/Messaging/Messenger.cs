using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models.Messaging
{
    class Messenger : Disposable
    {
        public delegate void OnMessageReceived(Message message);
        public event OnMessageReceived MessageReceived;


        public Messenger()
        {
            _messengerList.Add(this);
        }


        protected override void DisposeUnmanagedResource()
        {
            _messengerList.Remove(this);
        }


        public void AddMessageFilter(string messageKey)
        {
            _keyFilter.Add(messageKey);
        }


        public void DispatchMessage(Message message)
        {
            Task.Factory.StartNew(() => DispatchMessageImpl(message));
        }


        private void DispatchMessageImpl(Message message)
        {
            foreach (var messenger in _messengerList)
            {
                if (messenger != this)
                {
                    if (messenger._keyFilter.Count > 0)
                    {
                        if (!messenger._keyFilter.Contains(message.Key))
                        {
                            continue;
                        }
                    }

                    if (messenger.MessageReceived != null)
                    {
                        messenger.MessageReceived(message);
                    }
                }
            }
        }


        private static List<Messenger> _messengerList = new List<Messenger>();

        private List<string> _keyFilter = new List<string>();

    }
}
