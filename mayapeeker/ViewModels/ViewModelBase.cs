using mayapeeker.Models;
using mayapeeker.Models.Interactivity;
using System;

namespace mayapeeker.ViewModels
{
    class ViewModelBase : BindableBase
    {
        public virtual void Initialize() { }


        // デフォルトでは受信のみ
        protected InteractionMessenger Messenger = new InteractionMessenger();

    }
}
