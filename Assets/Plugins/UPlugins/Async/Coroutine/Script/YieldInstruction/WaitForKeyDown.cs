using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForKeyDown : CustomYieldInstruction
    {
        public KeyCode KeyCode;

        public override bool keepWaiting => !Input.GetKeyDown(KeyCode);

        public WaitForKeyDown(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForKeyDown(string keyCode)
        {
            while (!Input.GetKeyDown(keyCode))
            {
                yield return null;
            }
        }
    }
}