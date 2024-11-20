#if UNITY_EDITOR
using Aya.Extension;
using Aya.Render;
using UnityEditor;
using UnityEngine;

public static class QuickCreateGameObject
{
    [MenuItem("GameObject/Create GameObject/Default", false, 0)]
    public static void CreateEmptyDefault(MenuCommand menuCommand)
    {
        CreateEmptyWithName(menuCommand, nameof(GameObject));
    }

    [MenuItem("GameObject/Create GameObject/Trans", false, 1000)]
    public static void CreateEmptyTrans(MenuCommand menuCommand)
    {
        CreateEmptyWithName(menuCommand, "Trans");
    }

    [MenuItem("GameObject/Create GameObject/Renderer", false, 1000)]
    public static void CreateEmptyRenderer(MenuCommand menuCommand)
    {
        CreateEmptyWithName(menuCommand, nameof(Renderer));
    }

    [MenuItem("GameObject/Create GameObject/Trigger", false, 1000)]
    public static void CreateEmptyTrigger(MenuCommand menuCommand)
    {
        CreateEmptyWithName(menuCommand, "Trigger");
    }

    [MenuItem("GameObject/Create GameObject/Collider", false, 1000)]
    public static void CreateEmptyCollider(MenuCommand menuCommand)
    {
        CreateEmptyWithName(menuCommand, nameof(Collider));
    }

    [MenuItem("GameObject/Create GameObject/Item", false, 2000)]
    public static void CreateEmptyItem(MenuCommand menuCommand)
    {
        var itemGo = CreateEmptyWithName(menuCommand, "Item");
        var triggerGo = CreateEmptyWithName(itemGo, "Trigger");
        var collider = triggerGo.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0f, 0.5f, 0f);
        triggerGo.AddComponent<AutoGizmos>();

        itemGo.SetLayerRecursion(LayerMask.NameToLayer("Item"));
        Selection.activeGameObject = itemGo;
    }


    internal static GameObject CreateEmptyWithName(MenuCommand menuCommand, string name)
    {
        return CreateEmptyWithName(menuCommand.context as GameObject, name);
    }

    internal static GameObject CreateEmptyWithName(GameObject selection, string name)
    {
        var go = new GameObject(name);
        GameObjectUtility.SetParentAndAlign(go, selection);
        go.transform.ResetLocal();
        Selection.activeGameObject = go;
        return go;
    }
}
#endif