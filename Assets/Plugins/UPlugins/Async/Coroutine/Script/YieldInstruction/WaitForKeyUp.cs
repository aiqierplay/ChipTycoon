using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForKeyUp : CustomYieldInstruction
    {
        public KeyCode KeyCode;

        public override bool keepWaiting => !Input.GetKeyUp(KeyCode);

        public WaitForKeyUp(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForKeyUp(string keyCode)
        {
            while (!Input.GetKeyUp(keyCode))
            {
                yield return null;
            }
        }
    }
}