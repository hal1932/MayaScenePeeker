﻿using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class ModelBase : BindableBase
    {
        public ModelBase()
        {
            Messenger = new Coordination.InterModelMessengar(this);
        }


        protected Coordination.InterModelMessengar Messenger;
    }
}