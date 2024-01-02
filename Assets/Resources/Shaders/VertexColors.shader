Shader "Custom/VertexColors" {
	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		Pass{
			Tags { "RenderPipeline" = "UniversalPipeline" }
			
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct appdata {
				float4 vertex : POSITION;
				half4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f {
				float4 vertex : SV_POSITION;
				float4 vertexColor: COLOR0;
			};
			
			// half4 _Color;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.vertexColor = v.color;

				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				half4 col = i.vertexColor;
				col.a = 1;
				return col;
			}
			
			ENDHLSL
		}
	}

	
	FallBack "Diffuse"
}