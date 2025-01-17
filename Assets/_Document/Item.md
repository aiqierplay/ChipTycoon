# Item 通用参数说明

## General 基础参数
### Layer Mask
1. 作用生效对象的层级，一般为 `Player`。
2. 如果玩家由多个部分组成并且道具作用域特殊部位，则指定为部位的层级。
3. 如果道具是 `Item Spawn Prefab` 之类在关卡启动时特殊处理的道具，则一般为 `Nothing` 。

### De Spawn Mode
道具回收/关闭模式

|Type   |Description|
|-|-|
|None   |始终不回收|
|Effect |生效后回收，与 `Effect Mode` 相关|
|Exit   |目标离开道具范围后回收|

### De Active Renderer
道具生效后关闭渲染部分，但道具逻辑依旧运作，一般不需要启用。

### Effect Mode
|Type   |Description|
|-|-|
|Once       |仅生效一次|
|Un Limit   |不限制生效次数，每次进入触发生效|
|Count      |指定生效次数，每次进入触发生效|
|Stay       |玩家在道具作用范围内时，每间隔一定时间触发生效一次|

___

## Condition
道具生效的条件，如果不满足任意一个条件，则不会触发生效和计数。

___

## Renderer
用于指定道具实际渲染的内容，道具的样式可以直接做在预制中，也可以通过以下两个列表配置实现动态内容渲染，两个列表可以同时使用。
### Render Prefabs
道具初始化时，会显示此列表中所有内容。
### Render Random Prefabs
道具初始化时，会随机选取列表中的一个内容显示。
### Item Group Index
道具组索引，需配合 `Item Group Setting` 使用。默认为 `-1` 时则不生效。`大于等于0`时，会从玩家当前选择的道具组，渲染指定索引的内容。

___

## Active
### Active List
道具`首次`生效时，打开列表中的所有物体。例如触发机关开启前方大门或者浮桥。
### De Active List
道具`首次`生效时，关闭列表中的所有物体。例如触发机关关闭前方路障。

___

## Exclude
排除列表，用于实现一组道具的互斥，一般用于位置相近容易同时碰撞触发的道具。一组道具中有 `N` 个道具，则每个道具都需要在列表中添加除自身之外的 `N-1` 个道具。在道具自身被触发后，列表中其他道具都会进入无法触发状态，但显示状态不会发生变化。

___

## Effect & Animation
### Self Fx
当道具生效时，在道具自身所在位置，播放列表中的所有特效。
### Self Random Fx
当道具生效时，在道具自身所在位置，播放列表中的随机特效。
### Target Fx
当道具生效时，在玩家所在位置，播放列表中的所有特效。
### Target Random Fx
当道具生效时，在玩家所在位置，播放列表中的随机特效。

### Tween Animation List
基于 `UTween Animation` 组件的动画列表，在道具每次生效时，会触发播放。
### Animation Data List
基于 `Animator` 组件的动画列表，需要指定状态机和需要播放的动画片段名称，以及道具初始化后默认状态的动画片段名称。

### VibrationType
触发道具效果时的震动模式

___

## Camera & Dock
### Switch Camera
是否开启触发后切换相机
### Enter Camera
进入道具范围后，切换的相机名字
### Exit Camera
离开道具范围后，切换的相机名字

### Enable Dock
是否开启停靠效果
### Dock Trans
停靠的位置和旋转和缩放，从指定的 Transform 获取
### Dock Duration
停靠动画的插值时长