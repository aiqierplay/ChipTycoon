# UNotify

## 问题
无显示时的层级结构初始化

# UNotify 使用手册
## 特性
* 树形结构管理所有通知节点，每个节点都可以有多个父节点和子节点。
* 子节点状态变化会自动刷新所有父级节点状态，父节点也可以统一设置所有子节点状态。
* 状态处理，层级结构处理与显示分离，数据层运行不依赖显示层。
* 可自定义状态显示和状态处理方法。
* 支持固定层级结构和动态变化层级结构。

## 组件
### UNotify

### NotifyNode

### NotifyChecker

### NotifyView

## x

* Notify View
  * Notify Node
      * Notify Checker List