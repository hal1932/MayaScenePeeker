using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models.Messaging
{
    class Message
    {
        public string Key { get; private set; }
        public object Content { get; private set; }

        public Message(string key, object content)
        {
            Key = key;
            Content = content;
        }
    }
}
