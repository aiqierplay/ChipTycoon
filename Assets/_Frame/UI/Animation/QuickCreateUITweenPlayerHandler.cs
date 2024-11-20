#if UNITY_EDITOR
using Aya.TweenPro;
using UnityEditor;
using UnityEngine;

public class QuickCreateUITweenPlayerHandler : MonoBehaviour
{
    [MenuItem("GameObject/Create UTween Player (With UI Handler)", false, 0)]
    public static void CreateEmptyDefault(MenuCommand menuCommand)
    {
        var tweenPlayer = CreateEmpty(menuCommand, nameof(UTweenPlayer));
        var uiTweenHandler = tweenPlayer.gameObject.AddComponent<UITweenPlayerHandler>();
        uiTweenHandler.TweenPlayer = tweenPlayer;
    }

    internal static UTweenPlayer CreateEmpty(MenuCommand menuCommand, string name)
    {
        var go = new GameObject(name);
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        go.AddComponent<RectTransform>();
        go.transform.localPosition = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        var tweenPlayer = go.AddComponent<UTweenPlayer>();
        Selection.activeObject = go;
        return tweenPlayer;
    }
}

#endif