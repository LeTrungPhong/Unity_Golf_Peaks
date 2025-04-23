Shader "Custom/OpacityFade"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _Opacity ("Opacity", Range(0, 1)) = 1.0 // Giá trị từ 0 (hoàn toàn trong suốt) đến 1 (không trong suốt)
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Các tham số
            fixed4 _Color;
            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Kết hợp màu với opacity
                fixed4 col = _Color;
                col.a = _Opacity; // Thay đổi alpha từ 0 đến 1
                return col;
            }
            ENDCG
        }
    }

    FallBack "Transparent/Diffuse"
}
