using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public static class TypeCaches
    {
        public static BindingFlags DefaultBindingFlags => BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

#if UNITY_EDITOR

        public static Dictionary<Type, TweenerEditorData> TweenerEditorDataDic
        {
            get
            {
                if (_tweenerEditorDataDic == null)
                {
                    _tweenerEditorDataDic = new Dictionary<Type, TweenerEditorData>();
                    var tweenerTypes = TypeCache.GetTypesWithAttribute<TweenerAttribute>();
                    var tweenerEditorDataList = new List<TweenerEditorData>();

                    for (var i = 0; i < tweenerTypes.Count; i++)
                    {
                        var tweenerType = tweenerTypes[i];
                        var tweenerEditorData = new TweenerEditorData();
                        tweenerEditorData.Cache(tweenerType);
                        tweenerEditorDataList.Add(tweenerEditorData);
                    }

                    tweenerEditorDataList.Sort((i1, i2) =>
                    {
                        var orderCompare = i1.Info.Order - i2.Info.Order;
                        if (orderCompare != 0) return orderCompare;
                        if (i1.TargetType != null && i2.TargetType != null)
                        {
                            var typeCompare = string.Compare(i1.TargetType.Name, i2.TargetType.Name, StringComparison.Ordinal);
                            if (typeCompare != 0) return typeCompare;
                        }
                       
                        var nameCompare = string.Compare(i1.Info.DisplayName, i2.Info.DisplayName, StringComparison.Ordinal);
                        return nameCompare;
                    });

                    for (var i = 0; i < tweenerEditorDataList.Count; i++)
                    {
                        var tweenerEditorData = tweenerEditorDataList[i];
                        _tweenerEditorDataDic.Add(tweenerEditorData.Type, tweenerEditorData);
                    }
                }

                return _tweenerEditorDataDic;
            }
        }

        private static Dictionary<Type, TweenerEditorData> _tweenerEditorDataDic;

#endif
    }
}
