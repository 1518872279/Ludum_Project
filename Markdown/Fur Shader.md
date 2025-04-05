# Fur Vertex Shader for Dynamic Fur System

This shader demonstrates a fur effect using a shell technique similar to dynamic grass systems. In this approach, multiple layers (or shells) of a mesh are displaced along their normals to simulate fur depth. A wind factor is applied to create dynamic movement, making the fur appear to sway naturally.

## Key Features

- **Vertex Displacement:**  
  Each vertex is displaced along its normal. The displacement is scaled by a specified fur length and modulated by a wind factor, similar to dynamic grass swaying in the wind.

- **Wind Simulation:**  
  A sine wave, combined with the vertex normal and a configurable wind direction, creates a time-based wind effect. The strength of the wind can be adjusted to achieve the desired movement.

- **Shell Technique:**  
  The shader is intended for use with multiple fur shells (layers). Each shell is rendered with slightly different displacement values to build up a volumetric fur effect.

## Shader Code

Below is the complete shader code that can be used in Unity. You can adjust the properties in the Inspector to fine-tune the fur's appearance and behavior.

```c
Shader "Custom/FurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FurLength ("Fur Length", Range(0, 0.2)) = 0.05
        _FurColor ("Fur Color", Color) = (1,1,1,1)
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0, 0)
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float _FurLength;
            float4 _FurColor;
            float4 _WindDirection;
            float _WindStrength;
            
            v2f vert (appdata v)
            {
                v2f o;
                // Compute wind effect using sine function based on time and vertex normal
                float wind = sin(_Time.y + dot(v.normal, _WindDirection.xyz)) * _WindStrength;
                // Displace vertex along its normal, scaled by fur length and wind influence
                float3 offset = v.normal * _FurLength * wind;
                float4 displacedVertex = v.vertex + float4(offset, 0);
                o.vertex = UnityObjectToClipPos(displacedVertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Apply texture and multiply by fur color
                fixed4 col = tex2D(_MainTex, i.uv) * _FurColor;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
