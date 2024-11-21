Shader "Custom/FillShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _CutoffHeight ("Cutoff Height", Float) = 0.5
        _GrayColor ("Gray Color", Color) = (0.5, 0.5, 0.5, 1)
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

            sampler2D _MainTex;
            float _CutoffHeight;
            fixed4 _GrayColor;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Eðer world Y pozisyonu cutoff'dan küçükse gri yap
                if (i.worldPos.y < _CutoffHeight)
                {
                    return _GrayColor;
                }

                // Aksi halde dokuyu uygula
                return tex2D(_MainTex, i.worldPos.xz);
            }
            ENDCG
        }
    }
}
