Shader "Custom/Waves" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Range(0, 1)) = 0.1
        _Length ("Length", Range(0, 10)) = 1
        _Speed ("Speed", Range(0, 10)) = 1
        _Offset ("Offset", Range(0, 1)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Amplitude;
            float _Length;
            float _Speed;
            float _Offset;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.vertex.xy * 0.5 + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float x = i.worldPos.x;
                float z = i.worldPos.z;

                float dx = x - _Offset;
                float dz = z - _Offset;
                float dist = sqrt(dx * dx + dz * dz);
                float wavePhase = dist / _Length + _Time.y * _Speed;
                float waveHeight = _Amplitude * sin(wavePhase * 2 * 3.14159265358979323846);
                float waveDx = -2 * 3.14159265358979323846 * _Amplitude * _Speed / _Length * cos(wavePhase * 2 * 3.14159265358979323846) * dx / dist;
                float waveDz = -2 * 3.14159265358979323846 * _Amplitude * _Speed / _Length * cos(wavePhase * 2 * 3.14159265358979323846) * dz / dist;

                i.vertex.y += waveHeight;
                float3 offset = float3(waveDx, 0, waveDz);
                i.vertex.xyz += offset;

                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor;
            }
            ENDCG
        }
    }
}
