using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForMouseButton : CustomYieldInstruction
    {
        public int Button = 0;

        public override bool keepWaiting => !Input.GetMouseButton(Button);

        public WaitForMouseButton(int button = 0)
        {
            Button = button;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForMouseButton(int button)
        {
            while (!Input.GetMouseButton(button))
            {
                yield return null;
            }
        }
    }
}
