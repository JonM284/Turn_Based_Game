Shader "Jettelly/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Space(10)]
		_OutColor("Outline Color", Color) = (1, 1, 1, 1)
		_OutValue("Outer Outline", Range(0.0, 0.2)) = 0.1
		_InnValue("Inner Outline", Range(1, 3)) = 2
		_MainCol("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {        
        Pass
        {
            Name "Outer Outline"
            Tags
            {
				"RenderPipeline" = "UniversalPipeline"
				"LightMode" = "UniversalForward"
                "Queue"="Transparent"
            }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
            };

            sampler2D _MainTex;
            float4 _OutColor;
            float _OutValue;

            float4 outline(float4 vertexPos, float outValue)
            {
                float4x4 scale = float4x4
                (
                    1 + outValue, 0, 0, 0,
                    0, 1 + outValue, 0, 0, 
                    0, 0, 1 + outValue, 0,
                    0, 0, 0, 1 + outValue
                );

                return mul(scale, vertexPos);
            }

            v2f vert (appdata v)
            {
                v2f o;
                float4 vertexPos = outline(v.vertex, _OutValue);
                o.vertex = UnityObjectToClipPos(vertexPos);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                fixed4 col = tex2D(_MainTex, i.uv);
                return float4(_OutColor.r, _OutColor.g, _OutColor.b, col.a);
            }
            ENDCG
        }
        
        Pass
        {
            Name "Main Color"
            Tags
            {
                "Queue"="Transparent+1"
            }

            Blend SrcAlpha OneMinusSrcAlpha

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                fixed4 col = tex2D(_MainTex, i.uv);

                return col * _MainCol;
            }
            ENDCG
        }

        Pass
        {
            Name "Inner Outline"
            Tags
            {
                "Queue"="Transparent+2"
            }

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _InnValue;
            float4 _OutColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal_world = UnityObjectToWorldNormal(v.normal);
                o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                half3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);
                half3 normal = i.normal_world;
                fixed output = 0;
                Unity_FresnelEffect_float(normal, viewDir, _InnValue, output);
                output = step(0.5, output) ;

                return output * _OutColor;
            }
            ENDCG
        }
    }
}
