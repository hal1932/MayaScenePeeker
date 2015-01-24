using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models.Coordination
{
    public class InterModelMessage
    {
        public string Key { get; private set; }
        public object Content { get; private set; }

        public InterModelMessage(string key, object content)
        {
            Key = key;
            Content = content;
        }
    }
}
