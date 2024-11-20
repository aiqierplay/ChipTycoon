using Aya.Notify;
using UnityEngine;

namespace Aya.Sample.Notify
{
    public class RedPointTreeCreator : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public void Init()
        {
            UNotify.CreateNode("Main");
            UNotify.CreateNode("Main.Item");
            UNotify.CreateNode("Main.Skill");
            UNotify.CreateNode("Main.Mail");
            UNotify.CreateNode("Main.Item.Item01");
        }
    }
}
