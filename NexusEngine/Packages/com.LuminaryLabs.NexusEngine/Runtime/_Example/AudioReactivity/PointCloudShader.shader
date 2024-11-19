Shader "Custom/URP_PointCloudShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

    // Structured buffer for point data
    StructuredBuffer<float3> pointPositions;
    StructuredBuffer<float3> pointColors;
    StructuredBuffer<float> pointSizes;

    struct Attributes
    {
        uint id : SV_VertexID;
    };

    struct Varyings
    {
        float4 position : SV_POSITION;
        float3 color : COLOR;
    };

    Varyings Vert(Attributes IN)
    {
        Varyings OUT;

        // Retrieve position, color, and size from the buffers using the vertex ID
        float3 pos = pointPositions[IN.id];
        float3 col = pointColors[IN.id];
        float size = pointSizes[IN.id];

        // Convert world space position to clip space for rendering
        float4 worldPosition = float4(pos, 1.0);
        OUT.position = TransformObjectToHClip(worldPosition);

        // Pass the color to the fragment shader
        OUT.color = col;

        return OUT;
    }

    half4 Frag(Varyings IN) : SV_Target
    {
        return half4(IN.color, 1.0);
    }
    ENDHLSL

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        Pass
        {
            Name "PointCloudPass"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
