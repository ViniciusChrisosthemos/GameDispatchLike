Shader "Custom/SpriteRadialFill"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _FillAmount ("Fill Amount", Range(0,1)) = 1
        _StartAngle ("Start Angle (Rad)", Range(0,6.28)) = 1.57
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _FillAmount;
            float _StartAngle;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // centro da textura (assumindo pivô no centro)
                float2 uv = i.uv - 0.5;

                float angle = atan2(uv.y, uv.x);
                if (angle < 0) angle += 6.2831853; // normaliza para [0, 2π]

                float dist = length(uv);
                float filledAngle = _FillAmount * 6.2831853;

                bool visible = (angle >= _StartAngle && angle <= _StartAngle + filledAngle);

                if (!visible) discard;

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
