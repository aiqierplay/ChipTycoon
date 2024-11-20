using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using Aya.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityPropertyInitializer : EntityBase
{
    [TypeReference(typeof(EntityBase))] public TypeReference Type;
    [ValueDropdown(nameof(GetItemInitableProperties), DropdownTitle = "Select Property")] public string Property;
    public IEnumerable GetItemInitableProperties => GetFiledAndPropertyDropdownListByType(Type, new[] { typeof(int), typeof(float) });
    [ValueDropdown(nameof(GetItemFilterProperties), DropdownTitle = "Select Property")] public string FilterProperty;
    public IEnumerable GetItemFilterProperties => GetFiledAndPropertyDropdownListByType(Type, new[] { typeof(bool) });

    [TableList(ShowIndexLabels = true), SerializeReference] public List<EntityProperty> PropertyList = new List<EntityProperty>();

    [NonSerialized] public bool Cache;
    [NonSerialized] public List<EntityBase> EntityList;
    [NonSerialized] public FieldInfo FieldInfo;

    [Button("Add Row"), ShowIf(nameof(CheckShowAddRow))]
    public void AddRow()
    {
        var type = PropertyList[0].GetType();
        var newRow = Activator.CreateInstance(type);
        PropertyList.Add(newRow as EntityProperty);
    }

    public bool CheckShowAddRow()
    {
        return PropertyList.Count > 0;
    }

    public void Init()
    {
        // if (!Cache)
        {
            EntityList = Trans.GetComponentsInChildren(Type, true).ToList(component => component as EntityBase);
            // Custom Selector ...
            if (!string.IsNullOrEmpty(FilterProperty))
            {
                EntityList = EntityList.FindAll(e =>
                {
                    var result = (bool)e.GetField(FilterProperty);
                    return result;
                });
            }

            FieldInfo = Type.Type.GetField(Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            // Cache = true;
        }

        if (FieldInfo == null) return;
        for (var i = 0; i < EntityList.Count && i < PropertyList.Count; i++)
        {
            var entity = EntityList[i];
            var property = PropertyList[i];
            var value = property.GetValue();
            if (FieldInfo.FieldType == typeof(int))
            {
                entity.SetField(Property, value.CastType<int>());
            }
            else
            {
                entity.SetField(Property, value);
                // Debug.Log(Property + " " + value);
            }

            // entity.Refresh();
            if (entity is ItemBase item)
            {
                item.ExecuteNextFrame(item.Refresh);
            }
        }
    }
}
