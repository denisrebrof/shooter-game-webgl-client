Shader "Unlit/MinimapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTexOpacity ("Texture Opacity", Range(0.0, 1.0)) = 1.0
        _MarkTex ("Mark", 2D) = "white" {}
        _EnemyTex ("Enemy", 2D) = "white" {}
        _AllyTex ("Ally", 2D) = "white" {}
        _EnemyMarkColor ("Enemy Mark Color", COLOR) = (1,1,1,1)
        _AllyMarkColor ("Ally Mark Color", COLOR) = (1,1,1,1)
        _PlayerMarkSize ("Player Mark Size", Range(0.0, 0.5)) = 0.2
        _EnemyMarkSize ("Enemy Mark Size", Range(0.0, 0.5)) = 0.2
        _AllyMarkSize ("Ally Mark Size", Range(0.0, 0.5)) = 0.2
        _PlayerPosX ("Player Pos X", Range(0.0, 1.0)) = 0.5
        _PlayerPosY ("Player Pos Y", Range(0.0, 1.0)) = 0.5
        _PlayerRot ("Player Rot", Range(-10.0, 10.0)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MarkTex;
            float4 _MarkTex_ST;

            sampler2D _EnemyTex;
            float4 _EnemyTex_ST;

            sampler2D _AllyTex;
            float4 _AllyTex_ST;

            float _PlayerPosX;
            float _PlayerPosY;
            float _PlayerMarkSize;
            float _PlayerRot;
            float _EnemyMarkSize;
            float _AllyMarkSize;
            float _MainTexOpacity;

            int _EnemiesCount = 0;
            int _AllyStartIndex = 0;
            float _EnemiesPos[16*2];

            fixed4 _EnemyMarkColor;
            fixed4 _AllyMarkColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float get_relative_pos(const float min, const float max, const float pos)
            {
                const float rel_pos = pos - min;
                const float size = max - min;
                return clamp(rel_pos / size, 0, 1);
            }

            float2 rotate_uv(float2 uv, float2 pivot, float rotation)
            {
                const float cosa = cos(rotation);
                const float sina = sin(rotation);
                uv -= pivot;
                return float2(
                    cosa * uv.x - sina * uv.y,
                    cosa * uv.y + sina * uv.x
                ) + pivot;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _MainTexOpacity;
                
                const float half_mark_size_p = _PlayerMarkSize * 0.5;
                const float x_pos = get_relative_pos(_PlayerPosX - half_mark_size_p, _PlayerPosX + half_mark_size_p,
                    i.uv.x);
                const float y_pos = get_relative_pos(_PlayerPosY - half_mark_size_p, _PlayerPosY + half_mark_size_p,
                    i.uv.y);
                if (x_pos > 0 && y_pos > 0)
                {
                    float2 uv = float2(x_pos, y_pos);
                    uv = rotate_uv(uv, float2(0.5, 0.5), _PlayerRot);
                    col += tex2D(_MarkTex, uv);
                }

                const float half_enemy_mark_size = _EnemyMarkSize * 0.5;
                const float half_ally_mark_size = _AllyMarkSize * 0.5;
                for (int ind = 1; ind < _EnemiesCount * 2; ind+=2)
                {
                    const float player_x = _EnemiesPos[ind - 1];
                    const float player_y = _EnemiesPos[ind];
                    const bool is_ally = ind > _AllyStartIndex * 2;
                    const float half_mark_size = is_ally? half_ally_mark_size:half_enemy_mark_size;
                    const float player_x_pos = get_relative_pos(player_x - half_mark_size, player_x + half_mark_size,
                        i.uv.x);
                    const float player_y_pos = get_relative_pos(player_y - half_mark_size, player_y + half_mark_size,
                        i.uv.y);
                    if (player_x_pos > 0 && player_y_pos > 0)
                    {
                        const float2 uv = float2(player_x_pos, player_y_pos);
                        const float4 add_color = is_ally? tex2D(_AllyTex, uv): tex2D(_EnemyTex, uv);
                        const float4 type_color = is_ally? _AllyMarkColor: _EnemyMarkColor;
                        col = lerp(col, type_color, add_color.r);
                    }
                }
                
                return col;
            }
            ENDCG
        }
    }
}