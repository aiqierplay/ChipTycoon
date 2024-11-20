#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public partial class UTweenPlayer
    {
        [MenuItem("GameObject/Create UTween Player", false, 0)]
        internal static void CreateEmptyUTweenPlayer(MenuCommand menuCommand)
        {
            CreateEmpty(menuCommand, nameof(UTweenPlayer));
        }

        // [MenuItem("GameObject/Create UTween Animation/Position", false, 200)]
        // internal static void CreateUTweenPlayerPosition(MenuCommand menuCommand)
        // {
        //     CreateWithTweener<TweenPosition, Transform>(menuCommand, "Tween Position");
        // }
        //
        // [MenuItem("GameObject/Create UTween Animation/EulerAngles", false, 201)]
        // internal static void CreateUTweenPlayerEulerAngles(MenuCommand menuCommand)
        // {
        //     CreateWithTweener<TweenEulerAngles, Transform>(menuCommand, "Tween EulerAngles");
        // }
        //
        // [MenuItem("GameObject/Create UTween Animation/Scale", false, 202)]
        // internal static void CreateUTweenPlayerScale(MenuCommand menuCommand)
        // {
        //     CreateWithTweener<TweenScale, Transform>(menuCommand, "Tween Scale");
        // }

        internal static UTweenPlayer CreateWithTweener<TTweener, TTarget>(MenuCommand menuCommand, string name) where TTweener : Tweener<TTarget>, new() where TTarget : Component
        {
            var tweenPlayer = CreateEmpty(menuCommand, name);
            var tweener = UTween.Create<TTweener>();
            tweenPlayer.Animation.Join(tweener);
            tweener.Target = tweenPlayer.GetComponentInChildren<TTarget>();
            return tweenPlayer;
        }

        internal static UTweenPlayer CreateEmpty(MenuCommand menuCommand, string name)
        {
            var go = new GameObject(name);
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            go.transform.localPosition = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            var tweenPlayer = go.AddComponent<UTweenPlayer>();
            Selection.activeObject = go;
            return tweenPlayer;
        }
    }
}
#endif