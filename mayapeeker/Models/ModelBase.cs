
namespace mayapeeker.Models
{
    public class ModelBase : BindableBase
    {
        public ModelBase()
        {
            Messenger = new Interactivity.InteractionMessengar(this);
        }


        protected Interactivity.InteractionMessengar Messenger;
    }
}
