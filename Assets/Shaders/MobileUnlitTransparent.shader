Shader "Unlit/MobileTransparentInstancedWithColorAndAlpha"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondColor ("Second Color", Color) = (1, 0, 0, 1)
        _ThresholdMin ("Threshold Min", Range(0, 1)) = 0.0
        _ThresholdMax ("Threshold Max", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 200

        Cull Back

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _SecondColor;
            float _ThresholdMin;
            float _ThresholdMax;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 texColor = tex2D(_MainTex, i.uv);
                half4 col;

                // Плавный переход между текстурой и вторым цветом
                float blendFactor = smoothstep(_ThresholdMin, _ThresholdMax, i.uv.y);
                col = lerp(_SecondColor, texColor * _Color, blendFactor);

                // Прозрачность на противоположном конце объекта
                if (i.uv.y > 0.9)
                {
                    float alphaFactor = smoothstep(0.9, 1.0, i.uv.y);
                    col.a *= (1.0 - alphaFactor);
                }

                clip(col.a - 0.01);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
