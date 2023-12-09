Shader "DGM/Advanced/FloorReflection"
{
	Properties
	{
		_AlphaScale("透明度", Range(0, 1)) = 1
		_MainTex("地板贴图",2D) = ""
	}

		SubShader
	{
		Tags { "Queue" = "Geometry" "IngnoreProjector" = "True" "RenderType" = "Opaque" }
		LOD 200
		Pass
		{
			Tags { "LightMode" = "UniversalForward" }


			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			sampler2D _ReflectionTex;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _AlphaScale;
			half _StartPixel;
			half _Fade;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			v2f vert(appdata v)
			{
				v2f f;
				f.position = TransformObjectToHClip(v.vertex);
				f.screenPos = ComputeScreenPos(f.position);
				f.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return f;
			}


			half4 frag(v2f i) : SV_Target
			{
				
				half textColor = tex2D(_ReflectionTex, i.screenPos.xy / i.screenPos.w);
				half4 color = textColor * saturate(((i.screenPos.xy / i.screenPos.w).y - _StartPixel + 0.5) / 0.5) * _AlphaScale;
				color += tex2D(_MainTex, i.uv);
				color.a = 1;
				return color;
			}
				ENDHLSL
			}

	}
}
