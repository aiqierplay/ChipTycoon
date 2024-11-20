using System;

[Serializable]
public abstract class SaveInfoDic<TInfo> : SaveInfoCollection<string, TInfo> where TInfo : SaveInfoDic<TInfo>, new()
{
  
}