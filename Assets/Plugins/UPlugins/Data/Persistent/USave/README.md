# USave

## 特性
* 支持简易单存档模式和适配中大型游戏的多存档模式。
* 支持多种模式的自动保存。
* 支持同步/异步存取。
* 可替换加密、Json序列化和数据格式转换实现方式。
* 自动合并帧内的多次IO操作。
* 基于 Odin 的存档数据可视化编辑器。

## 文件结构
- Save_Main
- Save_Default
- Save_01
- Save_02
- ...

## 文件说明
* **Save_Main** 中保存了存档列表的基础信息，无论是否有存档，这个文件都会被存取，
* 多存档模式下，多个存档以 **Save_SlotKey** 的文件名格式存在，**SlotKey** 由用户自定义。
* **Save_Default** 是单存档模式下的存档文件，自动维护。

## 数据格式
| 功能 | Json | PlayeerPrefs |
|:---|:---|:---|
| 存档文件 | <input type="checkbox" checked> | <input type="checkbox"> |
| 多存档数据拆分 | <input type="checkbox" checked> | <input type="checkbox"> |
| 异步读写 | <input type="checkbox" checked> | <input type="checkbox"> |
| 数据加密 | <input type="checkbox" checked> | <input type="checkbox" checked> |

## 单存档流程
单存档模式的存档位维护为全自动，无需手动调用存档栏位相关接口，适用于大部分小游戏开发。

### 程序启动
* USave.Load()

```
启动时自动调用一次，无需手动执行。
加载存档，默认 Save_Default 存档位，单存档模式下会自动创建。
```

* 读写存档数据
* USave.Save()
```
保存存档，单存档模式下，程序暂停，退出程序时会自动调用。
程序运行过程中，如果数据发生修改且需要确保数据保存，需手动调用。
默认 Save_Default 存档位
```

## 多存档流程
多存档模式下主数据存档自动维护，具体游戏数据的存档栏位需要手动调用存档栏位相关接口自行维护。

* 程序启动
* 从 USave.SlotList 获取存档列表信息
* USave.ExistSlot(slotKey); 检查存档是否存在
* USave.CreateSlot(slotKey); 创建存档
* USave.SelectSlot(slotKey); 选择存档
* USave.Load(); 加载选择的存档
* 读写存档数据
* USave.Save(); 保存选择的存档 

## USave API
### 变量
| 参数 | 类型 | 说明 |
|:---|:---|:---|
| `FilePath` | `string` | 持久化存档文件的保存路径 |
| `SelectSlotKey` | `string` | 当前选择的存档 Key |
| `IsAsyncWorking` | `bool` | 是否正在进行异步存取 |
| `RequireSave` | `bool` | 是否已请求保存，一帧内的多处数据改动和多次保存请求，会合并在帧结束时统一进行一次保存操作以节省IO性能。 |
| `DataChanged` | `bool` | 自动保存间隔时间，单位s |
| `SlotList` | `List<GameSaveSlotData>` | 存档列表数据，用于按需显示存档列表。 |

### 方法
#### Key List
```cs
USave.GetAllKeys(slotKey);
```

#### Load
```cs
USave.Load();
USave.LoadAsync(()=>{
    // Load Complete
});
```

#### Save
```cs
USave.Save();
USave.SaveAysnc(()=>{
    // Save Complete
});
USave.SaveImmediately();
```

#### Slot List
```cs
USave.CreateSlot(slotKey);
USave.ExistSlot(slotKey);
USave.SelectSlot(slotKey);
USave.DeleteSlot(slotKey);
```

#### Get / Set Value
```cs
USave.GetValue(valueKey);
USave.SetValue(valueKey, value);
```

#### Get / Set Object
```cs
USave.GetObject(valueKey);
USave.SetObject(valueKey, value);
```

#### Exist Key
```cs
USave.ExistKey(valueKey);
```

### Delete
```cs
USave.DeleteKey(valueKey);
USave.DeleteAll();
```

## USaveSetting API
## 配置文件 USaveSetting
| 参数 | 类型 | 说明 |
|:---|:---|:---|
| `Mode` | `USaveMode` | 存档工作模式，可选单存档 / 多存档 |
| `Format` | `USaveFormat` | 存储数据的格式 |
| `AutoMode` | `USaveAutoMode` | 自动保存的工作模式 |
| `AutoSaveInterval` | `float` | 自动保存间隔时间，单位s |
| `AutoSaveAsync` | `bool` | 自动保存是否使用异步操作，当存档体积较大时建议开启，但需要注意间隔时间过短，上一次保存未完成时，无法触发下一次保存操作。 | 
| `AutoLoadMainData` | `bool` | 自动加载主存档文件 |
| `Encrypt` | `bool` | 加密存储 |
| `FileExtension` | `string` | 存档文件的扩展名 |
| `MainDataKey` | `string` | 主存档文件的文件名和存储 Key |
| `DefaultSlotKey` | `string` | 默认存档文件的文件名和存储 Key |

### USaveMode
| 枚举值 | 说明 |
|:---|:---|
| `Single` | 单存档模式 |
| `Multi` | 多存档模式 |

### USaveFormat
| 枚举值 | 说明 |
|:---|:---|
| `Json` | Json 文本格式存储，会在持久化目录中创建存档文件。 |
| `PlayerPrefs` | 使用 Unity 自带存储方式存储，实际存储内容参考 Unity 定义。|

### USaveAutoMode
| 枚举值 | 说明 |
|:---|:---|
| `Manual` | 手动保存，不会在任何时机自动调用实际保存，如果不执行保存就退出程序，则会丢失上一次保存之前所有的存档数据改动。 |
| `PauseQuit` | 在程序暂停，丢失焦点，正常退出时，自动触发保存操作。 |
| `Interval` | 间隔一定时间自动保存数据，同时会自动开启暂停退出触发保存。 |