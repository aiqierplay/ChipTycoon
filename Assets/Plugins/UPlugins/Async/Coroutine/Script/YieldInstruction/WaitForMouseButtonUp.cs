using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForMouseButtonUp : CustomYieldInstruction
    {
        public int Button = 0;

        public override bool keepWaiting => !Input.GetMouseButtonUp(Button);

        public WaitForMouseButtonUp(int button = 0)
        {
            Button = button;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForMouseButtonUp(int button)
        {
            while (!Input.GetMouseButtonUp(button))
            {
                yield return null;
            }
        }
    }
}
