
namespace mayapeeker.Models.Interactivity
{
    public class InteractionMessage
    {
        public string Key { get; private set; }
        public object Content { get; private set; }

        public InteractionMessage(string key)
        {
            Key = key;
        }

        public InteractionMessage(string key, object content)
        {
            Key = key;
            Content = content;
        }
    }
}
