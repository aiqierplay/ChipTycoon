using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForButtonUp : CustomYieldInstruction
    {
        public string ButtonName;

        public override bool keepWaiting => !Input.GetButtonUp(ButtonName);

        public WaitForButtonUp(string buttonName)
        {
            ButtonName = buttonName;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForButtonUp(string buttonName)
        {
            while (!Input.GetButtonUp(buttonName))
            {
                yield return null;
            }
        }
    }
}