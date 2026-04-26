Shader "Custom/ParadoxScanline"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ScanColor ("Scanline Color", Color) = (0, 1, 0.5, 1)
        _ScanSpeed ("Scan Speed", Float) = 2.0
        _ScanWidth ("Scan Width", Float) = 0.1
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _ScanColor;
            float _ScanSpeed;
            float _ScanWidth;
            float _EmissionStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;
                
                // Scanline effect logic
                float scan = sin((i.worldPos.y * 5.0) + (_Time.y * _ScanSpeed));
                scan = smoothstep(1.0 - _ScanWidth, 1.0, scan);

                fixed3 finalColor = col.rgb + (_ScanColor.rgb * scan * _EmissionStrength);
                return fixed4(finalColor, col.a);
            }
            ENDCG
        }
    }
}
