#if UNITY_EDITOR
using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public struct GUICheckChangeArea : IDisposable
    {
        private bool _end;
        private bool _changed;
        private Object _target;

        public bool Changed
        {
            get
            {
                if (_end) return _changed;
                _end = true;
                _changed = EditorGUI.EndChangeCheck();
                if (_changed && _target)
                {
                    Undo.RecordObject(_target, _target.name);
                }

                return _changed;
            }
        }

        public static GUICheckChangeArea Create(Object target = null)
        {
            EditorGUI.BeginChangeCheck();
            return new GUICheckChangeArea
            {
                _end = false,
                _changed = false,
                _target = target
            };
        }

        void IDisposable.Dispose()
        {
            if (_end) return;
            _end = true;
            _changed = EditorGUI.EndChangeCheck();
        }
    }
}
#endif