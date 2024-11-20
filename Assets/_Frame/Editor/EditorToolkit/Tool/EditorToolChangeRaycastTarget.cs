#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[EditorTool]
public class EditorToolChangeRaycastTarget : EditorToolBase
{
    public override string GetTitle() => "Scene/Change Raycast";
    public override SdfIconType GetIcon() => SdfIconType.ClipboardCheck;

#pragma warning disable CS0414
    private bool _selectRaycastComplete;
    private bool _cancelRaycastComplete;
#pragma warning restore CS0414

    [PreviewField(50)]
    [OnValueChanged("SelectRaycastListValueChangeCallBack")]
    [HorizontalGroup("Raycast")]
    [PropertySpace(10, 10)]
    [BoxGroup("Raycast/Left", false)]
    public List<GameObject> SelectRaycastList = new List<GameObject>();

    public void SelectRaycastListValueChangeCallBack()
    {
        _selectRaycastComplete = false;
    }


    [PreviewField(50)]
    [OnValueChanged("CancelRaycastListValueChangeCallBack")]
    [PropertySpace(10, 10)]
    [BoxGroup("Raycast/Right", false)]
    public List<GameObject> CancelRaycastList = new List<GameObject>();

    public void CancelRaycastListValueChangeCallBack()
    {
        _cancelRaycastComplete = false;
    }

    [HideIf("@_selectRaycastComplete == true || SelectRaycastList.Count == 0")]
    [BoxGroup("Raycast/Left")]
    [Button(ButtonSizes.Large, ButtonStyle.FoldoutButton)]
    public void SelectRaycast()
    {
        for (var i = 0; i < SelectRaycastList.Count; i++)
        {
            if (SelectRaycastList[i] == null)
            {
                continue;
            }

            var graphicArray = SelectRaycastList[i].GetComponentsInChildren<Graphic>(true);
            for (var k = 0; k < graphicArray.Length; k++)
            {
                graphicArray[k].raycastTarget = true;
                EditorUtility.SetDirty(graphicArray[k]);
            }

            var tempObject = SelectRaycastList[i];
            var isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(tempObject);
            var isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(tempObject);
            if (isPrefabAsset)
            {
                PrefabUtility.SavePrefabAsset(SelectRaycastList[i]);
            }

            if (isPrefabInstance)
            {
                PrefabUtility.ApplyPrefabInstance(SelectRaycastList[i], InteractionMode.UserAction);
            }
        }

        _selectRaycastComplete = true;
        AssetDatabase.Refresh();
    }


    [HideIf("@ _cancelRaycastComplete == true || CancelRaycastList.Count == 0")]
    [BoxGroup("Raycast/Right")]
    [Button(ButtonSizes.Large, ButtonStyle.FoldoutButton)]
    public void CancelRaycast()
    {
        for (var i = 0; i < CancelRaycastList.Count; i++)
        {
            if (CancelRaycastList[i] == null)
            {
                continue;
            }

            var graphicArray = CancelRaycastList[i].GetComponentsInChildren<Graphic>(true);
            for (var k = 0; k < graphicArray.Length; k++)
            {
                graphicArray[k].raycastTarget = false;
                EditorUtility.SetDirty(graphicArray[k]);
            }

            var tempObject = CancelRaycastList[i];
            var isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(tempObject);
            var isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(tempObject);

            if (isPrefabAsset)
            {
                PrefabUtility.SavePrefabAsset(CancelRaycastList[i]);
            }

            if (isPrefabInstance)
            {
                PrefabUtility.ApplyPrefabInstance(CancelRaycastList[i], InteractionMode.UserAction);
            }
        }

        _cancelRaycastComplete = true;
        AssetDatabase.Refresh();
    }
}
#endif