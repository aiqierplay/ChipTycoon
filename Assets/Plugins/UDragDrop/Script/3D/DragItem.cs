using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drag Item")]
    [DisallowMultipleComponent]
    public class DragItem : DragItemBase
    {
        [Header("3D")] public DragPlaceType DragPlane = DragPlaceType.XZ;
        public DropRayType DropRayType = DropRayType.PlaneNormal;

        [NonSerialized] public DragEventHandler EventHandler;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            EventHandler = DragEventHandler.Ins;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion

        #region Register / DeRegister

        public override void Register()
        {
            base.Register();
            // var dragItemList = UDragDropManager.Ins.GetDragItem3DList(GroupKey);
            // dragItemList.Add(this);
        }

        public override void DeRegister()
        {
            base.DeRegister();
            // if (UDragDropManager.Ins == null) return;
            // var dragItemList = UDragDropManager.Ins.GetDragItem3DList(GroupKey);
            // dragItemList.Remove(this);
        }

        #endregion

        #region Event Handler

        public void OnDown()
        {
            DragBeginPosition = GetCurrentPosition();
            Pickup();
        }

        public void OnUp()
        {
            DragEndPosition = GetCurrentPosition();
            Drop();
        }

        public void OnBeginDrag()
        {
            DragBegin();
        }

        public void OnDrag()
        {
            DragMovePosition = GetCurrentPosition() - DragBeginPosition;
            Drag();
        }

        public void OnEndDrag()
        {
            DragEndPosition = GetCurrentPosition();
            DragEnd();
        }

        public virtual Vector3 GetCurrentPosition()
        {
            var mainCamera = UDragDrop.GetMainCamera();
            var startPosition = BeginTransData.Position;
            switch (DragPlane)
            {
                case DragPlaceType.XZ:
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (DragDropUtil.LinePlaneIntersection(ray.origin, ray.direction, startPosition, Vector3.up, out var point))
                    {
                        return point;
                    }
                }
                    break;
                case DragPlaceType.XY:
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (DragDropUtil.LinePlaneIntersection(ray.origin, ray.direction, startPosition, Vector3.forward, out var point))
                    {
                        return point;
                    }
                }
                    break;
                case DragPlaceType.YZ:
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (DragDropUtil.LinePlaneIntersection(ray.origin, ray.direction, startPosition, Vector3.right, out var point))
                    {
                        return point;
                    }
                }
                    break;
                case DragPlaceType.AreaPlane:
                    break;
                case DragPlaceType.ScreenPlane:
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (DragDropUtil.LinePlaneIntersection(ray.origin, ray.direction, startPosition, mainCamera.transform.forward, out var point))
                    {
                        return point;
                    }
                }
                    break;
            }

            return startPosition;
        }

        #endregion

        #region Check
        
        public override DropSlotBase FindDropSlot()
        {
            switch (DropRayType)
            {
                case DropRayType.CameraRay:
                    {
                        var mainCamera = UDragDrop.GetMainCamera();
                        var origin = Transform.position;
                        var direction = mainCamera.transform.forward;
                        origin -= direction * UDragDrop.RayCheckDistance;
                        var ray = new Ray(origin, direction);
                        LayerMask layer = 1 << gameObject.layer;
                        var slot = DragDropUtil.Raycast<DropSlot>(ray.origin, ray.direction, UDragDrop.RayCheckDistance * 2f, layer).Item1;
                        return slot;
                    }
                case DropRayType.PlaneNormal:
                    {
                        var origin = Transform.position;
                        var direction = Vector3.down;
                        origin -= direction * UDragDrop.RayCheckDistance;
                        var ray = new Ray(origin, direction);
                        LayerMask layer = 1 << gameObject.layer;
                        var slot = DragDropUtil.Raycast<DropSlot>(ray.origin, ray.direction, UDragDrop.RayCheckDistance * 2f, layer).Item1;
                        return slot;
                    }
            }

            return default;
        } 

        #endregion
    }
}