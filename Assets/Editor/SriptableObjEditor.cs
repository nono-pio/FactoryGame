using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Item),true)]
[CanEditMultipleObjects]
public class ItemEditor : Editor
{
    private Item item { get { return (target as Item); } }

    public override Texture2D RenderStaticPreview(string assetPath,Object[] subAssets,int width,int height)
    {
        if(item.image != null)
        {
            Type t = GetType("UnityEditor.SpriteUtility");
            if(t!=null)
            {
                MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite),typeof(Color),typeof(int),typeof(int) });
                if(method!=null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { item.image, Color.white, width, height });
                    if(ret is Texture2D)
                        return ret as Texture2D;
                }
            }
        }
        return base.RenderStaticPreview(assetPath,subAssets,width,height);
    }

    private static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);
        if(type!=null)
            return type;

        if(TypeName.Contains("."))
        {
            var assemblyName = TypeName.Substring(0,TypeName.IndexOf('.'));
            var assembly = Assembly.Load(assemblyName);
            if(assembly==null)
                return null;
            type=assembly.GetType(TypeName);
            if(type!=null)
                return type;
        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach(var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if(assembly!=null)
            {
                type=assembly.GetType(TypeName);
                if(type!=null)
                    return type;
            }
        }
        return null;
    }
}

[CustomEditor(typeof(ItemCraft),true)]
[CanEditMultipleObjects]
public class ItemCraftEditor : Editor
{
    private ItemCraft craft { get { return (target as ItemCraft); } }

    public override Texture2D RenderStaticPreview(string assetPath,Object[] subAssets,int width,int height)
    {
        if(craft.item != null && craft.item.image != null)
        {
            Type t = GetType("UnityEditor.SpriteUtility");
            if(t!=null)
            {
                MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite),typeof(Color),typeof(int),typeof(int) });
                if(method!=null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { craft.item.image, Color.white, width, height });
                    if(ret is Texture2D)
                        return ret as Texture2D;
                }
            }
        }
        return base.RenderStaticPreview(assetPath,subAssets,width,height);
    }

    private static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);
        if(type!=null)
            return type;

        if(TypeName.Contains("."))
        {
            var assemblyName = TypeName.Substring(0,TypeName.IndexOf('.'));
            var assembly = Assembly.Load(assemblyName);
            if(assembly==null)
                return null;
            type=assembly.GetType(TypeName);
            if(type!=null)
                return type;
        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach(var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if(assembly!=null)
            {
                type=assembly.GetType(TypeName);
                if(type!=null)
                    return type;
            }
        }
        return null;
    }
}