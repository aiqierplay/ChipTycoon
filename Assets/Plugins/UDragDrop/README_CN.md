<div align="left">    
<img src="images/UDragDrop_Logo.png" width = "196" height = "196"/>
</div>

# UDragDrop
**UDragDrop** 是一个适用于 Unity 的快速实现拖放功能的插件，同时支持 UI 和 场景内游戏对象。

![license](https://img.shields.io/github/license/ls9512/UDragDrop)
[![openupm](https://img.shields.io/npm/v/com.ls9512.UDragDrop?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.ls9512.UDragDrop/)
![Unity: 2019.4.3f1](https://img.shields.io/badge/Unity-2019.4.3f1-blue) 
![.NET 4.x](https://img.shields.io/badge/.NET-4.x-blue) 
![topLanguage](https://img.shields.io/github/languages/top/ls9512/UDragDrop)
![size](https://img.shields.io/github/languages/code-size/ls9512/UDragDrop)
![last](https://img.shields.io/github/last-commit/ls9512/UDragDrop)
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)

[![issue](https://img.shields.io/github/issues/ls9512/UDragDrop)](https://github.com/ls9512/UDragDrop/issues)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/ls9512/UDragDrop/pulls)
[![Updates](https://img.shields.io/badge/Platform-%20iOS%20%7C%20OS%20X%20%7C%20Android%20%7C%20Windows%20%7C%20Linux%20-brightgreen.svg)](https://github.com/ls9512/UDragDrop)

[[English Documents Available]](README.md)

> 官方交流QQ群：[1070645638](https://jq.qq.com/?_wv=1027&k=ezkLnUln)

<!-- vscode-markdown-toc -->


<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

***

* 零代码实现通用拖放功能
* 支持任意位置，限制区域，限制摆放槽位 多种拖放模式
* UGUI 下自动适配 Canvas 的多种渲染模式
* 同时支持 UGUI / 2D / 3D 对象拖放
* 支持组合不规则限制区域
* 支持拖拽对象和拖放区域分组
* 完整的回调监听

***

## 快速开始
### DragItem
### DragArea
#### 单个拖拽区域
#### 组合拖拽区域

### DropSlot
Pickup
DragBegin
Drag
Drop
DragEnd

## UGUI
### 必要条件
* 场景中包含一个 CullingMask 包含了 "UI" 的相机，如相机设置不同于默认，则需要通过代码提前指定 UI 渲染相机.
* UIDragItem 必须包含 MaskableGraphic 组件, 比如 Image, 用于确定识别拖动的有效区域以及接收 UI 事件.
* 场景中需存在 EventSystem 用于接收处理 UI 事件.

## 3D
### 必要条件
* 场景中包含一个 Tag 为 "MainCamera" 的主相机，并且拖拽相关组件都有此相机渲染，如果相机设置不同于默认，则需要通过代码提前指定相应的渲染相机.
* DragItem 和 DropSlot 都必须包含 Collider 组件以用于射线检测.
* 建议将 DragItem 和 DropSlot 设置不同的 Layer 以便于拖拽检测不被其他物体遮挡, 同时有助于提高物理检测性能.

## 2D
### 必要条件
* Main Camera
* DragItem 和 DropSlot 都必须包含 Collider2D 组件以用于射线检测。
* 建议将 DragItem 和 DropSlot 设置不同的 Layer 以便于拖拽检测不被其他物体遮挡, 同时有助于提高物理检测性能.

## 可选功能
### 拖放模式
### 拖放区域分组
### 回调监听
#### DragItemCallback
#### DropSlotCallback