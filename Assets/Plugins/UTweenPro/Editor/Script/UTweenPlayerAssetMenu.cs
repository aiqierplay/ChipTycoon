#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace Aya.TweenPro
{
    public partial class UTweenPlayer
    {
        #region Context Menu

        [ContextMenu("Export Preset...", false, 10000)]
        public void ExportPreset()
        {
            var path = EditorUtility.SaveFilePanel("Export param to UTween Animation Preset", Application.dataPath, "UTween", "asset");
            path = path.Remove(0, path.IndexOf("Assets", StringComparison.Ordinal));
            var asset = ScriptableObject.CreateInstance<UTweenAnimationPreset>();
            asset.Animation = Animation;
            var saveAsset = Instantiate(asset);
            DestroyImmediate(asset);
            AssetDatabase.CreateAsset(saveAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [ContextMenu("Import Preset...", false, 10001)]
        public void ImportPreset()
        {
            var path = EditorUtility.OpenFilePanel("Select a UTween Animation Preset", Application.dataPath, "asset");
            path = path.Remove(0, path.IndexOf("Assets", StringComparison.Ordinal));
            var asset = (UTweenAnimationPreset)AssetDatabase.LoadAssetAtPath(path, typeof(UTweenAnimationPreset));
            if (asset == null) return;

            UTweenAnimationPreset.ApplyPreset(gameObject, asset);
            Undo.RegisterCompleteObjectUndo(this, "Import UTween Animation Preset");
            EditorUtility.SetDirty(gameObject);
            RefreshEditorAction();
        }

        #endregion
    }
}
#endif