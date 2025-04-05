Shader "Custom/FurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FurTex ("Fur Pattern", 2D) = "white" {}
        _FurLength ("Fur Length", Range(0, 0.2)) = 0.05
        _FurDensity ("Fur Density", Range(0, 2)) = 1.0
        _FurThinness ("Fur Thinness", Range(0, 10)) = 1.0
        _FurShading ("Fur Shading", Range(0, 1)) = 0.25
        _FurColor ("Fur Color", Color) = (1,1,1,1)
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0, 0)
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.5
        _FurGravityStrength ("Fur Gravity", Range(0, 1)) = 0.25
        _Layer ("Layer", Range(0, 1)) = 0.0 // Will be set by the renderer for each shell
        
        // Brush properties (controlled via script)
        // _BrushPos ("Brush Position", Vector) = (0, 0, 0, 0)
        // _BrushRadius ("Brush Radius", Float) = 0.5
        // _BrushStrength ("Brush Strength", Float) = 0.1
        // _BrushFalloff ("Brush Falloff", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

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
                float3 normal : NORMAL;
                float layer : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            sampler2D _FurTex;
            float _FurLength;
            float _FurDensity;
            float _FurThinness;
            float _FurShading;
            float4 _FurColor;
            float4 _WindDirection;
            float _WindStrength;
            float _FurGravityStrength;
            float _Layer;
            
            // Brush properties (set globally from script)
            float3 _BrushPos;
            float _BrushRadius;
            float _BrushStrength;
            float _BrushFalloff;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                // Store the shell layer value (0 = base, 1 = tip)
                o.layer = _Layer;
                
                // Get the original vertex position
                float4 vertexPos = v.vertex;
                
                // Calculate the distance from the vertex to the brush position
                float distToBrush = distance(vertexPos.xyz, _BrushPos);
                
                // Compute wind effect using sine function based on time and vertex normal
                float wind = sin(_Time.y + dot(vertexPos.xyz, _WindDirection.xyz)) * _WindStrength;
                
                // Compute gravity effect
                float3 gravity = float3(0, -1, 0) * _FurGravityStrength;
                
                // Displace vertex along its normal, scaled by fur length, layer position, and influenced by wind and gravity
                float3 offset = v.normal * _FurLength * _Layer;
                offset += wind * _Layer * _WindDirection.xyz * _FurLength;
                offset += gravity * _Layer * _Layer * _FurLength;
                
                // Apply brush effect if the vertex is within the brush radius
                if (distToBrush < _BrushRadius && _BrushStrength > 0)
                {
                    // Calculate brush influence based on distance (using falloff)
                    float brushInfluence = 1.0 - pow(distToBrush / _BrushRadius, _BrushFalloff);
                    
                    // Create a brush direction vector away from the brush center
                    // The effect is stronger on outer shell layers
                    float3 brushDir = normalize(vertexPos.xyz - _BrushPos);
                    
                    // Apply stronger brush effect to the tips of the fur
                    float layerInfluence = _Layer * _Layer; // Quadratic to make tips more affected
                    
                    // Add the brush displacement to the offset
                    offset += brushDir * _BrushStrength * brushInfluence * layerInfluence;
                }
                
                // Apply the final displacement
                float4 displacedVertex = vertexPos + float4(offset, 0);
                o.vertex = UnityObjectToClipPos(displacedVertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the fur pattern
                fixed4 furPattern = tex2D(_FurTex, i.uv * _FurThinness);
                
                // Create a density falloff - thicker at the base, thinner at the tips
                float density = furPattern.r * _FurDensity;
                
                // If the fur is too thin at this point, discard the fragment
                if (density < i.layer)
                    discard;
                
                // Base color from the main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Apply fur color
                col *= _FurColor;
                
                // Apply simple shading based on layer (darker toward the base)
                col = lerp(col, col * _FurShading, i.layer);
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
} 