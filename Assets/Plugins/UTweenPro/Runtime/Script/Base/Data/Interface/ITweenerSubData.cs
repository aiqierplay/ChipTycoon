#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public interface ITweenerSubData
    {
        void Reset();

#if UNITY_EDITOR
        SerializedProperty DataProperty { get; set; }
        void DrawInspector();
#endif
    }
}