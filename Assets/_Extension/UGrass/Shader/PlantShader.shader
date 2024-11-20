Shader "Aya/PlantShader" {
Properties {
    _BaseColor("Color", Color) = (0, 0, 0, 1)
    _TopColor("Top Color", Color) = (1, 1, 1, 1)
    _MainTex("Main Texture", 2D) = "white" {}

    _WindDirection("Wind Direction", Vector) = (0, 0, 0, 0)
    _WindStrength("Wind Strength", Float) = 1.0
    _WindFrequency("Wind Frequency", Float) = 1.0

    _CutDirection("Cut Direction", Vector) = (0, 0, 0, 0)
    _Extrusion("Extrusion", Float) = 0
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass {
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
            };

            struct v2f {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(0)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _BaseColor;
			float4 _TopColor;
			sampler2D _MainTex;

			float4 _WindDirection;
			float _WindStrength;
			float _WindFrequency;

			float4 _CutDirection;
            // float _Extrusion;

            UNITY_INSTANCING_BUFFER_START(InstanceBuffer)
            UNITY_DEFINE_INSTANCED_PROP(float, _Extrusion)
            UNITY_INSTANCING_BUFFER_END(InstanceBuffer)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.uv = v.uv;

                // 挤压
                float4 extrusionBuffer = UNITY_ACCESS_INSTANCED_PROP(InstanceBuffer, _Extrusion);
                float extrusionPower = extrusionBuffer * (v.uv.y * v.uv.y);
                float4 extrusion = normalize(v.vertex) * extrusionPower;
                extrusion.y = -extrusionPower / 2;
                v.vertex += extrusion;
                o.vertex = UnityObjectToClipPos(v.vertex);

				// 风力
				float windStrengthY = v.uv.y * _WindStrength * sin(_Time.y * _WindFrequency);
				float windStrengthXZ = v.uv.y * _WindStrength;
				// strength *= sin(_Time.z * (o.positionWS.x + o.positionWS.z) * _WindFrequency);
				float windX = windStrengthXZ * _WindDirection.x * sin(_Time.z * _WindFrequency + o.vertex.x + o.vertex.z);
				float windZ = windStrengthXZ * _WindDirection.z * sin(_Time.z * _WindFrequency + o.vertex.x + o.vertex.z);
				o.vertex += float4(windX, windStrengthY * _WindDirection.y , windZ, 0);

				// 切割倾倒
				float cutStrengthY = v.uv.y;
				float cutStrengthXZ = v.uv.y;
				o.vertex += float4(_CutDirection.x * cutStrengthXZ, _CutDirection.y * cutStrengthY, _CutDirection.z * cutStrengthXZ, 0);

                UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv) * _BaseColor;
                UNITY_APPLY_FOG(i.fogCoord, color);
                UNITY_OPAQUE_ALPHA(color.a);
                color = lerp(color, _TopColor, i.uv.y);
                return color;
            }

        ENDCG
    }
}

}
