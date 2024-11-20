using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForButtonDown : CustomYieldInstruction
    {
        public string ButtonName;

        public override bool keepWaiting => !Input.GetButtonDown(ButtonName);

        public WaitForButtonDown(string buttonName)
        {
            ButtonName = buttonName;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForButtonDown(string buttonName)
        {
            while (!Input.GetButtonDown(buttonName))
            {
                yield return null;
            }
        }
    }
}
