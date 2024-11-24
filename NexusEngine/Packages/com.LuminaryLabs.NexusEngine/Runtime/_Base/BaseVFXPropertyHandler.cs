using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Handler for applying a list of property updates to a material.
/// </summary>
public class BaseVFXPropertyHandler : BaseSequence<BaseVFXPropertyHandlerData>
{

    private VisualEffect _visualEffect;
    private List<PropertyUpdateDefinition> _propertyUpdateDefinitions;

    protected override UniTask Initialize(BaseVFXPropertyHandlerData currentData)
    {
        _visualEffect = currentData.visualEffect;
        _propertyUpdateDefinitions = new List<PropertyUpdateDefinition>(currentData.PropertyUpdateDefinitions);
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        foreach (var propertyUpdateDefinition in _propertyUpdateDefinitions)
        {
            propertyUpdateDefinition.Apply(_visualEffect);
        }

        this.Complete();
    }
}


[System.Serializable]
public class BaseVFXPropertyHandlerData : BaseSequenceData
{
    public VisualEffect visualEffect;

    [SerializeReference]
    public List<PropertyUpdateDefinition> PropertyUpdateDefinitions;
}

[System.Serializable]
public class PropertyUpdateDefinition
{
    public string PropertyName;
    public virtual void Apply(VisualEffect visualEffect){}
}


[System.Serializable]
public abstract class PropertyUpdateDefinition<T> : PropertyUpdateDefinition
{    
    [SerializeField]
    public T Value;
}

/// <summary>
/// Concrete class for updating a float property.
/// </summary>
[System.Serializable]
public class FloatPropertyUpdateDefinition : PropertyUpdateDefinition<float>
{

    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Vector2 property.
/// </summary>
[System.Serializable]
public class Vector2PropertyUpdateDefinition : PropertyUpdateDefinition<Vector2>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector2(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Vector3 property.
/// </summary>
[System.Serializable]
public class Vector3PropertyUpdateDefinition : PropertyUpdateDefinition<Vector3>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector3(PropertyName, Value);
    }
}
/// <summary>
/// Concrete class for updating an AnimationCurve property.
/// </summary>
[System.Serializable]
public class AnimationCurvePropertyUpdateDefinition : PropertyUpdateDefinition<AnimationCurve>
{
    public override void Apply(VisualEffect visualEffect)
    {
        // AnimationCurve cannot be directly set; use alternative logic if needed.
        Debug.LogWarning("AnimationCurve is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating an ArcCircle property.
/// </summary>
[System.Serializable]
public class ArcCirclePropertyUpdateDefinition : PropertyUpdateDefinition<float>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating an ArcCone property.
/// </summary>
[System.Serializable]
public class ArcConePropertyUpdateDefinition : PropertyUpdateDefinition<float>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating an ArcSphere property.
/// </summary>
[System.Serializable]
public class ArcSpherePropertyUpdateDefinition : PropertyUpdateDefinition<float>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating an ArcTorus property.
/// </summary>
[System.Serializable]
public class ArcTorusPropertyUpdateDefinition : PropertyUpdateDefinition<float>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating an AxisAlignedBox property.
/// </summary>
[System.Serializable]
public class AxisAlignedBoxPropertyUpdateDefinition : PropertyUpdateDefinition<Vector3>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector3(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Bool property.
/// </summary>
[System.Serializable]
public class BoolPropertyUpdateDefinition : PropertyUpdateDefinition<bool>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetBool(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Camera property.
/// </summary>
[System.Serializable]
public class CameraPropertyUpdateDefinition : PropertyUpdateDefinition<Camera>
{
    public override void Apply(VisualEffect visualEffect)
    {
        // Custom logic to handle Camera type
        Debug.LogWarning("Camera is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating a Circle property.
/// </summary>
[System.Serializable]
public class CirclePropertyUpdateDefinition : PropertyUpdateDefinition<float>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetFloat(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Color property.
/// </summary>
[System.Serializable]
public class ColorPropertyUpdateDefinition : PropertyUpdateDefinition<Color>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector4(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Cubemap property.
/// </summary>
[System.Serializable]
public class CubemapPropertyUpdateDefinition : PropertyUpdateDefinition<Cubemap>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetTexture(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Direction property.
/// </summary>
[System.Serializable]
public class DirectionPropertyUpdateDefinition : PropertyUpdateDefinition<Vector3>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector3(PropertyName, Value);
    }
}



/// <summary>
/// Concrete class for updating a Gradient property.
/// </summary>
[System.Serializable]
public class GradientPropertyUpdateDefinition : PropertyUpdateDefinition<Gradient>
{
    public override void Apply(VisualEffect visualEffect)
    {
        // Gradients are not directly supported; custom handling is required.
        Debug.LogWarning("Gradient is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating a GraphicsBuffer property.
/// </summary>
[System.Serializable]
public class GraphicsBufferPropertyUpdateDefinition : PropertyUpdateDefinition<GraphicsBuffer>
{
    public override void Apply(VisualEffect visualEffect)
    {
        // GraphicsBuffer requires custom logic.
        Debug.LogWarning("GraphicsBuffer is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating an Int property.
/// </summary>
[System.Serializable]
public class IntPropertyUpdateDefinition : PropertyUpdateDefinition<int>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetInt(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Matrix4x4 property.
/// </summary>
[System.Serializable]
public class Matrix4x4PropertyUpdateDefinition : PropertyUpdateDefinition<Matrix4x4>
{
    public override void Apply(VisualEffect visualEffect)
    {
        Debug.LogWarning("Matrix4x4 is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating a Mesh property.
/// </summary>
[System.Serializable]
public class MeshPropertyUpdateDefinition : PropertyUpdateDefinition<Mesh>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetMesh(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a SkinnedMeshRenderer property.
/// </summary>
[System.Serializable]
public class SkinnedMeshRendererPropertyUpdateDefinition : PropertyUpdateDefinition<SkinnedMeshRenderer>
{
    public override void Apply(VisualEffect visualEffect)
    {
        Debug.LogWarning("SkinnedMeshRenderer is not directly supported by VisualEffect.");
    }
}

/// <summary>
/// Concrete class for updating a Texture2D property.
/// </summary>
[System.Serializable]
public class Texture2DPropertyUpdateDefinition : PropertyUpdateDefinition<Texture2D>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetTexture(PropertyName, Value);
    }
}

/// <summary>
/// Concrete class for updating a Texture3D property.
/// </summary>
[System.Serializable]
public class Texture3DPropertyUpdateDefinition : PropertyUpdateDefinition<Texture3D>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetTexture(PropertyName, Value);
    }
}


/// <summary>
/// Concrete class for updating a Vector4 property.
/// </summary>
[System.Serializable]
public class Vector4PropertyUpdateDefinition : PropertyUpdateDefinition<Vector4>
{
    public override void Apply(VisualEffect visualEffect)
    {
        visualEffect.SetVector4(PropertyName, Value);
    }
}


