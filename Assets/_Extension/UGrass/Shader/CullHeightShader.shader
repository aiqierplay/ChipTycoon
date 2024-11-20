Shader "Aya/CullHeightShader" 
{
    Properties {
        [Header(Base)]
        _BaseColor("Color", Color) = (0, 0, 0, 1)
        _MainTex("Main Texture", 2D) = "white" {}
        _AreaCenterOffset("Grass Area Center Offset", Vector) =  (0, 0, 0, 0)

        [Header(Height)]
        [NoScaleOffset]
        _HeightMapTex("Height Map Texture", 2D) = "white" {}
        _HeightMapScale("Height Map Scale", Float) = 1
        _HeightMapOffset("Height Map Offset", Vector) =  (0.5, 0.5, 0, 0)
        _Height("Height", Float) = 0.1
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
                sampler2D _MainTex;
                float4 _AreaCenterOffset;

                sampler2D _HeightMapTex;
                float _HeightMapScale;
                float4 _HeightMapOffset;
                float _Height;

                v2f vert (appdata v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.uv = v.uv;
                    o.localVertex = v.vertex;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                    o.vertex = UnityObjectToClipPos(v.vertex);

                    // Height Map
                    float2 heightUv = (o.worldPos.xz - _AreaCenterOffset.xz) * _HeightMapScale + _HeightMapOffset.xy;
                    float alpha = tex2Dlod(_HeightMapTex, float4(heightUv, 0, 0)).a;
                    float4 height = alpha == 0 ? _Height : 0;
                    o.vertex.y += height;

                    return o;
                }


                fixed4 frag (v2f i) : SV_Target
                {
                    // Base Color
                    float4 color = tex2D(_MainTex, i.uv) * _BaseColor;
                    UNITY_APPLY_FOG(i.fogCoord, color);
                    UNITY_OPAQUE_ALPHA(color.a);
     
                    // Diffuse Light
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                    float diffuse = max(dot(i.worldNormal, lightDir), 0.0);
                    color.rgb *= diffuse;

                    return color;
                }

            ENDCG
        }
    }

    FallBack "Diffuse"
}