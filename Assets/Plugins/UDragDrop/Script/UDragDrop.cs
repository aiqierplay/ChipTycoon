using System.Collections.Generic;
using UnityEngine;

namespace Aya.DragDrop
{
    public static class UDragDrop
    {
        public const string AddComponentMenuPath = nameof(UDragDrop);
        public const string DefaultGroupKey = "Default";
        public static float RayCheckDistance = 1000f;

        public static Camera UICamera;
        public static Camera MainCamera;

        #region Cache

        public static Dictionary<string, DragGroupData> GroupDataDic = new();

        public static DragGroupData GetGroupData(string groupKey = UDragDrop.DefaultGroupKey)
        {
            if (GroupDataDic.TryGetValue(groupKey, out var groupData))
            {
                return groupData;
            }

            groupData = new DragGroupData(groupKey);
            GroupDataDic.Add(groupKey, groupData);
            return groupData;
        }

        public static Camera GetMainCamera()
        {
            if (MainCamera != null) return MainCamera;
            MainCamera = Camera.main;
            return MainCamera;
        }

        public static Camera GetUICamera()
        {
            if (UICamera != null) return UICamera;
            var cameras = Camera.allCameras;
            var uiLayer = LayerMask.NameToLayer("UI");
            foreach (var camera in cameras)
            {
                if ((camera.cullingMask & (1 << uiLayer)) > 0)
                {
                    UICamera = camera;
                    break;
                }
            }

            return UICamera;
        }

        #endregion
    }
}