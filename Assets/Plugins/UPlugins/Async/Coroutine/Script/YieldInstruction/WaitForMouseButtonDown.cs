using System.Collections;
using UnityEngine;

namespace Aya.Async
{
    public class WaitForMouseButtonDown : CustomYieldInstruction
    {
        public int Button = 0;

        public override bool keepWaiting => !Input.GetMouseButtonDown(Button);

        public WaitForMouseButtonDown(int button = 0)
        {
            Button = button;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForMouseButtonDown(int button)
        {
            while (!Input.GetMouseButtonDown(button))
            {
                yield return null;
            }
        }
    }
}
