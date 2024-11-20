using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MaterialPropertyBlockColor : EntityBase
{
    [ValueDropdown(nameof(GetRendererList))] public Renderer Render;

    public List<Renderer> GetRendererList()
    {
        return transform.GetComponentsInChildren<Renderer>().ToList();
    }

    [ValueDropdown(nameof(GetMaterialList))] public int MaterialIndex;

    public List<ValueDropdownItem<int>> GetMaterialList()
    {
        if (Render == null) return null;
        var list = new List<ValueDropdownItem<int>>();
        for (var i = 0; i < Render.sharedMaterials.Length; i++)
        {
            var item = new ValueDropdownItem<int>(Render.sharedMaterials[i].name, i);
            list.Add(item);
        }

        return list;
    }

    [ValueDropdown(nameof(GetMaterialPropertyList))] public string MaterialProperty;

    public List<ValueDropdownItem<string>> GetMaterialPropertyList()
    {
        if (Render == null) return null;
        var list = new List<ValueDropdownItem<string>>();
#if UNITY_EDITOR
        var material = Render.sharedMaterials[MaterialIndex];

        var shader = material.shader;
        var shaderCount = ShaderUtil.GetPropertyCount(shader);
        for (var i = 0; i < shaderCount; i++)
        {
            var index = i;
            // var hidden = ShaderUtil.IsShaderPropertyHidden(shader, index);
            var shaderPropertyType = ShaderUtil.GetPropertyType(shader, index);

            if (shaderPropertyType == ShaderUtil.ShaderPropertyType.Color)
            {
                var shaderPropertyName = ShaderUtil.GetPropertyName(shader, index);
                var shaderPropertyDescription = ShaderUtil.GetPropertyDescription(shader, index);
                var item = new ValueDropdownItem<string>(shaderPropertyName + " - " + shaderPropertyDescription, shaderPropertyName);
                list.Add(item);
            }
        }
#endif

        return list;
    }

    public Color Value = Color.white;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetValue();
    }

    public void SetValue()
    {
        if (Render == null) return;
        if (Render.sharedMaterials.Length < MaterialIndex) return;
        var materialPropertyBlock = new MaterialPropertyBlock();
        Render.GetPropertyBlock(materialPropertyBlock, MaterialIndex);
        materialPropertyBlock.SetColor(MaterialProperty, Value);
        Render.SetPropertyBlock(materialPropertyBlock, MaterialIndex);
    }
}
