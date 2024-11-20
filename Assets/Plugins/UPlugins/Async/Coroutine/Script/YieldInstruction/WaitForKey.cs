using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForKey : CustomYieldInstruction
    {
        public KeyCode KeyCode;

        public override bool keepWaiting => !Input.GetKey(KeyCode);

        public WaitForKey(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForKey(string keyCode)
        {
            while (!Input.GetKey(keyCode))
            {
                yield return null;
            }
        }
    }
}
