using Aya.Notify;

namespace Aya.Sample.Notify
{
    public class ItemData
    {
        public int Key;
        public string RedPointKey;

        public bool IsNew;
        public NotifyNode NotifyNode;

        public void Init(int key)
        {
            Key = key;
            RedPointKey = "Main.Item.Item" + Key;
            NotifyNode = UNotify.GetNode(RedPointKey, active => IsNew = active, () => IsNew);
        }
    }
}