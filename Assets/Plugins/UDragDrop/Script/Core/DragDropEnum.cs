
namespace Aya.DragDrop
{
    // 拖动平面
    // 如果该平面垂直于相机，则可能无法拖动
    public enum DragPlaceType
    {
        XZ = 0,
        XY = 1,
        YZ = 2,
        AreaPlane = 3,
        ScreenPlane = 4,
    }

    // 放置检测射线类型
    public enum DropRayType
    {
        CameraRay = 0,
        PlaneNormal = 1,
    }

    // 拖动状态
    public enum DragItemState
    {
        // 闲置状态，不在槽位内
        Idle = 0,
        // 拾取过程中
        Pickup = 1,
        // 拖动过程中
        Drag = 2,
        // 放置在槽位内
        DropToSlot = 3,
        // 放置在任何地方
        DropAnyWhere = 4,
        // 返回起点
        DropBackToStart = 5,
    }

    // 放置失败处理模式
    public enum DropFailMode
    {
        None = 0,
        Restore = 1,
    }

    // 拖动元素插值状态
    public enum DragItemLerpState
    {
        // 空闲
        Idle = 0,
        // 正在插值过程中
        Lerp = 1,
    }

    // 拖放目标类型
    public enum DragTargetMode
    {
        // 自由拖放到任何地方，但受存在的区域限制
        Free = 0,
        // 仅可以拖放到槽位中，同时受到区域限制
        Slot = 1,
    }

    // 槽位工作模式
    public enum DropMode
    {
        // 仅空槽位可以放置
        DropEmpty = 0,
        // 空槽位可以放置，非空槽位与来源对象的槽位互相交换位置
        DropReplace = 1,
        // 自定义实现，用于扩展
        Custom = 2,
    }

    // 元素放进槽位后的状态处理模式
    public enum DropPlaceMode
    {
        OnlyPos = 0,
        MoveToTransWithPos = 1,
    }
}