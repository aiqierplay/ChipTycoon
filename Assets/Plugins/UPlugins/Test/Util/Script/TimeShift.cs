/////////////////////////////////////////////////////////////////////////////
//
//  Script   : TimeShift.cs
//  Info     : 时间变速
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2017
//
/////////////////////////////////////////////////////////////////////////////
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aya.Test
{
    public class TimeShift : MonoBehaviour
    {
        [Range(0, 10f)] public float Speed = 1f;

        void Update()
        {
            SetSpeed(Speed);
        }

        private void SetSpeed(float speed)
        {
            UnityEngine.Time.timeScale = speed;
            Speed = speed;
        }

#if UNITY_EDITOR
        [Button("0 x")]
        [GUIColor(1f, 0.5f, 0.5f)]
        public void SetStop()
        {
            SetSpeed(0);
        }

        [Button("0.1 x")]
        public void SetSlower()
        {
            SetSpeed(0.1f);
        }

        [Button("0.5 x")]
        public void SetSlow()
        {
            SetSpeed(0.5f);
        }

        [Button("1 x")]
        [GUIColor(0.5f, 1f, 1f)]
        public void SetNormal()
        {
            SetSpeed(1f);
        }

        [Button("1.5 x")]
        public void SetFast()
        {
            SetSpeed(1.5f);
        }

        [Button("2 x")]
        public void SetFaster()
        {
            SetSpeed(2f);
        }

        [Button("5 x")]
        public void SetFasterer()
        {
            SetSpeed(5f);
        }

        [Button("10 x")]
        public void SetFastest()
        {
            SetSpeed(10f);
        }
#endif
    }

}
