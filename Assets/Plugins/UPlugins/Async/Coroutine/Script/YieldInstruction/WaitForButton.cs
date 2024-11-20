using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForButton : CustomYieldInstruction
    {
        public string ButtonName;

        public override bool keepWaiting => !Input.GetButton(ButtonName);

        public WaitForButton(string buttonName)
        {
            ButtonName = buttonName;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForButton(string buttonName)
        {
            while (!Input.GetButton(buttonName))
            {
                yield return null;
            }
        }
    }
}
