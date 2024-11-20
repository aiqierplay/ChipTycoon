Shader "Aya/GrassShader" 
{
    Properties {
        [Header(Base)]
        _BaseColor("Color", Color) = (0, 0, 0, 1)
        _TopColor("Top Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}
        _AreaCenterOffset("Grass Area Center Offset", Vector) =  (0, 0, 0, 0)

        [Header(Wind)]
        _WindDirection("Wind Direction", Vector) = (0, 0, 0, 0)
        _WindStrength("Wind Strength", Float) = 1.0
        _WindFrequency("Wind Frequency", Float) = 1.0

        [Header(Cut)]
        _CutDirection("Cut Direction", Vector) = (0, 0, 0, 0)

        [Header(Extrusion)]
        _Extrusion ("Extrusion", Float) = 0
        _ExtrusionRadius ("Extrusion Radius", Float) = 0
        _ExtrusionPower ("Extrusion Power", Float) = 0

        [Header(Wave)]
        [NoScaleOffset]
        _WaveTex("Wave Texture", 2D) = "white" {}
        _WaveScale("Wave Scale", Float) = 1
        _WaveDirection("Wave Direction", Vector) = (0, 0, 0, 0)
        _WaveSpeed("Wave Speed", Vector) =  (0, 0, 0, 0)

        [Header(Culling)]
        [NoScaleOffset]
        _CullingTex("Culling Texture", 2D) = "white" {}
        _CullingScale("Culling Scale", Float) = 1
        _CullingOffset("Culling Offset", Vector) =  (0.5, 0.5, 0, 0)

        [Header(Height)]
        [NoScaleOffset]
        _NoiseHeightTex("Noise Height Texture", 2D) = "white" {}
        _NoiseScale("Noise Scale", Float) = 1
        _NosieHeight("Noise Height", Float) = 0.1
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent" 
        }

        LOD 100

        Pass {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_fog
                // #pragma instancing_options force_array
                #pragma multi_compile_instancing

                #include "UnityCG.cginc"

                struct appdata {
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 localVertex : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                    UNITY_FOG_COORDS(0)
                    UNITY_VERTEX_OUTPUT_STEREO
                    float3 worldNormal : TEXCOORD3;
                };

                fixed4 _BaseColor;
                float4 _TopColor;
                sampler2D _MainTex;
                float4 _AreaCenterOffset;

                float4 _WindDirection;
                float _WindStrength;
                float _WindFrequency;

                float4 _CutDirection;
                float _ExtrusionRadius;
                float _ExtrusionPower;

                sampler2D _WaveTex;
                float _WaveScale;
                float4 _WaveDirection;
                float4 _WaveSpeed;

                sampler2D _CullingTex;
                float _CullingScale;
                float4 _CullingOffset;

                sampler2D _NoiseHeightTex;
                float _NoiseScale;
                float _NosieHeight;

                uniform float4 _PlaceholderPositions[10];
                uniform float _PlaceholderCount; 

                v2f vert (appdata v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.uv = v.uv;
                    o.localVertex = v.vertex;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                    // Extrusion
                    float dis = 99999;
                    float4 nearestPlaceholderPos = float4(0, 0, 0, 0);
                    float4 extrusion = float4(0, 0, 0, 0);

                    for (int i = 0; i < _PlaceholderCount; i++){
                        float4 placeholderPos = _PlaceholderPositions[i];
                        float disTemp = length(placeholderPos - o.worldPos);
                        if (disTemp < dis){
                            dis = disTemp;
                            nearestPlaceholderPos = placeholderPos;
                        }
                    }

                    if (dis < _ExtrusionRadius){
                        float factor = 1 - dis / _ExtrusionRadius;
                        extrusion.xz = normalize(o.worldPos - nearestPlaceholderPos).xz * (factor) * _ExtrusionPower * v.uv.y / 2;
                        extrusion.y = -factor * _ExtrusionPower * v.uv.y;
                        v.vertex += extrusion;
                    }

                    o.vertex = UnityObjectToClipPos(v.vertex);

                    // Old
                    // float4 extrusionBuffer = UNITY_ACCESS_INSTANCED_PROP(InstanceBuffer, _Extrusion);
                    // float extrusionPower = extrusionBuffer * (v.uv.y * v.uv.y);
                    // float4 extrusion = normalize(v.vertex) * extrusionPower;
                    // extrusion.y = -extrusionPower / 2;
                    // v.vertex += extrusion;
                    // o.vertex = UnityObjectToClipPos(v.vertex);

                    // Nosie Height
                    float2 noiseHeightUv = o.worldPos.xz * _NoiseScale;
                    float4 noiseHeight = tex2Dlod(_NoiseHeightTex, float4(noiseHeightUv, 0, 0)).r * _NosieHeight * v.uv.y;
                    o.vertex.y -= noiseHeight;

                    // Wind
                    float windStrengthY = v.uv.y * _WindStrength * sin(_Time.y * _WindFrequency);
                    float windStrengthXZ = v.uv.y * _WindStrength;
                    float windX = windStrengthXZ * _WindDirection.x * sin(_Time.x * _WindFrequency);
                    float windZ = windStrengthXZ * _WindDirection.z * sin(_Time.y * _WindFrequency);
                    o.vertex += float4(windX, windStrengthY * _WindDirection.y , windZ, 0);

                    // Cut
                    float cutStrengthY = v.uv.y;
                    float cutStrengthXZ = v.uv.y;
                    o.vertex += float4(_CutDirection.x * cutStrengthXZ, _CutDirection.y * cutStrengthY, _CutDirection.z * cutStrengthXZ, 0);

                    // Wave
                    float2 waveUv = o.worldPos.xz * _WaveScale + _WaveSpeed * _Time.y;
                    float4 waveColor = tex2Dlod(_WaveTex, float4(waveUv, 0, 0));
                    float wave = waveColor.r;
                    float waveX = wave * _WaveDirection.x * v.uv.y;
                    float waveY = wave * _WaveDirection.y * v.uv.y;
                    float waveZ = wave * _WaveDirection.z * v.uv.y;
                    o.vertex += float4(waveX, waveY, waveZ, 0);

                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }


                fixed4 frag (v2f i) : SV_Target
                {
                    // Base Color
                    float4 color = tex2D(_MainTex, i.uv) * _BaseColor;
                    UNITY_APPLY_FOG(i.fogCoord, color);
                    UNITY_OPAQUE_ALPHA(color.a);
                    color = lerp(color, _TopColor, i.uv.y);

                    // Diffuse Light
                    // float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                    // float diffuse = max(dot(i.worldNormal, lightDir), 0.0);
                    // color.rgb *= (1 - diffuse);

                    // Culling
                    float2 cullingUv =  ((i.worldPos.xz - _AreaCenterOffset.xz) * _CullingScale + _CullingOffset.xy) ;
                    float4 cullingColor = tex2D(_CullingTex, cullingUv.xy);
                    color.a = cullingColor.a;

                    return color;
                }

            ENDCG
        }
    }

    FallBack "Diffuse"
}