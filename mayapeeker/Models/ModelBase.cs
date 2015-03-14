
namespace mayapeeker.Models
{
    public class ModelBase : BindableBase
    {
        public ModelBase()
        {
            Messenger = new Interactivity.InteractionMessenger(this);
        }


        protected Interactivity.InteractionMessenger Messenger;
    }
}
