using System;
using UnityEngine;

namespace Aya.DragDrop
{
    public class DragEventHandler : MonoBehaviour
    {
        #region Singleton

        internal static bool ApplicationIsQuitting = false;
        internal static DragEventHandler Instance;

        public static DragEventHandler Ins
        {
            get
            {
                if (Application.isPlaying && ApplicationIsQuitting)
                {
                    return null;
                }

                if (Instance != null) return Instance;
                Instance = FindObjectOfType<DragEventHandler>();
                if (Instance != null)
                {
                    DontDestroyOnLoad(Instance);
                    return Instance;
                }

                var insName = nameof(DragEventHandler);
                if (!Application.isPlaying)
                {
                    insName += " (Editor)";
                }

                var obj = new GameObject
                {
                    name = insName,
                    hideFlags = HideFlags.None,
                };

                Instance = obj.AddComponent<DragEventHandler>();
                if (Application.isPlaying) DontDestroyOnLoad(Instance);
                return Instance;
            }
        }

        #endregion

        [NonSerialized] public LayerMask? DragItemLayer;

        #region MonoBehaviour

        public void OnEnable()
        {
            IsMouseButtonDown = false;
        }
        public void Update()
        {
            UpdateImpl();
        }

        public void OnDestroy()
        {
            if (Application.isPlaying)
            {
                ApplicationIsQuitting = true;
            }

            if (Instance != null)
            {
                Destroy(Instance);
                Instance = null;
            }
        }

        #endregion

        [NonSerialized] public bool IsMouseButtonDown;
        [NonSerialized] public DragItem DragItem;
        [NonSerialized] public Vector3 MouseStartPosition;

        public void UpdateImpl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseButtonDown) return;
                IsMouseButtonDown = true;
                var rayItem = RayCheck();
                if (rayItem != null)
                {
                    if (DragItem != null)
                    {
                        DragItem.OnUp();
                        DragItem.OnEndDrag();
                    }

                    DragItem = rayItem;
                    MouseStartPosition = Input.mousePosition;
                    DragItem.OnDown();
                }
                else
                {
                    if (DragItem != null)
                    {
                        DragItem.OnUp();
                        DragItem.OnEndDrag();
                        DragItem = null;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!IsMouseButtonDown) return;
                IsMouseButtonDown = false;
                if (DragItem != null)
                {
                    DragItem.OnUp();
                    DragItem.OnEndDrag();
                    DragItem = null;
                }
            }

            if (IsMouseButtonDown)
            {
                if (DragItem != null)
                {
                    if (MouseStartPosition != Input.mousePosition && DragItem.ItemState != DragItemState.Drag)
                    {
                        DragItem.OnBeginDrag();
                        DragItem.OnDrag();
                        return;
                    }

                    if (DragItem.ItemState == DragItemState.Drag)
                    {
                        DragItem.OnDrag();
                    }
                }
            }
        }

        public LayerMask GetDragItemLayer()
        {
            if (DragItemLayer != null) return DragItemLayer.Value;
            var layer = 0;
            foreach (var kv in UDragDrop.GroupDataDic)
            {
                foreach (var dragItem in kv.Value.ItemList)
                {
                    if (dragItem is not DragDrop.DragItem) continue;
                    var layerValue = 1 << dragItem.gameObject.layer;
                    layer |= layerValue;
                }
            }

            LayerMask result = layer;
            DragItemLayer = result;
            return result;
        }

        public virtual DragItem RayCheck()
        {
            var mainCamera = UDragDrop.GetMainCamera();
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var area = DragDropUtil.Raycast<DragItem>(ray.origin, ray.direction, 1000f, GetDragItemLayer()).Item1;
            return area;
        }
    }
}